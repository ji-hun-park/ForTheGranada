using UnityEngine;
using UnityEngine.UI;

public class chatUI : MonoBehaviour
{
    public string[] chats;
    public int idx = 0;
    public Text txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        txt = GameObject.Find("chat").GetComponent<Text>();
        chats = new string[10];
        chats[0] = "게임 설명입니다!\r\n\r\n이동은 방향키 또는 WASD로 조작하며 공격은 불가합니다!\r\n모든 상호 작용은 F키를 통해 이루어집니다.\r\n제한 시간 안에 적을 피해 필요한 열쇠 조각을 모두 모아 여주인공이 떨어뜨린 소지품을 모아야 합니다!";
        chats[1] = "팔각형 회색 문 앞에서 F를 눌러 맵 이동이 가능합니다.\r\n상자와 상호 작용 시 미니게임을 진행하며\r\n정답을 맞출 시 랜덤 아이템 또는 열쇠 조각을 얻을 수 있습니다.";
        chats[2] = "미니게임에서 오답을 선택 시 패널티로\r\n5초 간 상자 이용이 금지되면서 넉백됩니다.\r\n상자가 회색이면 상호작용 불가능합니다.";
        chats[3] = "미니맵은 M키, 메뉴창은 ESC키를 통해 키고 끌 수 있습니다.\r\n미니맵에서 빨간 색이 현재 맵, 파란 색이 소지품 맵입니다.";
        chats[4] = "화면 상단 좌측은 HP와 쉴드,\r\n상단 중앙은 남은 시간,\r\n상단 우측은 미니맵\r\n하단 좌측은 아이템 목록\r\n하단 우측은 열쇠 조각 수, 스테이지 번호입니다.";
        chats[5] = "적의 시야는 부채꼴 모양이며,\r\n감지될 시 공격을 가합니다.\r\n지형지물 뒤에 숨으면 공격을 중단합니다.\r\n맵 진입 시 0.5초 간 무적 상태가 됩니다.";
        chats[6] = "필요한 열쇠 조각을 다 모으면 큰 상자에서 소지품이 보이며\r\n큰 상자 근처에서 F키를 눌러 다음 스테이지로 진입이 가능해집니다.\r\n스테이지는 일반 스테이지 3개와 보스 스테이지로 이루어져 있습니다.\r\n보스는 장애물과 충돌시켜 데미지를 입힐 수 있습니다!";
        chats[7] = "일반 체력 회복은 최대 체력을 넘길 수 없고\r\n신발은 하나당 이속이 10% 증가하며 최대 5개까지,\r\n쉴드 및 각종 아이템은 최대 한 개만 소지 가능합니다.";
        chats[8] = "최대 체력은 최대 2개 증가하며\r\n상자 투시는 아이템이 미리 보이고\r\n부활은 체력 1로 부활,\r\n회피는 적에게 피격 시 3초간 이속 30% 증가,\r\n감지 가속은 적에게 발각될 시 1초간 이속이 20%(회피 있으면 30%) 증가합니다";
        chats[9] = "매 스테이지 시작 시 스테이지와 아이템이 자동 저장됩니다.\r\n저장된 정보는 이어하기를 통해 불러올 수 있습니다.";
    }

    // Update is called once per frame
    void Update()
    {
        if (chats.Length > idx)
        {
            txt.text = chats[idx];
        }
        else
        {
            idx = 0;
            if ((GameManager.Instance.ui_list[2] != null && GameManager.Instance.ui_list[2].gameObject.activeSelf == false) || !GameManager.Instance.is_running) Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (chats.Length > idx) idx++;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (chats.Length > idx) idx++;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (chats.Length > idx) idx++;
        }
    }

    public void OnClickNextPageButton()
    {
        if (chats.Length > idx) idx++;
    }
}
