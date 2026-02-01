using UnityEngine;
using UnityEngine.SceneManagement;

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
    ChangeLevel,
    PlaySoundEffect,
    WinGame,
}

/// <summary>
/// 轉換場景資訊
/// </summary>
[System.Serializable]
public struct ChangeLevelInfo
{
    public GameObject levelRoot;
    public Transform playerStartPosition;
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
    public ChangeLevelInfo changeLevelInfo;
    public SoundEffectType soundEffectType;
}

public class InteractiveObjectBase : MonoBehaviour
{
    /// <summary>
    /// 玩家一進入這個觸發就會互動
    /// </summary>
    [SerializeField]
    private bool WithoutInteract;
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
    /// 是否以觸發過了
    /// </summary>
    private bool isInteracted = false;

    /// <summary>
    /// 顯示訊息茅點設定
    /// </summary>
    private Transform showMessageAnchor;

    public string GetMaskIdIfAble()
    {
        for (var i = 0; i < interactiveSuccessResults.Length; i++)
        {
            if (interactiveSuccessResults[i].resultType == InteractiveResultType.WearMask)
            {
                return interactiveSuccessResults[i].getObject.maskClass.ToString();
            }
        }

        return "";
    }

    /// <summary>
    /// 玩家一進入這個觸發就會判斷是否能直接互動
    /// </summary>
    /// <param name="player"></param>
    public void InteractOnEnter(PlayerMovement player)
    {
        if(!WithoutInteract)
        {
            return;
        }

        Interact(player);
    }

    /// <summary>
    /// 玩家與此物件互動時觸發
    /// </summary>
    public void Interact(PlayerMovement player)
    {
        if (CloseIfSuccess && isInteracted)
        {
            return;
        }
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

                    if ((player.myCurrentAccessory != condition.accessoriesType != condition.isAntiLogic))
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
            ProcessInteractiveResult(player, ref interactiveFailResults);
            return;
        }

        ProcessInteractiveResult(player,ref interactiveSuccessResults);

        if (CloseIfSuccess)
        {
            gameObject.SetActive(false);
            isInteracted = true;
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

    private void ProcessInteractiveResult(PlayerMovement player ,ref InteractiveResult[] results)
    {
        foreach (var result in results)
        {
            switch (result.resultType)
            {
                case InteractiveResultType.ShowMessage:
                    GameManager.Instance.ShowTextBubble(showMessageAnchor, result.message);
                    break;
                case InteractiveResultType.WearMask:
                    player.ChangeMask(result.getObject.maskClass);
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
                case InteractiveResultType.ChangeLevel:
                    GameManager.Instance.ChangeLevel(result.changeLevelInfo.levelRoot, result.changeLevelInfo.playerStartPosition.position);
                    break;
                case InteractiveResultType.PlaySoundEffect:
                    AudioManager.Instance.PlaySFX(result.soundEffectType);
                    break;
                case InteractiveResultType.WinGame:
                    SceneManager.LoadScene(6);
                    break;
            }
        }
    }
}
