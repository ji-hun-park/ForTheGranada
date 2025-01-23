using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class submenuUI : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button firstbutton;
    void Awake()
    {
        if (GameManager.Instance.is_running) firstbutton = GameObject.Find("RetButton").GetComponent<Button>();
        eventSystem = EventSystem.current;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        // ?��?�� 로드?�� ?�� ?���?
        SceneManager.sceneLoaded += OnSceneLoaded;

        FBS();
    }

    private void OnDisable()
    {
        // ?�� 로드 ?��벤트 ?��?��
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.gameObject.SetActive(true);
    }

    public void OnClickReturnButton()
    {
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_running = false;
        GameManager.Instance.is_boss = false;
        Time.timeScale = 1;
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.ingamebgm.Stop();
        audiomanager.Instance.bossstagebgm.Stop();
        audiomanager.Instance.mainmenubgm.Play();
        audiomanager.Instance.mainmenubgm.loop = true;
        SceneManager.LoadScene("MainMenuScene");
    }

    /*public void OnClickSaveButton()
    {
        Debug.Log("����Ǿ����ϴ�!");
    }*/

    public void OnClickInfoButton()
    {
        audiomanager.Instance.menusfx.Play();
        if (UIManager.Instance.UIList[7] != null)
        {
            UIManager.Instance.UIList[7].gameObject.SetActive(true);
        }
    }

    public void OnClickSettingButton()
    {
        audiomanager.Instance.menusfx.Play();
        if (UIManager.Instance.UIList[0] != null)
        {
            UIManager.Instance.UIList[0].gameObject.SetActive(true);
        }
    }

    public void OnClickCloseButton()
    {
        audiomanager.Instance.menusfx.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void FBS()
    {
        // 버튼을 첫 번째 선택된 오브젝트로 설정
        if (firstbutton != null) eventSystem.SetSelectedGameObject(firstbutton.gameObject);
    }

}
