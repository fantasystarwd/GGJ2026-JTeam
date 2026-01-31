using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public event Action ButtonBackpackClicked;

    [SerializeField]
    private Button _buttonBackpack;

    private void Awake()
    {
        _buttonBackpack.onClick.AddListener(OnButtonBackpackClick);
    }

    private void OnButtonBackpackClick()
    {
        ButtonBackpackClicked?.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
