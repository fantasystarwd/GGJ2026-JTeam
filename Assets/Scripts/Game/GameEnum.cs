
/// <summary>
/// 面具職業定義
/// </summary>
public enum MaskClass
{
    None, // 未裝備面具時
    White,
    Cook,
	Thief,
	Artisan,
	Wrestler,
	Scholar,
	Prism,
	oxygenMask,
	StoneMask,
	MemoryMask
}

/// <summary>
/// 飾品類別定義
/// </summary>
public enum AccessoriesType
{
    None,
	Glassbead,
	Feather,
	Icecrystal,
	Pyroxene,
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
