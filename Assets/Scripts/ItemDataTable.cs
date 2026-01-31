using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public string id;
    public string name;
    public Sprite icon;
    public int value;
    public bool isUsable;
}

[CreateAssetMenu(fileName = "ItemDataTable", menuName = "ItemDataTable")]
public class ItemDataTable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<ItemData> _dataList;

    private readonly Dictionary<string, ItemData> _dataTable = new();

    public ItemData GetData(string itemId)
    {
        if (_dataTable.TryGetValue(itemId, out ItemData itemData))
        {
            return itemData;
        }
        return null;
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        _dataTable.Clear();
        for (var i = 0; i < _dataList.Count; i++)
        {
            ItemData data = _dataList[i];
            _dataTable[data.id] = data;
        }
    }
}
