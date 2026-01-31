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
    private Transform _selectionHighlight;
    [SerializeField]
    private List<UIBackpackSlot> _slots;

    private int _selectedSlotIndex = -1;

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
        SelectSlotIndex(index);
    }

    private void Update()
    {
        NavigateSlot();
    }

    public void Show()
    {
        SelectSlotIndex(0);
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

    public void SelectSlotIndex(int index)
    {
        if (index < 0 || index >= _slots.Count)
        {
            _selectedSlotIndex = -1;
            _selectionHighlight.gameObject.SetActive(false);
            return;
        }

        _selectedSlotIndex = index;
        _selectionHighlight.transform.position = _slots[index].transform.position;
        _selectionHighlight.gameObject.SetActive(true);
    }

    // Assume 4 slot per row
    // Use WSAD or Arrow keys to navigate
    private void NavigateSlot()
    {
        const int SlotsPerRow = 4;

        if (_selectedSlotIndex == -1)
        {
            _selectedSlotIndex = 0;
            SelectSlotIndex(_selectedSlotIndex);
            return;
        }

        int previousIndex = _selectedSlotIndex;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_selectedSlotIndex - SlotsPerRow >= 0)
            {
                _selectedSlotIndex -= SlotsPerRow;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_selectedSlotIndex + SlotsPerRow < _slots.Count)
            {
                _selectedSlotIndex += SlotsPerRow;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_selectedSlotIndex % SlotsPerRow - 1 >= 0)
            {
                _selectedSlotIndex -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_selectedSlotIndex % SlotsPerRow + 1 < SlotsPerRow)
            {
                _selectedSlotIndex += 1;
            }
        }

        if (previousIndex != _selectedSlotIndex)
        {
            SelectSlotIndex(_selectedSlotIndex);
        }
    }
}
