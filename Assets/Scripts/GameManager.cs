using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const KeyCode KeyCodeBackpack = KeyCode.B;

    [SerializeField]
    private InventoryManager _inventoryManager;
    [SerializeField]
    private Camera _gameCamera;
    [SerializeField]
    private Camera _uiCamera;
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

        _uiScreenFader.ForceFadeIn();
        _uiTextBubble.ForceHide();

        _uiMain.Show();
        _uiBackpack.Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnTimeReset();
        }

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

        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowTextBubble(_player, "Hello, this is a text bubble!");
        }
    }

    public void OnTimeReset()
    {
        TimeResetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid TimeResetAsync(CancellationToken cancellationToken)
    {
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
        // Reset level items

        await _uiScreenFader.FadeInAsync(1.0f, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }

    private void OpenUIBackpack()
    {
        _uiMain.Hide();
        _uiBackpack.SetItems(_inventoryManager.items);
        _uiBackpack.Show();
    }

    private void CloseUIBackpack()
    {
        _uiMain.Show();
        _uiBackpack.Hide();
    }

    public bool HasItem(string itemName)
    {
        return _inventoryManager.HasItem(itemName);
    }

    public void AddItem(InteractiveResult result)
    {
        _inventoryManager.AddItem(result);
    }

    public void RemoveItem(string itemName)
    {
        _inventoryManager.RemoveItem(itemName);
    }

    public void ShowTextBubble(Transform pivot, string text)
    {
        _uiTextBubble.Show(pivot, _gameCamera, _uiCamera, text);
    }
}
