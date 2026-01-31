using System;

[Serializable]
public struct InventoryItem
{
    public InteractiveResultType itemType;
    public MaskClass maskClass;
    public AccessoriesType accessoriesType;
    public PropType propType;

    public string GetItemID()
    {
        switch (itemType)
        {
            case InteractiveResultType.GetObject: 
                return propType.ToString();
            case InteractiveResultType.ShowMessage: 
                if (maskClass != MaskClass.None) return maskClass.ToString();
                if (accessoriesType != AccessoriesType.None) return accessoriesType.ToString();
                break;
        }
        return "Unknown";
    }
}
