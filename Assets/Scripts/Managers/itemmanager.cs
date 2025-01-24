using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static ItemManager Instance;
    
    //[SerializeField]
    private Item item;
    public Item Item { get { return item; } set { item = value; } }
    public List<Item> itemList;
    public inneritem[] innerItems;
    
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
    
    public int SelectItem(int rannum1)
    {
        int itemNum = 10;

        if (rannum1 >= 1 && rannum1 <= 50)
        {
            itemNum = 1;
            //Debug.Log("체력 회복");
        }
        else if (rannum1 >= 51 && rannum1 <= 70)
        {
            itemNum = 2;
            //Debug.Log("쉴드 획득");
        }
        else if (rannum1 >= 71 && rannum1 <= 81)
        {
            itemNum = 3;
            //Debug.Log("이속 증가");
        }
        else if (rannum1 >= 82 && rannum1 <= 87)
        {
            itemNum = 0;
            //Debug.Log("최대 체력 증가");
        }
        else if (rannum1 >= 88 && rannum1 <= 92)
        {
            itemNum = 5;
            //Debug.Log("피격 시 이속 증가");
        }
        else if (rannum1 >= 93 && rannum1 <= 94)
        {
            itemNum = 6;
            //Debug.Log("감지 시 이속 증가");
        }
        else if (rannum1 >= 95 && rannum1 <= 99)
        {
            itemNum = 7;
            //Debug.Log("상자 투시");
        }
        else if (rannum1 == 100)
        {
            itemNum = 4;
            //Debug.Log("부활 템 획득!");
        }
        else
        {
            Debug.LogError("Out of itemNum");
        }

        return itemNum;
    }
    
    public void SetItems()
    {
        // 초기화 작업
        for (int k = 0; k < innerItems.Length; k++)
        {
            innerItems[k].is_set = false;
        }

        // 열쇠를 미리 세팅
        int[] rankey = MinigameManager.Instance.RanNumGenWithNum(GameManager.Instance.req_key, innerItems.Length);

        foreach (int i in rankey)
        {
            innerItems[i].itemNumber = 8;
            innerItems[i].is_set = true;
        }

        // 나머지 아이템 랜덤 세팅
        for (int j = 0; j < innerItems.Length; j++)
        {
            if (!innerItems[j].is_set)
            {
                innerItems[j].itemNumber = SelectItem(UnityEngine.Random.Range(1, 101));
                innerItems[j].is_set = true;
            }
        }
    }

    public void StartSetItmScr()
    {
        StartCoroutine(SetItemScripts());
    }
    
    public void StartWaitFive()
    {
        StartCoroutine(WaitFiveSeconds());
    }
    
    private IEnumerator SetItemScripts()
    {
        yield return new WaitForSeconds(1f);
        // InnerItem 스크립트를 가진 모든 오브젝트 찾기
        innerItems = FindObjectsOfType<inneritem>(true);
        // 모든 상자에 키랑 아이템 할당
        if (innerItems.Length >= GameManager.Instance.req_key) SetItems();
    }
    
    private IEnumerator WaitFiveSeconds()
    {
        // 5초 기다리고 응답없으면 프리셋 적용
        yield return new WaitForSeconds(5f);
        if (!GameManager.Instance.is_catch)
        {
            Debug.Log("응답 너무 느림");
            MinigameManager.Instance.FailRequest();
            GameManager.Instance.is_catch = true;
            if (GameManager.Instance.mgui != null) GameManager.Instance.mgui.UpdateMinigame();
        }
    }
}
