using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveConditionType
{
    None,
    MaskClass,
    AccessoriesType,
    ObjectType,
}

public enum InteractiveResultType
{
    ShowMessage,
    WearMask,
    GetObject,
    OpenObstacle,
    Cooking,
}

/// <summary>
/// 互動條件定義
/// </summary>
[System.Serializable]
public struct InteractiveCondition
{
    public InteractiveConditionType conditionType;
    public bool isAntiLogic;
    public MaskClass maskClass;
    public AccessoriesType accessoriesType;
    public ObjectType objectType;
}

/// <summary>
/// 互動結果定義
/// </summary>
[System.Serializable]
public struct InteractiveResult
{
    public InteractiveResultType resultType;
    public string message;
    public InventoryItem getObject;
    public InteractObject actObject;
}

public class InteractiveObjectBase : MonoBehaviour
{
    /// <summary>
    /// 互動條件
    /// </summary>
    [SerializeField]
    private InteractiveCondition[] interactiveConditions;

    /// <summary>
    /// 互動成功結果
    /// </summary>
    [SerializeField]
    private InteractiveResult[] interactiveSuccessResults;

    /// <summary>
    /// 互動失敗結果
    /// </summary>
    [SerializeField]
    private InteractiveResult[] interactiveFailResults;

    /// <summary>
    /// 互動成功後是否關閉此物件
    /// </summary>
    [SerializeField]
    private bool CloseIfSuccess = false;

    /// <summary>
    /// 顯示訊息茅點設定
    /// </summary>
    private Transform showMessageAnchor;

    /// <summary>
    /// 玩家與此物件互動時觸發
    /// </summary>
    public void Interact(PlayerMovement player)
    {
        // 條件判斷
        bool canInteract = true;
        foreach (var condition in interactiveConditions)
        {
            switch (condition.conditionType)
            {
                case InteractiveConditionType.MaskClass:
                    if (player.myCurrentMask != condition.maskClass != condition.isAntiLogic)
                    {
                        canInteract = false;
                    }
                    break;
                case InteractiveConditionType.AccessoriesType:

                    if ((!GameManager.Instance.HasItem(condition.accessoriesType.ToString())) != condition.isAntiLogic)
                    {
                        canInteract = false;
                    }
                    break;
                case InteractiveConditionType.ObjectType:

                    if ((!GameManager.Instance.HasItem(condition.objectType.ToString())) != condition.isAntiLogic)
                    {
                        canInteract = false;
                    }
                    break;
                case InteractiveConditionType.None:
                default:
                    break;
            }
            if (!canInteract)
            {
                break;
            }
        }

        if (!canInteract)
        {
            ProcessInteractiveResult(ref interactiveFailResults);
            return;
        }

        ProcessInteractiveResult(ref interactiveSuccessResults);

        if (CloseIfSuccess)
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        showMessageAnchor = transform.Find("ShowMessageAnchor");
        if (showMessageAnchor == null)
        {
            showMessageAnchor = transform;
        }
    }

    private void ProcessInteractiveResult(ref InteractiveResult[] results)
    {
        foreach (var result in results)
        {
            switch (result.resultType)
            {
                case InteractiveResultType.ShowMessage:
                    GameManager.Instance.ShowTextBubble(showMessageAnchor, result.message);
                    break;
                case InteractiveResultType.WearMask:
                    break;
                case InteractiveResultType.GetObject:
                    GameManager.Instance.AddItem(result.getObject);
                    break;
                case InteractiveResultType.OpenObstacle:
                    result.actObject.SetState(true);
                    break;
                case InteractiveResultType.Cooking:
                    for (PropType prop = PropType.mushroom; prop <= PropType.rawmeat; prop++)
                    {
                        if (GameManager.Instance.HasItem(prop.ToString()))
                        {
                            GameManager.Instance.RemoveItem(prop.ToString());
                            GameManager.Instance.AddItem(result.getObject);
                        }
                    }
                    break;
            }
        }
    }
}
