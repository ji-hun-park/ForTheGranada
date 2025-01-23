using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static UIManager Instance;
    
    [SerializeField]private GameObject canvas;
    
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
        
        canvas = GameObject.Find("Canvas");
        UIList.Clear();
        
        // MainMenu Scene
        if (!GameManager.Instance.is_running)
        {
            FindUI("SettingsUI");
            FindUI("LevelUI");
            FindUI("MainMenuUI");
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
