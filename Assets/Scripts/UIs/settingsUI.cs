using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class settingsUI : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button firstbutton;
    void Awake()
    {
        //this.enabled = true;
        firstbutton = GameObject.Find("BakButton").GetComponent<Button>();
        eventSystem = EventSystem.current;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ESC();
    }

    private void OnEnable()
    {
        // ?��?�� 로드?�� ?�� ?���?
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 버튼을 첫 번째 선택된 오브젝트로 설정
        eventSystem.SetSelectedGameObject(firstbutton.gameObject);
    }

    private void OnDisable()
    {
        // ?�� 로드 ?��벤트 ?��?��
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ESC();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //GameManager.Instance.is_ingame = false;
    }

    public void ESC()
    {
        if (GameManager.Instance.ui_list[0] != null && !GameManager.Instance.is_running) GameManager.Instance.ui_list[0].gameObject.GetComponent<mainmenuUI>().FBS();
        if (GameManager.Instance.ui_list[2] != null && GameManager.Instance.is_running) GameManager.Instance.ui_list[2].gameObject.GetComponent<submenuUI>().FBS();
        GameManager.Instance.ui_list[10].gameObject.SetActive(false);
    }
}
