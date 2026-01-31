using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventoryItem> items = new List<InventoryItem>();
    public GameObject inventoryUI;
    public bool isOpen = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (inventoryUI != null) inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOpen)
        {
            OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseInventory();
        }
    }

    public void AddItem(InteractiveResult result)
    {
        InventoryItem newItem = new InventoryItem
        {
            itemType = result.resultType,
            maskClass = result.maskClass,
            accessoriesType = result.accessoriesType,
            propType = result.propType
        };

        items.Add(newItem);
        Debug.Log($"獲得物品: {newItem.GetItemID()}。目前數量: {items.Count}");
    }

    public void RemoveItem(string itemName)
    {
        InventoryItem toRemove = items.Find(x => x.GetItemID() == itemName);

        if (toRemove.GetItemID() == itemName)
        {
            items.Remove(toRemove);
            Debug.Log($"移除物品: {itemName}");
        }
    }

    public void OpenInventory()
    {
        isOpen = true;
        if (inventoryUI != null) inventoryUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInventory()
    {
        isOpen = false;
        if (inventoryUI != null) inventoryUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
