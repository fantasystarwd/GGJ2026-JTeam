
/// <summary>
/// 面具職業定義
/// </summary>
public enum MaskClass
{
    None, // 未裝備面具時
    Cook,
}

/// <summary>
/// 飾品類別定義
/// </summary>
public enum AccessoriesType
{
    None,
}

/// <summary>
/// 道具類別定義
/// </summary>
public enum PropType
{
    None,
    mushroom,
    potato,
    corn,
    rawmeat,
    mushroomsoup,
    fries,
    roastedcorn,
    grilledporkchops
}

/// <summary>
/// 玩家持有道具類別定義
/// </summary>
public enum ObjectType
{
    Mask, // 面具
    accessories, // 飾品
    Prop, // 道具
}
