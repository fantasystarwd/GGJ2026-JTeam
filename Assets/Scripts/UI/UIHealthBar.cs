using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    private SlicedFilledImage _fill;
    [SerializeField]
    private TMP_Text _text;

    public void SetValue(int current, int max)
    {
        float fillAmount = (float)current / max;
        _fill.fillAmount = fillAmount;
        _text.text = $"{current}/{max}";
    }
}
