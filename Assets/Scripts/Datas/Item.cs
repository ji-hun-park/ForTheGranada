using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Expendables = 0, //소모품
    Passive = 1,
    Temporary = 2,
    Resurrection = 3,
    Key = 4,
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [Header("아이템의 ID")]
    [SerializeField] private int item_ID;

    public int GetItemID
    {
        get { return item_ID; }
    }

    [Header("아이템의 이미지")]
    [SerializeField] private Sprite item_sprite;

    public Sprite GetItemSprite
    {
        get { return item_sprite; }
    }

    [Header("아이템이 중첩가능한 갯수")]
    [SerializeField] private int num_nesting;

    public int GetNumNesting
    {
        get { return num_nesting; }
    }

    [Header("아이템의 타입")]
    [SerializeField] private ItemType item_type;

    public ItemType GetItemType
    {
        get { return item_type; }
    }
}
