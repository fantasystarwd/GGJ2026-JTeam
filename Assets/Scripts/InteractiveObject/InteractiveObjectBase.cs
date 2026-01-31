using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveConditionType
{
    None,
    MaskClass,
    ObjectType,
}

public enum InteractiveResultType
{
    ShowMessage,
    GetObject,
    OpenObstacle
}

/// <summary>
/// 互動條件定義
/// </summary>
[System.Serializable]
public struct InteractiveCondition
{
    public InteractiveConditionType conditionType;
    public MaskClass maskClass;
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
    public MaskClass maskClass;
    public AccessoriesType accessoriesType;
    public PropType propType;
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
    /// 玩家與此物件互動時觸發
    /// </summary>
    public void Interact()
    {
        // 條件判斷
        bool canInteract = true;
        foreach (var condition in interactiveConditions)
        {
            switch (condition.conditionType)
            {
                //case InteractiveConditionType.MaskClass:
                //    if (GameManager.Instance.Player.MaskClass != condition.maskClass)
                //    {
                //        canInteract = false;
                //    }
                //    break;
                //case InteractiveConditionType.ObjectType:
                //    if (!GameManager.Instance.Player.HasObjectType(condition.objectType))
                //    {
                //        canInteract = false;
                //    }
                //    break;
                case InteractiveConditionType.None:
                default:
                    break;
            }
            if (!canInteract)
            {
                break;
            }
        }

        if(!canInteract)
        {
            Debug.Log($"[Interactive] Fail");
            ProcessInteractiveResult(ref interactiveFailResults);
            return;
        }

        Debug.Log($"[Interactive] Success");
        ProcessInteractiveResult(ref interactiveSuccessResults);

        if (CloseIfSuccess)
        {
           gameObject.SetActive(false);
        }
    }

    private void ProcessInteractiveResult(ref InteractiveResult[] results)
    {
        foreach (var result in results)
        {
            switch (result.resultType)
            {
                case InteractiveResultType.ShowMessage:
                    Debug.Log($"[Interactive] {result.message}");
                    break;
                case InteractiveResultType.GetObject:
                    break;
                case InteractiveResultType.OpenObstacle:
                    Debug.Log("[Interactive] Obstacle opened.");
                    result.actObject.SetState(true);
                    break;
            }
        }
    }
}
