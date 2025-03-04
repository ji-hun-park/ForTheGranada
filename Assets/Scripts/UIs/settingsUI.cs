using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class settingsUI : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button firstbutton;
    void Awake()
    {
        eventSystem = EventSystem.current;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ESC();
    }

    private void OnEnable()
    {
        // 버튼을 첫 번째 선택된 오브젝트로 설정
        if (eventSystem != null) eventSystem.SetSelectedGameObject(firstbutton.gameObject);
    }

    private void OnDisable()
    {
        ESC();
    }

    public void ESC()
    {
        if (GameManager.Instance.is_running)
        {
            if (UIManager.Instance.UIList[1] == null)
            {
                return;
            }
            
            UIManager.Instance.UIList[1].gameObject.GetComponent<submenuUI>().FBS();
        }
        else // MainMenuScene
        {
            if (UIManager.Instance.UIList[2] == null)
            {
                return;
            }
            
            UIManager.Instance.UIList[2].gameObject.GetComponent<mainmenuUI>().FBS();
        }
    }
}
