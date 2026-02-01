using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const KeyCode KeyCodeBackpack = KeyCode.B;
    public const KeyCode KeyCodeBackpackUse = KeyCode.E;

    [SerializeField]
    private InventoryManager _inventoryManager;
    [SerializeField]
    private ItemDataTable _itemDataTable;
    [SerializeField]
    private Camera _gameCamera;
    [SerializeField]
    private Camera _uiCamera;
    [SerializeField]
    private GameObject _levelRoot;
    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private PlayerMovement _player;
    [SerializeField]
    private UIScreenFader _uiScreenFader;
    [SerializeField]
    private UITextBubble _uiTextBubble;

    [Header("UI")]
    [SerializeField]
    private UIMain _uiMain;
    [SerializeField]
    private UIBackpack _uiBackpack;

    [Header("Data")]
    [SerializeField]
    private bool _isRunning;
    [SerializeField]
    private float _healthCurrent;
    [SerializeField]
    private int _healthMax = 180;
    [SerializeField]
    private float _costPerSecond = 1f;

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerMovement>();
        }

        if (_inventoryManager == null)
        {
            _inventoryManager = FindObjectOfType<InventoryManager>();
            if (_inventoryManager == null)
            {
                _inventoryManager = new GameObject("InventoryManager").AddComponent<InventoryManager>();
                DontDestroyOnLoad(_inventoryManager);
            }
        }

        _uiMain.ButtonBackpackClicked += OpenUIBackpack;
        _uiBackpack.ButtonCloseClicked += CloseUIBackpack;
        _uiBackpack.ButtonUseClicked += UseItem;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void Start()
    {
        _uiTextBubble.ForceHide();
        _uiMain.Show();
        _uiBackpack.Hide();

        _healthCurrent = _healthMax;
        SetupNewRoundAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCodeBackpack))
        {
            if (_uiBackpack.gameObject.activeSelf)
            {
                CloseUIBackpack();
            }
            else
            {
                OpenUIBackpack();
            }
        }

        if (_isRunning)
        {
            CostHealth();
            _uiMain.SetHealth(Mathf.CeilToInt(_healthCurrent), _healthMax);
        }
    }

    private void CostHealth()
    {
        _healthCurrent -= _costPerSecond * Time.deltaTime;
        if (_healthCurrent <= 0)
        {
            _healthCurrent = 0;
            OnTimeReset();
        }
    }

    public void PauseGame()
    {
        _isRunning = false;
        _player.DisabledMovement(true);
    }

    public void ResumeGame()
    {
        _isRunning = true;
        _player.DisabledMovement(false);
    }

    private async UniTask SetupNewRoundAsync(CancellationToken cancellationToken)
    {
        PauseGame();
        _uiScreenFader.ForceFadeOut();

        // Reset start level
        // 已經獲得的面具，在初始房間內會出現
        if (_levelRoot != null)
        {
            InteractiveObjectBase[] allInteractiveObjects = _levelRoot.GetComponentsInChildren<InteractiveObjectBase>(true);
            for (var i = 0; i < allInteractiveObjects.Length; i++)
            {
                InteractiveObjectBase interactiveObject = allInteractiveObjects[i];
                string maskId = interactiveObject.GetMaskIdIfAble();
                if (string.IsNullOrEmpty(maskId))
                {
                    continue;
                }

                if (!_inventoryManager.HasMask(maskId))
                {
                    interactiveObject.gameObject.SetActive(false);
                }
            }
        }

        await _uiScreenFader.FadeInAsync(1.0f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        ResumeGame();
    }

    public void OnTimeReset()
    {
        TimeResetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid TimeResetAsync(CancellationToken cancellationToken)
    {
        PauseGame();

        await _uiScreenFader.FadeOutAsync(1.0f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        _inventoryManager.DoTimeReset();
    }

    private void OpenUIBackpack()
    {
        PauseGame();
        _uiMain.Hide();
        RefreshUIBackpack();
        _uiBackpack.SelectSlotIndex(0);
        _uiBackpack.Show();
    }

    private void CloseUIBackpack()
    {
        ResumeGame();
        _uiMain.Show();
        _uiBackpack.Hide();
    }

    private void RefreshUIBackpack()
    {
        _uiBackpack.SetHealth(Mathf.CeilToInt(_healthCurrent), _healthMax);
        _uiBackpack.SetItems(_inventoryManager.items);
    }

    public bool HasItem(string itemName)
    {
        return _inventoryManager.HasItem(itemName);
    }

    public void AddItem(InventoryItem result)
    {
        _inventoryManager.AddItem(result);
    }

    public void RemoveItem(string itemName)
    {
        _inventoryManager.RemoveItem(itemName);
    }

    public void UseItem(int index)
    {
        bool success = UseItemInternal(index);
        if (!success)
        {
            return;
        }

        RefreshUIBackpack();
        _uiBackpack.SelectSlotIndex(index);
    }

    private bool UseItemInternal(int index)
    {
        if (index < 0 || index >= _inventoryManager.items.Count)
        {
            return false;
        }

        InventoryItem itemToUse = _inventoryManager.items[index];
        string itemId = itemToUse.GetItemID();
        ItemData itemData = _itemDataTable.GetData(itemId);
        if (itemData == null)
        {
            Debug.LogWarning($"找不到物品資料: {itemId}");
            return false;
        }

        if (!itemData.isUsable)
        {
            return false;
        }

        _healthCurrent += itemData.value;
        if (_healthCurrent > _healthMax)
        {
            _healthCurrent = _healthMax;
        }

        _inventoryManager.RemoveItemAtIndex(index);
        return true;
    }

    public void ShowTextBubble(Transform pivot, string text)
    {
        _uiTextBubble.Show(pivot, _gameCamera, _uiCamera, text);
    }

    public void ChangeLevel(GameObject levelRoot, Vector3 playerStartPosition)
    {
        ChangeLevelAsync(levelRoot, playerStartPosition, this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid ChangeLevelAsync(GameObject levelRoot,
        Vector3 playerStartPosition, CancellationToken cancellationToken)
    {
        PauseGame();

        await _uiScreenFader.FadeOutAsync(0.5f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        // Switch level
        if (_levelRoot != null)
        {
            _levelRoot.SetActive(false);
        }
        _levelRoot = levelRoot;
        if (_levelRoot != null)
        {
            // Note: Game space is on xz-plane
            _gameCamera.transform.position = new Vector3(
                _levelRoot.transform.position.x,
                _gameCamera.transform.position.y,
                _levelRoot.transform.position.z);
            _levelRoot.SetActive(true);
        }

        // Reset player position
        if (_player != null)
        {
            _player.transform.position = playerStartPosition;
        }

        await _uiScreenFader.FadeInAsync(0.5f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        ResumeGame();
    }
}
