using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void DoTimeReset()
    {
        TimeResetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid TimeResetAsync(CancellationToken cancellationToken)
    {
        // Save record
        // 1. 輪數紀錄+1
        round += 1;

        // 2. 已經獲得的面具，要被記錄起來，在初始房間內會出現
        for (var i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];
            if (item.itemType == InteractiveConditionType.MaskClass)
            {
                string itemId = item.GetItemID();
                if (!gainedMasks.Contains(itemId))
                {
                    gainedMasks.Add(itemId);
                }
            }
        }

        // Reset backpack
        // 面具不會保留在背包內。其他道具保留。
        items.RemoveAll(x =>
            x.itemType == InteractiveConditionType.MaskClass
        );

        await SceneManager.LoadSceneAsync("AlphaShow", LoadSceneMode.Single);
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
    }
}
