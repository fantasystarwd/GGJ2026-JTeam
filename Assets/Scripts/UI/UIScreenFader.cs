using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIScreenFader : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public void ForceFadeIn()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.gameObject.SetActive(false);
    }

    public async UniTask FadeInAsync(float duration, CancellationToken cancellationToken)
    {
        _canvasGroup.gameObject.SetActive(true);

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

        _canvasGroup.gameObject.SetActive(false);
    }

    public async UniTask FadeOutAsync(float duration, CancellationToken cancellationToken)
    {
        _canvasGroup.gameObject.SetActive(true);

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
