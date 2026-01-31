using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackpack : MonoBehaviour
{
    public event Action ButtonCloseClicked;

    [SerializeField]
    private Button _buttonClose;
    [SerializeField]
    private UIHealthBar _uiHealthBar;
    [SerializeField]
    private ItemDataTable _itemDataTable;
    [SerializeField]
    private List<UIBackpackSlot> _slots;

    private void Start()
    {
        _buttonClose.onClick.AddListener(OnButtonCloseClicked);
        for (var i = 0; i < _slots.Count; i++)
        {
            int index = i;  // Capture the current index
            _slots[i].Clicked += () => OnSlotClicked(index);
        }
    }

    private void OnButtonCloseClicked()
    {
        ButtonCloseClicked?.Invoke();
    }

    private void OnSlotClicked(int index)
    {
        Debug.Log($"Slot {index} clicked.");
        // Handle slot click logic here
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetHealth(int current, int max)
    {
        _uiHealthBar.SetValue(current, max);
    }

    public void SetItems(IReadOnlyList<InventoryItem> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            string itemKey = items[i].GetItemID();
            if (string.IsNullOrEmpty(itemKey))
            {
                _slots[i].SetIcon(null);
                continue;
            }

            if (itemKey == "Unknown")
            {
                _slots[i].SetIcon(null);
                continue;
            }

            ItemData data = _itemDataTable.GetData(itemKey);
            if (data == null)
            {
                _slots[i].SetIcon(null);
                continue;
            }

            _slots[i].SetIcon(data.icon);
        }

        for (var i = items.Count; i < _slots.Count; i++)
        {
            _slots[i].SetIcon(null);
        }
    }
}
