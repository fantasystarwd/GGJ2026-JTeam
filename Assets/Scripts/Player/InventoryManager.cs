using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("背包設定")]
    public List<string> items = new List<string>(); 
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


    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log($"獲得物品: {itemName}。目前數量: {items.Count}");
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
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