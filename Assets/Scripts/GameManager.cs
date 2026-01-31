using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const KeyCode KeyCodeBackpack = KeyCode.B;

    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private UIScreenFader _uiScreenFader;

    [Header("UI")]
    [SerializeField]
    private UIMain _uiMain;
    [SerializeField]
    private UIBackpack _uiBackpack;

    private void Start()
    {
        _uiMain.ButtonBackpackClicked += OpenUIBackpack;
        _uiBackpack.ButtonCloseClicked += CloseUIBackpack;

        _uiMain.Show();
        _uiBackpack.Hide();
        _uiScreenFader.ForceFadeIn();
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
        _uiBackpack.Show();
    }

    private void CloseUIBackpack()
    {
        _uiMain.Show();
        _uiBackpack.Hide();
    }
}
