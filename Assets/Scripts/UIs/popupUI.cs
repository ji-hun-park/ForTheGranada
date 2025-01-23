using UnityEngine;
using UnityEngine.UI;

public class popupUI : MonoBehaviour
{
    public Text MyText;
    public string iteminfo;
    public string itemname;
    public Item item;
    public int itemnumber;

    void OnEnable()
    {
        SetItemText();
        StartCoroutine(GameManager.Instance.WaitThreeSecond2());
    }

    private void SetItemText()
    {
        if (item != null) itemnumber = item.GetItemID;
        
        switch (itemnumber)
        {
            case 1:
                itemname = "최대 체력 증가";
                break;
            case 2:
                itemname = "체력 회복";
                break;
            case 3:
                itemname = "보호막";
                break;
            case 4:
                itemname = "이속 증가";
                break;
            case 5:
                itemname = "부활";
                break;
            case 6:
                itemname = "회피";
                break;
            case 7:
                itemname = "감지 시 이속 증가";
                break;
            case 8:
                itemname = "상자 투시";
                break;
            case 9:
                itemname = "열쇠 조각";
                break;
            default:
                Debug.LogError("Out of Range Number");
                break;
        }

        iteminfo = itemname + " 아이템 획득!";
        if (item != null) MyText.text = iteminfo;
    }

    public void GetText()
    {
        MyText = GetComponentInChildren<Text>();
    }
}
