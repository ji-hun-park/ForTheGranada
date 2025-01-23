using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class minigameUI : MonoBehaviour
{
    public Image[] img_list;
    public Text[] txt_list;
    public Button[] btn_list;
    public int LLM;
    public EventSystem eventSystem;

    void Awake()
    {
        this.enabled = true;
        eventSystem = EventSystem.current;
        img_list = GetComponentsInChildren<Image>();
        txt_list = GetComponentsInChildren<Text>();
        btn_list = GetComponentsInChildren<Button>();
    }

    public void UpdateMinigame()
    {
        GameManager.Instance.is_minigame = true;
        if (GameManager.Instance.is_mgset == true && GameManager.Instance.APIResponse != null && img_list.Length != 0 && img_list != null && GameManager.Instance.rannum3 != null && GameManager.Instance.rannum3.Length != 0)
        {
            img_list[2].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum3[0]];
            img_list[3].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum3[1]];
            img_list[4].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum3[2]];
        }

        if (GameManager.Instance.is_mgset == true && GameManager.Instance.APIResponse != null && txt_list.Length != 0 && txt_list != null && btn_list != null && btn_list.Length != 0 && GameManager.Instance.rannum3_2 != null && GameManager.Instance.rannum3_2.Length != 0)
        {
            for (int k = 1; k < 5; k++)
            {
                txt_list[k].text = "NULL";
            }

            LLM = Random.Range(1, 5);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    if (txt_list[j].text != "NULL")
                    {
                        //Debug.Log("!NULL");
                    }
                    else if (j == LLM)
                    {
                        txt_list[j].text = GameManager.Instance.APIResponse.Length > 6 ? GameManager.Instance.APIResponse.Substring(0, 6).Trim() : GameManager.Instance.APIResponse.Trim();
                    }
                    else
                    {
                        txt_list[j].text = GameManager.Instance.ans_list[GameManager.Instance.rannum3_2[i]];
                        break;
                    }
                }
            }
            if (txt_list[4].text == "NULL" && LLM == 4) txt_list[4].text = GameManager.Instance.APIResponse.Length > 6 ? GameManager.Instance.APIResponse.Substring(0, 6).Trim() : GameManager.Instance.APIResponse.Trim();

            // ��ư �ʱ�ȭ
            btn_list[1].onClick.RemoveAllListeners();
            btn_list[2].onClick.RemoveAllListeners();
            btn_list[3].onClick.RemoveAllListeners();
            btn_list[4].onClick.RemoveAllListeners();

            for (int i = 1; i < 5; i++)
            {
                if (i == LLM)
                {
                    btn_list[i].onClick.AddListener(OnClickCorrectButton);
                }
                else
                {
                    btn_list[i].onClick.AddListener(OnClickIncorrectButton);
                }
            }
        }
    }

    private void OnEnable()
    {
        // ?��?�� 로드?�� ?�� ?���?
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //UpdateMinigame();
        eventSystem.SetSelectedGameObject(btn_list[1].gameObject);
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        // ?�� 로드 ?��벤트 ?��?��
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.gameObject.SetActive(true);
    }

    public void OnClickRandomButton()
    {
        btn_list[1].onClick.RemoveAllListeners();
        btn_list[2].onClick.RemoveAllListeners();
        btn_list[3].onClick.RemoveAllListeners();
        btn_list[4].onClick.RemoveAllListeners();

        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }
        GameManager.Instance.is_delay = true;
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

    public void OnClickCorrectButton()
    {
        btn_list[1].onClick.RemoveAllListeners();
        btn_list[2].onClick.RemoveAllListeners();
        btn_list[3].onClick.RemoveAllListeners();
        btn_list[4].onClick.RemoveAllListeners();

        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }

        if (GameManager.Instance.is_mgset)
        {
            GameManager.Instance.is_mgset = false;
        }

        audiomanager.Instance.menusfx.Play();
        GameManager.Instance.im.getItem(GameManager.Instance.currentbox.ii.item);
        GameManager.Instance.pu.item = GameManager.Instance.currentbox.ii.item;
        Time.timeScale = 1;
        GameManager.Instance.currentbox.isOpen = true;
        GameManager.Instance.is_running = true;
        GameManager.Instance.is_catch = false;
        GameManager.Instance.is_rannum = true;
        GameManager.Instance.is_rannum2 = true;
        GameManager.Instance.currentbox.ii.Alpha255();
        GameManager.Instance.currentbox.ii.isGet = true;
        GameManager.Instance.ui_list[8].gameObject.SetActive(true);
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

    public void OnClickIncorrectButton()
    {
        btn_list[1].onClick.RemoveAllListeners();
        btn_list[2].onClick.RemoveAllListeners();
        btn_list[3].onClick.RemoveAllListeners();
        btn_list[4].onClick.RemoveAllListeners();

        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }

        if (GameManager.Instance.is_mgset)
        {
            GameManager.Instance.is_mgset = false;
        }

        audiomanager.Instance.menusfx.Play();
        Time.timeScale = 1;
        GameManager.Instance.is_running = true;
        GameManager.Instance.is_catch = false;
        GameManager.Instance.is_rannum = true;
        GameManager.Instance.is_rannum2 = true;
        GameManager.Instance.is_delay = true;
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

}
