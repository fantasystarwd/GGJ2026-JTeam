using System;

[Serializable]
public struct InventoryItem
{
    public InteractiveConditionType itemType;
    public MaskClass maskClass;
    public AccessoriesType accessoriesType;
    public PropType propType;

    public string GetItemID()
    {
        switch (itemType)
        {
            case InteractiveConditionType.MaskClass: 
                return maskClass.ToString();
            case InteractiveConditionType.AccessoriesType:
                return accessoriesType.ToString();
            case InteractiveConditionType.ObjectType:
                return propType.ToString();
            default:
                return "Unknown";
        }
    }
}
