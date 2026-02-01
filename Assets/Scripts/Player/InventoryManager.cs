using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int round = 1;
    public List<string> gainedMasks = new();
    public List<InventoryItem> items = new();

    public bool HasItem(string itemName)
    {
        return items.Exists(x => x.GetItemID() == itemName);
    }

    public void AddItem(InventoryItem result)
    {
        items.Add(result);
        Debug.Log($"獲得物品: {result.GetItemID()}。目前數量: {items.Count}");
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

    public void RemoveItemAtIndex(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Debug.LogWarning($"無效的物品索引: {index}");
            return;
        }

        items.RemoveAt(index);
    }
}
