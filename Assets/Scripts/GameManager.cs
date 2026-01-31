using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private UIScreenFader _uiScreenFader;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnTimeReset();
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
}
