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
    private List<UIBackpackSlot> _slots;

    private void Start()
    {
        _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
        for (var i = 0; i < _slots.Count; i++)
        {
            int index = i;  // Capture the current index
            _slots[i].Clicked += () => OnSlotClicked(index);
        }
    }

    private void OnSlotClicked(int index)
    {
        Debug.Log($"Slot {index} clicked.");
        // Handle slot click logic here
    }
}
