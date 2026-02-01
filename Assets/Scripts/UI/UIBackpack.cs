using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackpack : MonoBehaviour
{
    public event Action ButtonCloseClicked;
    public event Action<int> ButtonUseClicked;

    [SerializeField]
    private Button _buttonClose;
    [SerializeField]
    private Button _buttonUse;
    [SerializeField]
    private UIHealthBar _uiHealthBar;
    [SerializeField]
    private ItemDataTable _itemDataTable;
    [SerializeField]
    private Transform _selectionHighlight;
    [SerializeField]
    private List<UIBackpackSlot> _slots;

    private int _selectedSlotIndex = -1;
    private readonly List<InventoryItem> _cachedItems = new();

    private void Start()
    {
        _buttonClose.onClick.AddListener(OnButtonCloseClicked);
        _buttonUse.onClick.AddListener(OnButtonUseClicked);
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

    private void OnButtonUseClicked()
    {
        ButtonUseClicked?.Invoke(_selectedSlotIndex);
    }

    private void OnSlotClicked(int index)
    {
        SelectSlotIndex(index);
    }

    private void Update()
    {
        NavigateSlot();
        if (Input.GetKeyDown(GameManager.KeyCodeBackpackUse))
        {
            OnButtonUseClicked();
        }
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
        _cachedItems.Clear();
        int minCount = Math.Min(items.Count, _slots.Count);
        for (var i = 0; i < minCount; i++)
        {
            _cachedItems.Add(items[i]);
        }

        for (var i = 0; i < _cachedItems.Count; i++)
        {
            string itemId = _cachedItems[i].GetItemID();
            if (string.IsNullOrEmpty(itemId))
            {
                _slots[i].SetIcon(null);
                _slots[i].ShowIcon();
                continue;
            }

            string itemDataId = $"{_cachedItems[i].GetTypeId()}-{itemId}";
            ItemData data = _itemDataTable.GetData(itemDataId);
            if (data == null)
            {
                _slots[i].SetIcon(null);
                _slots[i].ShowIcon();
                continue;
            }

            _slots[i].SetIcon(data.icon);
            _slots[i].ShowIcon();
        }

        for (var i = _cachedItems.Count; i < _slots.Count; i++)
        {
            _slots[i].SetIcon(null);
            _slots[i].HideIcon();
        }
    }

    public void SelectSlotIndex(int index)
    {
        if (index < 0 || index >= _slots.Count)
        {
            _selectedSlotIndex = -1;
            _selectionHighlight.gameObject.SetActive(false);
            _buttonUse.gameObject.SetActive(false);
            return;
        }

        _selectedSlotIndex = index;
        _selectionHighlight.transform.position = _slots[index].transform.position;
        _selectionHighlight.gameObject.SetActive(true);

        bool isItemUsable = false;
        if (index >= 0 && index < _cachedItems.Count)
        {
            InventoryItem item = _cachedItems[index];
            ItemData data = _itemDataTable.GetData(item.GetItemID());
            if (data != null)
            {
                isItemUsable = data.isUsable;
            }
        }
        _buttonUse.gameObject.SetActive(isItemUsable);
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
