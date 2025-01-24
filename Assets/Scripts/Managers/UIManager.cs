using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static UIManager Instance;
    
    [SerializeField]private GameObject canvas;
    public Slider bossSlider;
    
    public List<RectTransform> UIList;
    
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
        
        UIList = new List<RectTransform>();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드되면 로그 띄움
        Debug.Log($"Scene Loaded(UI): {scene.name}");

        InitializeUI(scene);
    }

    private void InitializeUI(Scene scene)
    {
        // 씬이 초기화되면 로그 띄움
        Debug.Log($"Initializing scene(UI): {scene.name}");
        
        StartCoroutine(FindCanvasAfterDelay());
    }
    
    private IEnumerator FindCanvasAfterDelay()
    {
        yield return null; // 한 프레임 대기
        UIList.Clear();
        canvas = FindObjectOfType<Canvas>().gameObject;
        if (canvas != null)
        {
            Debug.Log($"Canvas Found: {canvas.name}");
        }
        else
        {
            Debug.Log("Canvas Not Found.");
        }
        
        // MainMenu Scene
        if (!GameManager.Instance.is_running)
        {
            InitMainMenuUI();
        }
        else
        {
            // Ingame Scene
            if (GameManager.Instance.is_ingame)
            {
                InitIngameUI();
            }
            
            // Boss Scene
            if (GameManager.Instance.is_boss)
            {
                InitBossUI();
            }
        }
    }

    private void InitMainMenuUI()
    {
        FindUI("SettingsUI");
        FindUI("LevelUI");
        FindUI("MainMenuUI");
        if (UIList[2] != null) GameManager.Instance.mm = UIList[2].GetComponent<mainmenuUI>();
    }

    private void InitIngameUI()
    {
        // ui_list에 필요한 UI들 가져오기
        FindUI("SettingsUI");
        FindUI("PauseMenuUI");
        FindUI("InGameUI");
        FindUI("MiniGameUI");
        if (UIList[3] != null) GameManager.Instance.mgui = UIList[3].GetComponent<minigameUI>();
        FindUI("GRayout5X5");
        FindUI("GRayout6X6");
        FindUI("GRayout7X7");
        FindUI("ChatUI");
        FindUI("OverUI");
        FindUI("PopupUI");
        if (UIList[9] != null) GameManager.Instance.pu = UIList[9].GetComponent<popupUI>();
        if (GameManager.Instance.pu != null) GameManager.Instance.pu.GetText();
    }

    private void InitBossUI()
    {
        FindUI("SettingsUI");
        FindUI("PauseMenuUI");
        FindUI("Slider");
        if (UIList[2] != null) bossSlider = UIList[2].GetComponent<Slider>();
        FindUI("ChatUI");
        FindUI("OverUI");
        FindUI("EndingUI");
    }
    
    public void UpdateHealthSlider()
    {
        if (bossSlider != null)
        {
            bossSlider.value = GameManager.Instance.GetNormalizedHealth();
        }
    }

    private void FindUI(string UIName)
    {
        Transform target = GameManager.Instance.FindChildByName(canvas.transform, UIName);

        if (target != null)
        {
            Debug.Log("찾은 오브젝트: " + target.name);
            UIList.Add(target.GetComponent<RectTransform>());
        }
        else
        {
            Debug.Log("오브젝트를 찾을 수 없습니다.");
        }
    }
}
