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

    public void ShowIcon()
    {
        _icon.gameObject.SetActive(true);
    }

    public void HideIcon()
    {
        _icon.gameObject.SetActive(false);
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }
}
