using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IconData
{
    public string key;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "IconDataTable", menuName = "IconDataTable")]
public class IconDataTable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<IconData> _dataList;

    private readonly Dictionary<string, Sprite> _dataDictionary = new();

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        _dataDictionary.Clear();
        for (var i = 0; i < _dataList.Count; i++)
        {
            IconData data = _dataList[i];
            _dataDictionary[data.key] = data.icon;
        }
    }

    public Sprite GetIcon(string key)
    {
        if (_dataDictionary.TryGetValue(key, out Sprite icon))
        {
            return icon;
        }
        return null;
    }
}
