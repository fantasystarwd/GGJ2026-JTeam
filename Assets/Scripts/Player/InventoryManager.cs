using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

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
}
