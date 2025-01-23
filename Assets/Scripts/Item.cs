using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Expendables = 0, //�Ҹ�ǰ
    Passive = 1,
    Temporary = 2,
    Resurrection = 3,
    Key = 4,
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [Header("�������� ID")]
    [SerializeField] private int item_ID;

    public int GetItemID
    {
        get { return item_ID; }
    }

    [Header("�������� �̹���")]
    [SerializeField] private Sprite item_sprite;

    public Sprite GetItemSprite
    {
        get { return item_sprite; }
    }

    [Header("�������� ��ø������ ����")]
    [SerializeField] private int num_nesting;

    public int GetNumNesting
    {
        get { return num_nesting; }
    }

    [Header("�������� Ÿ��")]
    [SerializeField] private ItemType item_type;

    public ItemType GetItemType
    {
        get { return item_type; }
    }
}
