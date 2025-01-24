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
        eventSystem = EventSystem.current;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        FBS();
    }

    public void OnClickReturnButton()
    {
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_running = false;
        GameManager.Instance.is_boss = false;
        Resume();
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.ingamebgm.Stop();
        audiomanager.Instance.bossstagebgm.Stop();
        audiomanager.Instance.mainmenubgm.Play();
        audiomanager.Instance.mainmenubgm.loop = true;
        SceneManager.LoadScene("MainMenuScene");
    }

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
        if (firstbutton != null && GameManager.Instance.is_running) eventSystem.SetSelectedGameObject(firstbutton.gameObject);
    }

}
