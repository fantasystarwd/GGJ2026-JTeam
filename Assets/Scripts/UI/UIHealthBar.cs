using TMPro;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _container;
    [SerializeField]
    private SlicedFilledImage _fill;
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private float _widthPerValue = 2f;

    public void SetValue(int current, int max)
    {
        _container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, max * _widthPerValue);
        float fillAmount = (float)current / max;
        _fill.fillAmount = fillAmount;
        _text.text = $"{current}/{max}";
    }
}
