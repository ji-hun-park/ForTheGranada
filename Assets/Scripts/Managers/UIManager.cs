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
    public Image speedCount;
    
    public List<RectTransform> UIList;
    public List<RectTransform> healthList;
    public List<RectTransform> healthLoseList;
    public List<RectTransform> itemList;
    
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
        healthList = new List<RectTransform>();
        healthLoseList = new List<RectTransform>();
        itemList = new List<RectTransform>();
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
        
        ClearList(); // 리스트들 초기화
        
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

    private void ClearList()
    {
        UIList.Clear();
        healthList.Clear();
        healthLoseList.Clear();
        itemList.Clear();
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
        
        // 체력과 아이템 UI 자식들 가져오기
        FindUI("HPUI");
        healthList.AddRange(UIList[10].GetComponentsInChildren<RectTransform>());
        FindUI("LOSEHPUI");
        healthLoseList.AddRange(UIList[11].GetComponentsInChildren<RectTransform>());
        FindUI("ITEMUI");
        itemList.AddRange(UIList[12].GetComponentsInChildren<RectTransform>());
        //스피드 카운트 렌더러
        if (itemList != null) speedCount = itemList[3].GetComponent<Image>();
        
        // Find로 찾았으니 UI List들 다시 비활성화
        if (healthList != null) healthList[6].gameObject.SetActive(false);
        if (healthList != null) healthList[7].gameObject.SetActive(false);
        if (healthList != null) healthList[8].gameObject.SetActive(false);
        if (healthLoseList != null)
        {
            for (int i = GameManager.Instance.maxHealth + 1; i < healthLoseList.Count; i++)
            {
                healthLoseList[i].gameObject.SetActive(false);
            }
        }
        if (itemList != null) itemList[4].gameObject.SetActive(false);
        if (itemList != null) itemList[5].gameObject.SetActive(false);
        if (itemList != null) itemList[6].gameObject.SetActive(false);
        if (itemList != null) itemList[7].gameObject.SetActive(false);
        
        // 아이템 UI들 업데이트
        UpdateShoe();
        UpdateItemUI();
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
        
        // 체력과 아이템 UI 자식들 가져오기
        FindUI("HPUI");
        healthList.AddRange(UIList[6].GetComponentsInChildren<RectTransform>());
        FindUI("LOSEHPUI");
        healthLoseList.AddRange(UIList[7].GetComponentsInChildren<RectTransform>());
        FindUI("ITEMUI");
        itemList.AddRange(UIList[8].GetComponentsInChildren<RectTransform>());
        //스피드 카운트 렌더러
        if (itemList != null) speedCount = itemList[3].GetComponent<Image>();
        
        if (healthLoseList != null)
        {
            for (int i = GameManager.Instance.maxHealth + 1; i < healthLoseList.Count; i++)
            {
                healthLoseList[i].gameObject.SetActive(false);
            }
        }
        if (healthList != null && healthList.Count != 0 && GameManager.Instance.armor == 0) healthList[8].gameObject.SetActive(false);
        
        // 아이템 UI들 업데이트
        UpdateShoe();
        UpdateItemUI();
    }
    
    public void UpdateHealthSlider()
    {
        if (bossSlider != null)
        {
            bossSlider.value = GameManager.Instance.boss_health / GameManager.Instance.boss_max_health;;
        }
    }
    
    public void UpdateShoe()
    {
        if (itemList == null || itemList.Count == 0) return;
        
        string spriteName = "Speed";
        spriteName += GameManager.Instance.speed_item;
        speedCount.sprite = Resources.Load<Sprite>(spriteName);
    }
    
    public void UpdateItemUI()
    {
        if (healthList == null || healthList.Count == 0 || itemList == null || itemList.Count == 0) return;
        if (GameManager.Instance.armor_item >= 1 && healthList[8] != null) healthList[8].gameObject.SetActive(true); else healthList[8].gameObject.SetActive(false);
        if (GameManager.Instance.is_resurrection && itemList[4] != null) itemList[4].gameObject.SetActive(true); else itemList[4].gameObject.SetActive(false);
        if (GameManager.Instance.is_attacked_speed && itemList[5] != null) itemList[5].gameObject.SetActive(true); else itemList[5].gameObject.SetActive(false);
        if (GameManager.Instance.is_stealth && itemList[6] != null) itemList[6].gameObject.SetActive(true); else itemList[6].gameObject.SetActive(false);
        if (GameManager.Instance.is_preview && itemList[7] != null) itemList[7].gameObject.SetActive(true); else itemList[7].gameObject.SetActive(false);
    }

    public void UpdateHealth()
    {
        if (healthList == null || healthList.Count == 0) return;
        
        switch (GameManager.Instance.health)
        {
            case 0:
                healthList[1].gameObject.SetActive(false);
                healthList[2].gameObject.SetActive(false);
                healthList[3].gameObject.SetActive(false);
                healthList[4].gameObject.SetActive(false);
                healthList[5].gameObject.SetActive(false);
                healthList[6].gameObject.SetActive(false);
                healthList[7].gameObject.SetActive(false);
                break;
            case 1:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(false);
                healthList[3].gameObject.SetActive(false);
                healthList[4].gameObject.SetActive(false);
                healthList[5].gameObject.SetActive(false);
                healthList[6].gameObject.SetActive(false);
                healthList[7].gameObject.SetActive(false);
                break;
            case 2:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(true);
                healthList[3].gameObject.SetActive(false);
                healthList[4].gameObject.SetActive(false);
                healthList[5].gameObject.SetActive(false);
                healthList[6].gameObject.SetActive(false);
                healthList[7].gameObject.SetActive(false);
                break;
            case 3:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(true);
                healthList[3].gameObject.SetActive(true);
                healthList[4].gameObject.SetActive(false);
                healthList[5].gameObject.SetActive(false);
                healthList[6].gameObject.SetActive(false);
                healthList[7].gameObject.SetActive(false);
                break;
            case 4:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(true);
                healthList[3].gameObject.SetActive(true);
                healthList[4].gameObject.SetActive(true);
                healthList[5].gameObject.SetActive(false);
                healthList[6].gameObject.SetActive(false);
                healthList[7].gameObject.SetActive(false);
                break;
            case 5:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(true);
                healthList[3].gameObject.SetActive(true);
                healthList[4].gameObject.SetActive(true);
                healthList[5].gameObject.SetActive(true);
                healthList[6].gameObject.SetActive(false);
                healthList[7].gameObject.SetActive(false);
                break;
            case 6:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(true);
                healthList[3].gameObject.SetActive(true);
                healthList[4].gameObject.SetActive(true);
                healthList[5].gameObject.SetActive(true);
                healthList[6].gameObject.SetActive(true);
                healthList[7].gameObject.SetActive(false);
                break;
            case 7:
                healthList[1].gameObject.SetActive(true);
                healthList[2].gameObject.SetActive(true);
                healthList[3].gameObject.SetActive(true);
                healthList[4].gameObject.SetActive(true);
                healthList[5].gameObject.SetActive(true);
                healthList[6].gameObject.SetActive(true);
                healthList[7].gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Out of Health!");
                break;
        }
    }
    
    public void SetItemIcon()
    {
        if (healthList != null && healthList.Count != 0)
        {
            if (GameManager.Instance.armor_item >= 1) healthList[8].gameObject.SetActive(true);
        }
        if (itemList != null && itemList.Count != 0)
        {
            if (GameManager.Instance.resurrection_item >= 1) itemList[4].gameObject.SetActive(true);
            if (GameManager.Instance.is_attacked_speed) itemList[5].gameObject.SetActive(true);
            if (GameManager.Instance.stealth_item >= 1) itemList[6].gameObject.SetActive(true);
            if (GameManager.Instance.preview_item >= 1) itemList[7].gameObject.SetActive(true);
        }
    }
    
    public void MapSetOnDiff()
    {
        switch (GameManager.Instance.diff)
            {
                case 1:
                    // 쉬움 난이도일 때 실행할 코드
                    if (UIManager.Instance.UIList[4] != null && GameManager.Instance.stage == 1)
                    {
                        UIManager.Instance.UIList[4].gameObject.SetActive(!UIManager.Instance.UIList[4].gameObject.activeSelf);
                    }
                    else if (UIManager.Instance.UIList[5] != null && GameManager.Instance.stage == 2)
                    {
                        UIManager.Instance.UIList[5].gameObject.SetActive(!UIManager.Instance.UIList[5].gameObject.activeSelf);
                    }
                    else if (UIManager.Instance.UIList[6] != null && GameManager.Instance.stage == 3)
                    {
                        UIManager.Instance.UIList[6].gameObject.SetActive(!UIManager.Instance.UIList[6].gameObject.activeSelf);
                    }
                    break;
                case 2:
                    // 보통 난이도일 때 실행할 코드
                    if (UIManager.Instance.UIList[4] != null && GameManager.Instance.stage == 1)
                    {
                        UIManager.Instance.UIList[4].gameObject.SetActive(!UIManager.Instance.UIList[4].gameObject.activeSelf);
                    }
                    else if (UIManager.Instance.UIList[5] != null && GameManager.Instance.stage == 2)
                    {
                        UIManager.Instance.UIList[5].gameObject.SetActive(!UIManager.Instance.UIList[5].gameObject.activeSelf);
                    }
                    else if (UIManager.Instance.UIList[6] != null && GameManager.Instance.stage == 3)
                    {
                        UIManager.Instance.UIList[6].gameObject.SetActive(!UIManager.Instance.UIList[6].gameObject.activeSelf);
                    }
                    break;
                case 3:
                    // 도전 난이도일 때 실행할 코드
                    if (UIManager.Instance.UIList[5] != null && GameManager.Instance.stage == 1)
                    {
                        UIManager.Instance.UIList[5].gameObject.SetActive(!UIManager.Instance.UIList[5].gameObject.activeSelf);
                    }
                    else if (UIManager.Instance.UIList[6] != null && GameManager.Instance.stage >= 2)
                    {
                        UIManager.Instance.UIList[6].gameObject.SetActive(!UIManager.Instance.UIList[6].gameObject.activeSelf);
                    }
                    break;
                default:
                    Debug.LogError("Out of Diff!");
                    break;
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
