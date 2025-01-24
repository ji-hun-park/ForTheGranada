using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static ItemManager Instance;
    
    //[SerializeField]
    private Item item;
    public Item Item { get { return item; } set { item = value; } }
    public List<Item> itemList;
    
    void Awake()
    {
        // Instance 존재 유무에 따라 게임 매니저 파괴 여부 정함
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 기존에 존재 안하면 이걸로 대체하고 파괴하지 않기
        }
        else
        {
            Destroy(gameObject); // 기존에 존재하면 자신파괴
        }
        
        /*
        #if UNITY_EDITOR
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Health.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Heal.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Armor.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Speed.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Ressurection.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Under_damaged.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Detect.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Preview.asset");
                itemlist.Add(Item);
                Item = AssetDatabase.LoadAssetAtPath<Item>("Assets/Item/Key.asset");
                itemlist.Add(Item);
        #endif
        */
        
        Item = Resources.Load<Item>("Health");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Heal");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Armor");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Speed");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Ressurection");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Under_damaged");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Detect");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Preview");
        itemList.Add(Item);
        Item = Resources.Load<Item>("Key");
        itemList.Add(Item);
    }

    public void getItem(Item item)
    {
        audiomanager.Instance.getitem.Play();
        if (item.GetItemType == ItemType.Expendables)
        {
            if (item.GetItemID == 1)
            {
                if (GameManager.Instance.health_item < item.GetNumNesting)
                {
                    GameManager.Instance.maxHealth++;
                    GameManager.Instance.health++;
                    GameManager.Instance.health_item++;
                    GameManager.Instance.health_lose_list[GameManager.Instance.maxHealth].gameObject.SetActive(true);
                }
                else if (GameManager.Instance.maxHealth > GameManager.Instance.health)
                {
                    GameManager.Instance.health++;
                }
            }
            else if (item.GetItemID == 2)//ȸ�� ������
            {
                if (GameManager.Instance.maxHealth > GameManager.Instance.health)
                {
                    GameManager.Instance.health++;
                }
            }
        }
        else if (item.GetItemType == ItemType.Passive)
        {
            if (item.GetItemID == 4 && GameManager.Instance.speed_item < item.GetNumNesting)
            {
                GameManager.Instance.speed_item++;
                GameManager.Instance.speed = GameManager.OriginSpeed * (1f + (0.1f * GameManager.Instance.speed_item));
                GameManager.Instance.tmpSpeed = GameManager.Instance.speed;
            }
            else if (item.GetItemID == 6 && GameManager.Instance.haste_item < item.GetNumNesting)
            {
                GameManager.Instance.is_attacked_speed = true;
                GameManager.Instance.item_list[5].gameObject.SetActive(true);
            }
            else if (item.GetItemID == 7 && GameManager.Instance.stealth_item < item.GetNumNesting)
            {
                //GameManager.Instance.stealthTime += 1f;
                GameManager.Instance.is_stealth = true;
                GameManager.Instance.stealth_item++;
                GameManager.Instance.item_list[6].gameObject.SetActive(true);
            }
            else if (item.GetItemID == 8 && GameManager.Instance.preview_item < item.GetNumNesting)
            {
                GameManager.Instance.is_preview = true;
                GameManager.Instance.preview_item++;
                GameManager.Instance.item_list[7].gameObject.SetActive(true);
            }
        }
        else if (item.GetItemType == ItemType.Temporary)
        {
            if (item.GetItemID == 3 && GameManager.Instance.armor_item < item.GetNumNesting)
            {
                GameManager.Instance.armor++;
                GameManager.Instance.armor_item++;
                UIManager.Instance.healthList[8].gameObject.SetActive(true);
            }
        }
        else if (item.GetItemType == ItemType.Resurrection)
        {
            if (item.GetItemID == 5 && GameManager.Instance.resurrection_item < item.GetNumNesting)
            {
                GameManager.Instance.is_resurrection = true;
                GameManager.Instance.resurrection_item++;
                GameManager.Instance.item_list[4].gameObject.SetActive(true);
            }
        }
        else if (item.GetItemType == ItemType.Key)
        {
            if (item.GetItemID == 9 && GameManager.Instance.key < GameManager.Instance.req_key)
            {
                GameManager.Instance.key++;
                GameManager.Instance.key_item++;
            }
        }
    }
}
