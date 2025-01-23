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
        if (UIManager.Instance.UIList[2] != null && !GameManager.Instance.is_running) UIManager.Instance.UIList[2].gameObject.GetComponent<mainmenuUI>().FBS();
        if (UIManager.Instance.UIList[1] != null && GameManager.Instance.is_running) UIManager.Instance.UIList[1].gameObject.GetComponent<submenuUI>().FBS();
        if (UIManager.Instance.UIList[0] != null) UIManager.Instance.UIList[0].gameObject.SetActive(false);
    }
}
