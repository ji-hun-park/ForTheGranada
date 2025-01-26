using UnityEngine;

public enum ItemType
{
    Expendables = 0,
    Passive = 1,
    Temporary = 2,
    Resurrection = 3,
    Key = 4,
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [Header("ID")]
    [SerializeField] private int item_ID;

    public int GetItemID
    {
        get { return item_ID; }
    }

    [Header("Sprite")]
    [SerializeField] private Sprite item_sprite;

    public Sprite GetItemSprite
    {
        get { return item_sprite; }
    }

    [Header("NumNesting")]
    [SerializeField] private int num_nesting;

    public int GetNumNesting
    {
        get { return num_nesting; }
    }

    [Header("Type")]
    [SerializeField] private ItemType item_type;

    public ItemType GetItemType
    {
        get { return item_type; }
    }
}
