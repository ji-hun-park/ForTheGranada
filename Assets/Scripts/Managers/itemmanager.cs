using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public class itemmanager : MonoBehaviour
{
    //[SerializeField]
    private Item item;
    public Item Item { get { return item; } set { item = value; } }
    public List<Item> itemlist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
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
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Heal");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Armor");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Speed");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Ressurection");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Under_damaged");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Detect");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Preview");
        itemlist.Add(Item);
        Item = Resources.Load<Item>("Key");
        itemlist.Add(Item);
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public void getItem(Item item)
    {
        audiomanager.Instance.getitem.Play();
        if (item.GetItemType == ItemType.Expendables)
        {
            if (item.GetItemID == 1)//ü�� ������
            {
                if (GameManager.Instance.health_item < item.GetNumNesting)
                {
                    GameManager.Instance.maxHealth++;
                    GameManager.Instance.health++;
                    GameManager.Instance.health_item++;
                    GameManager.Instance.health_lose_list[GameManager.Instance.maxHealth].gameObject.SetActive(true);
                }
                else if (GameManager.Instance.maxHealth > GameManager.Instance.health)//�ִ� ���� �ʰ��� ȸ�� �����۰� ���� ȿ��
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
            if (item.GetItemID == 4 && GameManager.Instance.speed_item < item.GetNumNesting)//�ӵ� ������
            {
                GameManager.Instance.speed_item++;
                GameManager.Instance.speed = GameManager.OriginSpeed * (1f + (0.1f * GameManager.Instance.speed_item));
                GameManager.Instance.tmpSpeed = GameManager.Instance.speed;
            }
            else if (item.GetItemID == 6 && GameManager.Instance.haste_item < item.GetNumNesting)//�ǰ� ������
            {
                GameManager.Instance.is_attacked_speed = true;
                GameManager.Instance.item_list[5].gameObject.SetActive(true);
            }
            else if (item.GetItemID == 7 && GameManager.Instance.stealth_item < item.GetNumNesting)//���� ������
            {
                //GameManager.Instance.stealthTime += 1f;
                GameManager.Instance.is_stealth = true;
                GameManager.Instance.stealth_item++;
                GameManager.Instance.item_list[6].gameObject.SetActive(true);
            }
            else if (item.GetItemID == 8 && GameManager.Instance.preview_item < item.GetNumNesting)//���� ������
            {
                GameManager.Instance.is_preview = true;
                GameManager.Instance.preview_item++;
                GameManager.Instance.item_list[7].gameObject.SetActive(true);
            }
        }
        else if (item.GetItemType == ItemType.Temporary)
        {
            if (item.GetItemID == 3 && GameManager.Instance.armor_item < item.GetNumNesting)//���� ������
            {
                GameManager.Instance.armor++;
                GameManager.Instance.armor_item++;
                GameManager.Instance.health_list[8].gameObject.SetActive(true);
            }
        }
        else if (item.GetItemType == ItemType.Resurrection)
        {
            if (item.GetItemID == 5 && GameManager.Instance.ressurection_item < item.GetNumNesting)//��Ȱ ������
            {
                GameManager.Instance.is_ressurection = true;
                GameManager.Instance.ressurection_item++;
                GameManager.Instance.item_list[4].gameObject.SetActive(true);
            }
        }
        else if (item.GetItemType == ItemType.Key)
        {
            if (item.GetItemID == 9 && GameManager.Instance.key < GameManager.Instance.req_key)//���� ����
            {
                GameManager.Instance.key++;
                GameManager.Instance.key_item++;
            }
        }
    }
}
