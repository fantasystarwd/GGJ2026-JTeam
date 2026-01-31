using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UITextBubble : MonoBehaviour
{
    [SerializeField]
    private RectTransform _root;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private float _keepDuration = 2f;
    [SerializeField]
    private float _fadeDuration = 0.3f;

    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _root = transform.parent as RectTransform;
    }

    private void OnDestroy()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    public void ForceHide()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
        _canvasGroup.alpha = 0f;
    }

    public void Show(Transform pivot, Camera gameCamera, Camera uiCamera, string text)
    {
        ForceHide();
        _cancellationTokenSource = new CancellationTokenSource();
        ShowAsync(pivot, gameCamera, uiCamera, text, _cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid ShowAsync(Transform pivot, Camera gameCamera, Camera uiCamera,
        string text, CancellationToken cancellationToken)
    {
        _text.text = text;
        Vector3 screenPos = gameCamera.WorldToScreenPoint(pivot.position);
        _ = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _root, screenPos, uiCamera, out Vector2 localPoint);
        (transform as RectTransform).anchoredPosition = localPoint;

        _canvasGroup.alpha = 1f;

        await UniTask.Delay((int)(_keepDuration * 1000), cancellationToken: cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        float elapsed = 0f;
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);
            await UniTask.Yield(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
}
