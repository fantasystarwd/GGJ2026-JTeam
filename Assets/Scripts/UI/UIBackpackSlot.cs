using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour
{
    public event Action Clicked;

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _icon;

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Clicked?.Invoke();
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
        _icon.gameObject.SetActive(icon != null);
    }
}
