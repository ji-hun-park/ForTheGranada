using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static GameManager Instance;

    [Header("Player Settings")]
    public int health = 1;
    public float speed;
    public const float OriginSpeed = 4f;
    public float tmpSpeed;
    public int maxHealth = 10;
    public int armor;
    public float stealthTime;
    public int key;
    public int req_key;
    public int health_item ;
    public int armor_item;
    public int stealth_item;
    public int key_item;
    public int speed_item;
    public int haste_item;
    public int preview_item;
    public int resurrection_item;
    public KeyCode interactKey = KeyCode.F;

    [Header("Game Settings")]
    public Dictionary<string, KeyCode> keyBindings;
    public Dictionary<string, Text> keyDisplayTexts;
    
    [SerializeField] private float _boss_health;
    public float boss_health
    {
        get => _boss_health;
        set
        {
            _boss_health = value;
        }
    }
    private float _boss_max_health = 100f;
    public float boss_max_health
    {
        get { return _boss_max_health; }
    }
    
    public int diff;
    public int stage;
    public const int MaxStage = 3;

    [Header("Flags")]
    public bool is_resurrection;
    public bool is_attacked_speed;
    public bool is_stealth;
    public bool is_detected;
    public bool is_preview;
    public bool is_running;
    public bool is_closebox;
    public bool is_minigame;
    public bool is_delay;
    public bool is_CoroutineRunning;
    public bool is_mgset;
    public bool is_catch;
    public bool is_rannum;
    public bool is_rannum2;
    [SerializeField]
    private bool _is_ingame;
    [SerializeField]
    private bool _is_boss;

    [Header("GetComponents")]
    private GameObject tmp;
    public itemboxcontroller currentbox;
    public popupUI pu;
    public mainmenuUI mm;
    public minigameUI mgui;
    public scanner sc;
    public playercontroller pc;
    public bosscontroller boscon;
    public TMP_Text hint_count;
    public TMP_Text stagetext;
    public Transform player;
    
    [Header("Lists")]
    public int[] rannum3;
    public int[] rannum3_2;
    public Sprite[] spr_list;
    public string[] ans_list;

    public bool is_ingame
    {
        get => _is_ingame;
        set
        {
            _is_ingame = value;
            Debug.Log($"is_ingame 값 변경됨: {_is_ingame}");
        }
    }
    public bool is_boss
    {
        get => _is_boss;
        set
        {
            _is_boss = value;
            Debug.Log($"is_boss 값 변경됨: {_is_boss}");
        }
    }

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

        interactKey = KeyCode.F;
        stealthTime = 0f;
        is_attacked_speed = false;
        is_preview = false;
        
        if (keyBindings == null) keyBindings = new Dictionary<string, KeyCode>();
        if (keyDisplayTexts == null) keyDisplayTexts = new Dictionary<string, Text>();
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
        Debug.Log($"Scene Loaded: {scene.name}");

        InitializeScene(scene);
    }

    private void InitializeScene(Scene scene)
    {
        // 씬이 초기화되면 로그 띄움
        Debug.Log($"Initializing scene: {scene.name}");
        
        // MainMenu Scene
        if (!is_running)
        {
            audiomanager.Instance.bossdash.Stop();
        }

        // Ingame 들어가면 초기화 작업 실행
        if (is_ingame == true)
        {
            audiomanager.Instance.bossdash.Stop();
            InitIngame();
        }
        // 보스전이면
        if (is_boss)
        {
            audiomanager.Instance.bossdash.Stop();
            InitBoss();
        }
    }

    private void InitIngame()
    {
        // 필요한 컴포넌트들 가져오기
        FetchForIngame();
        
        is_rannum = true;
        is_rannum2 = true;
        is_mgset = false;
        StartCoroutine(ResetCoroutine());

        // 난이도 선택에 따라 게임 설정들 변경
        SwitchingSettingsOnDiff();

        // 시간 정상화, 미니게임 OFF
        Time.timeScale = 1;
        is_minigame = false;

        // 미니게임용 이미지랑 보기 리스트 가져오기
        if (spr_list.Length == 0) spr_list = MinigameManager.Instance.ImageSet();
        if (ans_list.Length == 0) ans_list = MinigameManager.Instance.AnswerSet();
        
        UIManager.Instance.SetItemIcon();
    }

    private void InitBoss()
    {
        tmp = GameObject.FindGameObjectWithTag("Player");
        if (tmp != null)
        {
            player = tmp.GetComponent<Transform>();
            pc = tmp.GetComponent<playercontroller>();
        }
        tmp = GameObject.FindGameObjectWithTag("Boss");
        if (tmp != null) boscon = tmp.GetComponent<bosscontroller>();

        // 블럭 수가 난이도가 이지면 18개, 노말이면 12개, 도전이면 6개
        tmp = GameObject.Find("Block1_03");
        if (tmp != null && diff == 3) tmp.SetActive(false);
        tmp = GameObject.Find("Block1_04");
        if (tmp != null && diff == 3) tmp.SetActive(false);
        tmp = GameObject.Find("Block2_03");
        if (tmp != null && diff == 3) tmp.SetActive(false);
        tmp = GameObject.Find("Block2_04");
        if (tmp != null && diff == 3) tmp.SetActive(false);
        tmp = GameObject.Find("Block3_03");
        if (tmp != null && diff == 3) tmp.SetActive(false);
        tmp = GameObject.Find("Block3_04");
        if (tmp != null && diff == 3) tmp.SetActive(false);
        tmp = GameObject.Find("Block1_05");
        if (tmp != null && (diff == 3 || diff == 2)) tmp.SetActive(false);
        tmp = GameObject.Find("Block1_06");
        if (tmp != null && (diff == 3 || diff == 2)) tmp.SetActive(false);
        tmp = GameObject.Find("Block2_05");
        if (tmp != null && (diff == 3 || diff == 2)) tmp.SetActive(false);
        tmp = GameObject.Find("Block2_06");
        if (tmp != null && (diff == 3 || diff == 2)) tmp.SetActive(false);
        tmp = GameObject.Find("Block3_05");
        if (tmp != null && (diff == 3 || diff == 2)) tmp.SetActive(false);
        tmp = GameObject.Find("Block3_06");
        if (tmp != null && (diff == 3 || diff == 2)) tmp.SetActive(false);
        
        armor = 0;
        armor_item = 0;
        health_item = 0;
        
        switch (diff)
        {
            case 1:
                health = 5;
                maxHealth = 5;
                break;
            case 2:
                health = 3;
                maxHealth = 3;
                break;
            case 3:
                health = 1;
                maxHealth = 1;
                break;
            default:
                Debug.LogError("Out of Diff!");
                break;
        }
    }

    private void SwitchingSettingsOnDiff()
    {
        switch (diff)
            {
                case 1:
                    health = 5;
                    if (stage == 1) maxHealth = 5;
                    if (stage == 1) health_item = 0;
                    if (stage != 1) health = maxHealth;
                    switch (stage)
                    {
                        case 1:
                            req_key = 3;
                            break;
                        case 2:
                            req_key = 5;
                            break;
                        case 3:
                            req_key = 7;
                            break;
                        default:
                            Debug.LogError("Out of StageNum!");
                            break;
                    }
                    break;
                case 2:
                    health = 3;
                    if (stage == 1) maxHealth = 3;
                    if (stage == 1) health_item = 0;
                    if (stage != 1) health = maxHealth;
                    switch (stage)
                    {
                        case 1:
                            req_key = 3;
                            break;
                        case 2:
                            req_key = 5;
                            break;
                        case 3:
                            req_key = 7;
                            break;
                        default:
                            Debug.LogError("Out of StageNum!");
                            break;
                    }
                    break;
                case 3:
                    health = 1;
                    if (stage == 1) maxHealth = 1;
                    if (stage == 1) health_item = 0;
                    if (stage != 1) health = maxHealth;
                    switch (stage)
                    {
                        case 1:
                            req_key = 5;
                            break;
                        case 2:
                            req_key = 7;
                            break;
                        case 3:
                            req_key = 9;
                            break;
                        default:
                            Debug.LogError("Out of StageNum!");
                            break;
                    }
                    break;
                default:
                    Debug.LogError("Out of Diff!");
                    break;
            }
    }

    private void FetchForIngame()
    {
        tmp = GameObject.Find("Scanner");
        if (tmp != null) sc = tmp.GetComponent<scanner>();
        tmp = GameObject.Find("hintcount");
        if (tmp != null) hint_count = tmp.GetComponent<TMP_Text>();
        tmp = GameObject.Find("stagetext");
        if (tmp != null) stagetext = tmp.GetComponent<TMP_Text>();
        tmp = GameObject.FindGameObjectWithTag("Player");
        if (tmp != null)
        {
            player = tmp.GetComponent<Transform>();
            pc = tmp.GetComponent<playercontroller>();
        }
    }
    
    void Update()
    {
        if (health == 0 && (is_boss || is_ingame))
        {
            GameOver();
        }

        if (is_ingame)
        {
            if (is_rannum)
            {
                rannum3 = MinigameManager.Instance.RanNumGen();
                is_rannum = false;
            }

            if (is_rannum2)
            {
                rannum3_2 = MinigameManager.Instance.RanNumGen();
                is_rannum2 = false;
                if (mgui != null) mgui.UpdateMinigame();
            }

            if (is_catch && ans_list != null && ans_list.Length != 0)
            {
                foreach (var rannum in rannum3_2)
                {
                    if (APIManager.Instance.APIResponse == ans_list[rannum]) is_rannum2 = true;
                }
            }

            if (is_mgset == false)
            {
                is_mgset = true;
                Debug.Log("요청 전송");
                APIManager.Instance.RequestStart();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ESC 메뉴 여닫기
                ESCMenu();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                // 레벨 및 난이도 별 표시할 맵
                UIManager.Instance.MapSetOnDiff();
                audiomanager.Instance.menusfx.Play();
            }

            if (is_closebox && is_delay == false && is_mgset && is_catch && !currentbox.isOpen && currentbox.ii.is_set)
            {
                if (Input.GetKeyDown(interactKey))
                {
                    audiomanager.Instance.menusfx.Play();
                    UIManager.Instance.UIList[3].gameObject.SetActive(true);
                }
            }

            if (is_delay && is_CoroutineRunning == false)
            {
                MinigameManager.Instance.StartSelInc();
                ItemManager.Instance.StartWaitFive();
            }
            
            UIManager.Instance.UpdateHealth();
            UIManager.Instance.UpdateShoe();
            if (hint_count != null) hint_count.text = key + " / " + req_key;
            if (stagetext != null) stagetext.text = "S" + stage;
        }

        if (is_boss)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ESC 메뉴 여닫기
                ESCMenu();
            }
            
            UIManager.Instance.UpdateHealth();
            if (boss_health <= 0) StartCoroutine(EndingCoroutine());
        }
        
        if (Input.GetKey(KeyCode.B)) // 보스맵 테스트용
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                TestBoss(1);
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                TestBoss(2);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                TestBoss(3);
            }
        }
    }

    private void TestBoss(int selectDiff)
    {
        if (is_ingame)
        {
            is_ingame = false;
        }
        
        if (!is_boss)
        {
            is_boss = true;
        }
        
        SelDiff(selectDiff); // 난이도에 따른 체력과 난도변수 조정
        
        key = 0;
        key_item = 0;
        stage = 4;
        Time.timeScale = 1;
        is_running = true;
        speed = OriginSpeed;
        audiomanager.Instance.mainmenubgm.Stop();
        SceneManager.LoadScene("Stage_Boss");
    }

    private void SelDiff(int df)
    {
        switch (df)
        {
            case 1:
                health = 5;
                diff = 1;
                break;
            case 2:
                health = 3;
                diff = 2;
                break;
            case 3:
                health = 1;
                diff = 3;
                break;
            default:
                Debug.LogError("Out of Diff!");
                break;
        }
    }

    private void ESCMenu()
    {
        if (UIManager.Instance.UIList[1] != null)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                if (!UIManager.Instance.UIList[0].gameObject.activeSelf) Time.timeScale = 1;
            }
            audiomanager.Instance.menusfx.Play();
            UIManager.Instance.UIList[1].gameObject.SetActive(!UIManager.Instance.UIList[1].gameObject.activeSelf);
        }
    }

    public void GameOver()
    {
        if (!is_running) return;
        
        if (is_resurrection)
        {
            // 부활템 사용
            ItemManager.Instance.UseReserrectionItem();
        }
        else
        {
            UIManager.Instance.UpdateHealth();
            audiomanager.Instance.ingamebgm.Stop();
            audiomanager.Instance.bossstagebgm.Stop();
            audiomanager.Instance.bossdash.Stop();
            audiomanager.Instance.gameover.Play();
            if (pc != null) pc.Dead();
            is_running = false;
            Debug.Log("캐릭터 사망!");
            // 게임 오버 UI 띄우기
            if (is_ingame && UIManager.Instance.UIList[8] != null) // Normal
            {
                UIManager.Instance.UIList[8].gameObject.SetActive(true);
            }
            else if (is_boss && UIManager.Instance.UIList[4] != null) // Boss
            {
                UIManager.Instance.UIList[4].gameObject.SetActive(true);
            }
            speed = 0;
            StartCoroutine(WaitThreeSecond());
        }
    }

    public IEnumerator WaitThreeSecond()
    {
        // 2초 기다리고 오버 UI 제거 후 타이틀 화면으로 감
        yield return new WaitForSeconds(2f);
        if (is_ingame)
        {
            UIManager.Instance.UIList[8].gameObject.SetActive(false);
            is_ingame = false;
        }
        if (is_boss)
        {
            UIManager.Instance.UIList[4].gameObject.SetActive(false);
            is_boss = false;
        }
        SceneManager.LoadScene("MainMenuScene");
    }

    public IEnumerator WaitThreeSecond2()
    {
        // 2초 기다리기
        yield return new WaitForSeconds(2f);
        // 팝업 UI 제거
        UIManager.Instance.UIList[9].gameObject.SetActive(false);
    }

    public IEnumerator EndingCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        UIManager.Instance.UIList[5].gameObject.SetActive(true);
        boss_health = 1;
        Debug.Log("Ending!");
    }

    public IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        is_detected = false;
    }

    public IEnumerator ASCoroutine()
    {
        tmpSpeed = speed;
        speed *= 1.3f;
        yield return new WaitForSeconds(3f);
        speed = tmpSpeed;
        pc.ASCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }

    public IEnumerator STCoroutine()
    {
        tmpSpeed = speed;
        speed *= is_attacked_speed ? 1.3f : 1.2f;
        yield return new WaitForSeconds(1f);
        speed = tmpSpeed;
        pc.STCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }
    
    public Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }

            // 재귀적으로 자식 검색
            Transform result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null; // 찾지 못했을 경우 null 반환
    }
}
