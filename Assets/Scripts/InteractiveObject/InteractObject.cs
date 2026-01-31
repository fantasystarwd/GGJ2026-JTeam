using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 互動演出物件
/// </summary>
public class InteractObject : MonoBehaviour
{
    /// <summary>
    /// 開啟的圖片
    /// </summary>
    public Sprite OpenSprite;

    /// <summary>
    /// 關閉的圖片
    /// </summary>
    public Sprite CloseSprite;

    /// <summary>
    /// 圖片繪製管理
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// collider
    /// </summary>
    private BoxCollider boxCollider;

    /// <summary>
    /// 是否開啟
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// 是否開啟
    /// </summary>
    public bool IsOpen
    {
        get { return isOpen; }
    }

    /// <summary>
    /// 設定狀態
    /// </summary>
    /// <param name="open"></param>
    public void SetState(bool open)
    {
        isOpen = open;
        boxCollider.enabled = !open;
        if (isOpen)
        {
            spriteRenderer.sprite = OpenSprite;
        }
        else
        {
            spriteRenderer.sprite = CloseSprite;
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        SetState(false);
    }
}
