using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class mainmenuUI : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button firstbutton;

    void Awake()
    {
        //this.enabled = true;
        firstbutton = GameObject.Find("NewButton").GetComponent<Button>();
        eventSystem = EventSystem.current;
        FBS();
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
        FBS();
        audiomanager.Instance.mainmenubgm.Play();
        audiomanager.Instance.mainmenubgm.loop = true;
    }

    public void OnClickLevelSelectButton()
    {
        audiomanager.Instance.menusfx.Play();
        if (UIManager.Instance.UIList[0] != null) UIManager.Instance.UIList[0].gameObject.SetActive(true);
    }

    public void OnClickQuitButton()
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
        eventSystem.SetSelectedGameObject(firstbutton.gameObject);
    }

}
