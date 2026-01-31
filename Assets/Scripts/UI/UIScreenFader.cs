using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIScreenFader : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public async UniTask FadeInAsync(float duration, CancellationToken cancellationToken)
    {
        float elapsedTime = 0f;
        _canvasGroup.alpha = 1f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / duration);
            await UniTask.Yield(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
        }
        _canvasGroup.alpha = 0f;
    }

    public async UniTask FadeOutAsync(float duration, CancellationToken cancellationToken)
    {
        float elapsedTime = 0f;
        _canvasGroup.alpha = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            await UniTask.Yield(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
        }
        _canvasGroup.alpha = 1f;
    }
}
