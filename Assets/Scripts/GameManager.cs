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
    private Camera _gameCamera;
    [SerializeField]
    private Camera _uiCamera;
    [SerializeField]
    private GameObject _levelRoot;
    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Transform _player;
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
        _uiMain.ButtonBackpackClicked += OpenUIBackpack;
        _uiBackpack.ButtonCloseClicked += CloseUIBackpack;
        _uiBackpack.ButtonUseClicked += UseItem;

        _uiScreenFader.ForceFadeIn();
        _uiTextBubble.ForceHide();

        _uiMain.Show();
        _uiBackpack.Hide();

        _isRunning = true;
        _healthCurrent = _healthMax;
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

    public void OnTimeReset()
    {
        TimeResetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid TimeResetAsync(CancellationToken cancellationToken)
    {
        _isRunning = false;

        await _uiScreenFader.FadeOutAsync(1.0f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        // Reset player position
        _player.position = new Vector3(
            _startPosition.position.x, _startPosition.position.y, _player.position.z);

        // Reset backpack
        // Reset hp
        _healthCurrent = _healthMax;
        // Reset level items

        await _uiScreenFader.FadeInAsync(1.0f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        _isRunning = true;
    }

    private void OpenUIBackpack()
    {
        _isRunning = false;
        _uiMain.Hide();
        RefreshUIBackpack();
        _uiBackpack.SelectSlotIndex(0);
        _uiBackpack.Show();
    }

    private void CloseUIBackpack()
    {
        _isRunning = true;
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
        if (index < 0 || index >= _inventoryManager.items.Count)
        {
            return;
        }

        Debug.Log($"Use item at index {index}");
        //_inventoryManager.UseItem(index);
        RefreshUIBackpack();
        _uiBackpack.SelectSlotIndex(index);
    }

    public void ShowTextBubble(Transform pivot, string text)
    {
        _uiTextBubble.Show(pivot, _gameCamera, _uiCamera, text);
    }

    public void ChangeLevel(GameObject levelRoot, Vector3 playerStartPosition)
    {
        if (_levelRoot != null)
        {
            _levelRoot.SetActive(false);
        }
        _levelRoot = levelRoot;
        _levelRoot.SetActive(true);
        _player.position = playerStartPosition;
    }
}
