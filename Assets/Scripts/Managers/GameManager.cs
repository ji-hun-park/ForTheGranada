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
    public float speed = 0f;
    public float originspeed = 0f;
    public float tmpspeed = 0f;
    public float speed_for_boss_stage = 0f;
    public int maxHealth = 10;
    public int armor = 0;
    public float stealthTime = 0f;
    public int key;
    public int req_key;
    public int health_item = 0;
    public int armor_item = 0;
    public int stealth_item = 0;
    public int key_item = 0;
    public int speed_item = 0;
    public int haste_item = 0;
    public int preview_item = 0;
    public int ressurection_item = 0;
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
    [SerializeField] private float _boss_max_health;
    public float boss_max_health
    {
        get => _boss_max_health;
        set
        {
            _boss_max_health = value;
        }
    }
    public int diff = 0;
    public int stage = 0;
    public int maxstage = 0;

    [Header("Flags")]
    public bool is_ressurection;
    public bool is_attacked_speed;
    public bool is_stealth;
    public bool is_detected;
    public bool is_preview;
    public bool is_running;
    public bool is_closebox = false;
    public bool is_minigame = false;
    public bool is_delay = false;
    public bool is_CoroutineRunning = false;
    public bool is_mgset = false;
    public bool is_catch = false;
    public bool is_rannum = false;
    public bool is_rannum2 = false;
    [SerializeField]
    private bool _is_ingame = false;
    [SerializeField]
    private bool _is_boss = false;

    [Header("GetComponents")]
    private GameObject tmp;
    public itemboxcontroller currentbox;
    public popupUI pu;
    public minigamemanager mg;
    public minigameUI mgui;
    public itemmanager im;
    public timer tm;
    public scanner sc;
    public playercontroller pc;
    public bosscontroller boscon;
    public TMP_Text hint_count;
    public TMP_Text stagetext;
    //public Image keyimage;
    public Image speedcount;
    public Slider healthSlider;
    public Transform player;
    public inneritem[] innerItems;
    public RectTransform[] health_list;
    public RectTransform[] health_lose_list;
    public RectTransform[] item_list;
    public RectTransform[] ui_list;
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
        Debug.Log($"Scene Loaded: {scene.name}");

        InitializeScene(scene);
    }

    private void InitializeScene(Scene scene)
    {
        // 씬이 초기화되면 로그 띄움
        Debug.Log($"Initializing scene: {scene.name}");

        // 각 변수들 초기화
        maxstage = 3;
        //stealthTime = 0f;
        originspeed = 4f;
        speed_for_boss_stage = 4f;

        // MainMenu Scene
        if (!is_running)
        {
            audiomanager.Instance.bossdash.Stop();
            ui_list = new RectTransform[11];
            tmp = GameObject.Find("SettingsUI");
            if (tmp != null) ui_list[10] = tmp.GetComponent<RectTransform>();
            if (ui_list[10] != null) ui_list[10].gameObject.SetActive(false);
            tmp = GameObject.Find("LevelUI");
            if (tmp != null) ui_list[1] = tmp.GetComponent<RectTransform>();
            if (ui_list[1] != null) ui_list[1].gameObject.SetActive(false);
            tmp = GameObject.Find("MainMenuUI");
            if (tmp != null) ui_list[0] = tmp.GetComponent<RectTransform>();
            //if (ui_list[0] != null) ui_list[1].gameObject.SetActive(false);
            /*tmp = GameObject.Find("ChatUI");
            if (tmp != null)
            {
                ui_list[6] = tmp.GetComponent<RectTransform>();
                ui_list[6].gameObject.SetActive(false);
            }*/
        }

        // Ingame 들어가면 초기화 작업 실행
        if (is_ingame == true)
        {
            audiomanager.Instance.bossdash.Stop();
            is_rannum = true;
            is_rannum2 = true;
            is_mgset = false;
            StartCoroutine(ResetCoroutine());

            // 난이도 선택에 따라 게임 설정들 변경
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

            // 필요한 컴포넌트들 가져오기
            tmp = GameObject.Find("MinigameManager");
            if (tmp != null) mg = tmp.GetComponent<minigamemanager>();
            tmp = GameObject.Find("ItemManager");
            if (tmp != null) im = tmp.GetComponent<itemmanager>();
            tmp = GameObject.Find("TIME");
            if (tmp != null) tm = tmp.GetComponent<timer>();
            tmp = GameObject.Find("Scanner");
            if (tmp != null) sc = tmp.GetComponent<scanner>();
            tmp = GameObject.Find("hintcount");
            if (tmp != null) hint_count = tmp.GetComponent<TMP_Text>();
            tmp = GameObject.Find("stagetext");
            if (tmp != null) stagetext = tmp.GetComponent<TMP_Text>();
            //tmp = GameObject.Find("keyimage");
            //if (tmp != null) keyimage = tmp.GetComponent<Image>();
            tmp = GameObject.Find("Player");
            if (tmp != null) player = tmp.GetComponent<Transform>();
            if (player != null) pc = player.GetComponent<playercontroller>();

            // ui_list에 필요한 UI들 미리 가져오기
            ui_list = new RectTransform[11];
            tmp = GameObject.Find("InGameUI");
            if (tmp != null) ui_list[0] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("MiniGameUI");
            if (tmp != null) ui_list[1] = tmp.GetComponent<RectTransform>();
            if (ui_list[1] != null) mgui = ui_list[1].GetComponent<minigameUI>();
            tmp = GameObject.Find("PauseMenuUI");
            if (tmp != null) ui_list[2] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("GRayout5X5");
            if (tmp != null) ui_list[3] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("GRayout6X6");
            if (tmp != null) ui_list[4] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("GRayout7X7");
            if (tmp != null) ui_list[5] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("ChatUI");
            if (tmp != null) ui_list[6] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("OverUI");
            if (tmp != null) ui_list[7] = tmp.GetComponent<RectTransform>();
            //tmp = GameObject.Find("Canvas");
            //iftmp != null) CanvasList = tmp.GetComponentsInChildren<RectTransform>(true);
            tmp = GameObject.Find("PopupUI");
            if (tmp != null) ui_list[8] = tmp.GetComponent<RectTransform>();
            if (ui_list[8] != null) pu = ui_list[8].GetComponent<popupUI>();
            if (pu != null) pu.GetText();
            if (ui_list[8] != null) ui_list[8].gameObject.SetActive(false);
            tmp = GameObject.Find("SettingsUI");
            if (tmp != null) ui_list[10] = tmp.GetComponent<RectTransform>();
            if (ui_list[10] != null) ui_list[10].gameObject.SetActive(false);

            // 체력과 아이템 UI 자식들 가져오기
            tmp = GameObject.Find("HPUI");
            health_list = tmp.GetComponentsInChildren<RectTransform>();
            tmp = GameObject.Find("LOSEHPUI");
            health_lose_list = tmp.GetComponentsInChildren<RectTransform>();
            tmp = GameObject.Find("ITEMUI");
            item_list = tmp.GetComponentsInChildren<RectTransform>();

            //스피드 카운트 렌더러
            if (item_list != null) speedcount = item_list[3].GetComponent<Image>();

            // 아이템 UI들 업데이트
            updateshoe();
            updateitemui();

            // Find로 찾았으니 UI List들 다시 비활성화
            if (ui_list != null) ui_list[1].gameObject.SetActive(false);
            if (ui_list != null) ui_list[2].gameObject.SetActive(false);
            if (ui_list != null) ui_list[3].gameObject.SetActive(false);
            if (ui_list != null) ui_list[4].gameObject.SetActive(false);
            if (ui_list != null) ui_list[5].gameObject.SetActive(false);
            if (ui_list != null) ui_list[6].gameObject.SetActive(false);
            if (ui_list != null) ui_list[7].gameObject.SetActive(false);
            if (health_list != null) health_list[6].gameObject.SetActive(false);
            if (health_list != null) health_list[7].gameObject.SetActive(false);
            if (health_list != null) health_list[8].gameObject.SetActive(false);
            if (health_lose_list != null)
            {
                for (int i = maxHealth + 1; i < health_lose_list.Length; i++)
                {
                    health_lose_list[i].gameObject.SetActive(false);
                }
            }
            if (item_list != null) item_list[4].gameObject.SetActive(false);
            if (item_list != null) item_list[5].gameObject.SetActive(false);
            if (item_list != null) item_list[6].gameObject.SetActive(false);
            if (item_list != null) item_list[7].gameObject.SetActive(false);

            // 시간 정상화, 미니게임 OFF
            Time.timeScale = 1;
            is_minigame = false;

            // 미니게임용 이미지랑 보기 리스트 가져오기
            if (spr_list.Length == 0) spr_list = mg.ImageSet();
            if (ans_list.Length == 0) ans_list = mg.AnswerSet();

            StartCoroutine(SetBorder());
            StartCoroutine(SetItemScripts());
            SetItemIcon();
        }
        // 보스전이면
        if (is_boss)
        {
            audiomanager.Instance.bossdash.Stop();
            tmp = GameObject.Find("Player");
            if (tmp != null) player = tmp.GetComponent<Transform>();
            if (player != null) pc = player.GetComponent<playercontroller>();
            tmp = GameObject.Find("Slider");
            if (tmp != null) healthSlider = tmp.GetComponent<Slider>();
            tmp = GameObject.Find("BOSS");
            if (tmp != null) boscon = tmp.GetComponent<bosscontroller>();
            ui_list = new RectTransform[11];
            tmp = GameObject.Find("PauseMenuUI");
            if (tmp != null) ui_list[2] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("ChatUI");
            if (tmp != null) ui_list[6] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("OverUI");
            if (tmp != null) ui_list[7] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("EndingUI");
            if (tmp != null) ui_list[9] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("SettingsUI");
            if (tmp != null) ui_list[10] = tmp.GetComponent<RectTransform>();

            // UI 찾은 후 비활성화
            if (ui_list[10] != null) ui_list[10].gameObject.SetActive(false);
            if (ui_list[2] != null) ui_list[2].gameObject.SetActive(false);
            if (ui_list[6] != null) ui_list[6].gameObject.SetActive(false);
            if (ui_list[7] != null) ui_list[7].gameObject.SetActive(false);
            if (ui_list[9] != null) ui_list[9].gameObject.SetActive(false);

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
            tmp = GameObject.Find("HPUI");
            // 보스전용 체력UI
            health_list = tmp.GetComponentsInChildren<RectTransform>();
            tmp = GameObject.Find("LOSEHPUI");
            health_lose_list = tmp.GetComponentsInChildren<RectTransform>();
            armor = 0;
            armor_item = 0;
            if (health_lose_list != null)
            {
                for (int i = maxHealth + 1; i < health_lose_list.Length; i++)
                {
                    health_lose_list[i].gameObject.SetActive(false);
                }
            }
            if (health_list != null && health_list.Length != 0 && armor == 0) health_list[8].gameObject.SetActive(false);
            switch (diff)
            {
                case 1:
                    health = 5;
                    maxHealth = 5;
                    health_item = 0;
                    break;
                case 2:
                    health = 3;
                    maxHealth = 3;
                    health_item = 0;
                    break;
                case 3:
                    health = 1;
                    maxHealth = 1;
                    health_item = 0;
                    break;
                default:
                    break;
            }
            tmp = GameObject.Find("ITEMUI");
            item_list = tmp.GetComponentsInChildren<RectTransform>();
            //스피드 카운트 렌더러
            if (item_list != null) speedcount = item_list[3].GetComponent<Image>();
            updateitemui();
            updateshoe();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager is initialized");
        is_attacked_speed = false;
        is_preview = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0 && (is_boss || is_ingame))
        {
            GameOver();
        }

        if (is_ingame == true)
        {
            if (is_rannum)
            {
                if (mg != null) rannum3 = mg.RanNumGen();
                is_rannum = false;
            }

            if (is_rannum2)
            {
                if (mg != null) rannum3_2 = mg.RanNumGen();
                is_rannum2 = false;
                mgui.UpdateMinigame();
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
                if (ui_list[2] != null)
                {
                    if (Time.timeScale == 1)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        if (!ui_list[1].gameObject.activeSelf) Time.timeScale = 1;
                    }
                    audiomanager.Instance.menusfx.Play();
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf);
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                // 레벨 및 난이도 별 표시할 맵
                switch (diff)
                {
                    case 1:
                        // 값1일 때 실행할 코드
                        if (ui_list[3] != null && stage == 1)
                        {
                            ui_list[3].gameObject.SetActive(!ui_list[3].gameObject.activeSelf);
                        }
                        else if (ui_list[4] != null && stage == 2)
                        {
                            ui_list[4].gameObject.SetActive(!ui_list[4].gameObject.activeSelf);
                        }
                        else if (ui_list[5] != null && stage == 3)
                        {
                            ui_list[5].gameObject.SetActive(!ui_list[5].gameObject.activeSelf);
                        }
                        break;
                    case 2:
                        // 값2일 때 실행할 코드
                        if (ui_list[3] != null && stage == 1)
                        {
                            ui_list[3].gameObject.SetActive(!ui_list[3].gameObject.activeSelf);
                        }
                        else if (ui_list[4] != null && stage == 2)
                        {
                            ui_list[4].gameObject.SetActive(!ui_list[4].gameObject.activeSelf);
                        }
                        else if (ui_list[5] != null && stage == 3)
                        {
                            ui_list[5].gameObject.SetActive(!ui_list[5].gameObject.activeSelf);
                        }
                        break;
                    case 3:
                        // 값3일 때 실행할 코드
                        if (ui_list[4] != null && stage == 1)
                        {
                            ui_list[4].gameObject.SetActive(!ui_list[4].gameObject.activeSelf);
                        }
                        else if (ui_list[5] != null && stage >= 2)
                        {
                            ui_list[5].gameObject.SetActive(!ui_list[5].gameObject.activeSelf);
                        }
                        break;
                    default:
                        Debug.LogError("Out of Diff!");
                        break;
                }
                audiomanager.Instance.menusfx.Play();
            }

            if (is_closebox == true && is_delay == false && is_mgset == true && is_catch == true && !currentbox.isOpen && currentbox.ii.is_set)
            {
                if (Input.GetKeyDown(interactKey))
                {
                    audiomanager.Instance.menusfx.Play();
                    ui_list[1].gameObject.SetActive(true);
                }
            }

            if (is_delay && is_CoroutineRunning == false)
            {
                StartCoroutine(SelectedIncurrect());
                StartCoroutine(WaitFiveSecond());
            }

            if (hint_count != null) hint_count.text = key + " / " + req_key;
            if (health_list != null && health_list.Length != 0) updatehealth();
            if (item_list != null && item_list.Length != 0) updateshoe();
            if (stagetext != null) stagetext.text = "S" + stage;
        }

        if (is_boss)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ESC 메뉴 여닫기
                if (ui_list[2] != null)
                {
                    if (Time.timeScale == 1)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                    audiomanager.Instance.menusfx.Play();
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf);
                }
            }
            updatehealth();
            if (boss_health <= 0) StartCoroutine(EndingCoroutine());
        }

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            if (!is_ingame)
            {
                is_ingame = true;
            }
            if (is_boss)
            {
                is_boss = false;
            }
            is_running = true;
            diff = 1;
            stage = 1;
            key = 0;
            key_item = 0;
            Time.timeScale = 1;
            speed = originspeed;
            SceneManager.LoadScene("Test");
        }*/

        if (Input.GetKeyDown(KeyCode.T))
        {
            is_running = false;
            is_ingame = false;
            is_boss = false;
            SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (is_ingame)
            {
                is_ingame = false;
            }
            if (!is_boss)
            {
                is_boss = true;
            }
            health = 5;
            key = 0;
            key_item = 0;
            diff = 1;
            stage = 4;
            Time.timeScale = 1;
            is_running = true;
            speed = speed_for_boss_stage;
            audiomanager.Instance.mainmenubgm.Stop();
            SceneManager.LoadScene("Stage_Boss");
        }
        if (Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (is_ingame)
            {
                is_ingame = false;
            }
            if (!is_boss)
            {
                is_boss = true;
            }
            health = 5;
            key = 0;
            key_item = 0;
            diff = 2;
            stage = 4;
            Time.timeScale = 1;
            is_running = true;
            speed = speed_for_boss_stage;
            audiomanager.Instance.mainmenubgm.Stop();
            SceneManager.LoadScene("Stage_Boss");
        }
        if (Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (is_ingame)
            {
                is_ingame = false;
            }
            if (!is_boss)
            {
                is_boss = true;
            }
            health = 5;
            key = 0;
            key_item = 0;
            diff = 3;
            stage = 4;
            Time.timeScale = 1;
            is_running = true;
            speed = speed_for_boss_stage;
            audiomanager.Instance.mainmenubgm.Stop();
            SceneManager.LoadScene("Stage_Boss");
        }

        /*if (Input.GetKeyDown(KeyCode.U))
        {
            if (!is_ingame)
            {
                is_ingame = true;
            }
            if (is_boss)
            {
                is_boss = false;
            }
            is_running = true;
            diff = 1;
            stage = 1;
            key = 0;
            key_item = 0;
            Time.timeScale = 1;
            speed = originspeed;
            SceneManager.LoadScene("PlayScene");
        }*/
    }

    private IEnumerator WaitFiveSecond()
    {
        // 5초 기다리고 응답없으면 프리셋 적용
        yield return new WaitForSeconds(5f);
        if (!is_catch)
        {
            Debug.Log("응답 너무 느림");
            mg.FailRequest();
            is_catch = true;
            if (mgui != null) mgui.UpdateMinigame();
        }
    }

    public IEnumerator SelectedIncurrect()
    {
        is_CoroutineRunning = true;
        // 미니게임 오답 패널티
        yield return new WaitForSeconds(5f);
        is_delay = false;
        is_CoroutineRunning = false;
        Debug.Log("패널티 해제");
    }

    public int SelectItem(int rannum1)
    {
        int itemnum = 10;

        if (rannum1 >= 1 && rannum1 <= 50)
        {
            itemnum = 1;
            //Debug.Log("체력 회복");
        }
        else if (rannum1 >= 51 && rannum1 <= 70)
        {
            itemnum = 2;
            //Debug.Log("쉴드 획득");
        }
        else if (rannum1 >= 71 && rannum1 <= 81)
        {
            itemnum = 3;
            //Debug.Log("이속 증가");
        }
        else if (rannum1 >= 82 && rannum1 <= 87)
        {
            itemnum = 0;
            //Debug.Log("최대 체력 증가");
        }
        else if (rannum1 >= 88 && rannum1 <= 92)
        {
            itemnum = 5;
            //Debug.Log("피격 시 이속 증가");
        }
        else if (rannum1 >= 93 && rannum1 <= 94)
        {
            itemnum = 6;
            //Debug.Log("감지 시 이속 증가");
        }
        else if (rannum1 >= 95 && rannum1 <= 99)
        {
            itemnum = 7;
            //Debug.Log("상자 투시");
        }
        else if (rannum1 == 100)
        {
            itemnum = 4;
            //Debug.Log("부활 템 획득!");
        }
        else
        {
            Debug.LogError("Out of ItemNum");
        }
        //im.getItem(im.itemlist[itemnum]);
        return itemnum;
    }

    public void updatehealth()
    {
        switch (health)
        {
            case 0:
                health_list[1].gameObject.SetActive(false);
                health_list[2].gameObject.SetActive(false);
                health_list[3].gameObject.SetActive(false);
                health_list[4].gameObject.SetActive(false);
                health_list[5].gameObject.SetActive(false);
                health_list[6].gameObject.SetActive(false);
                health_list[7].gameObject.SetActive(false);
                break;
            case 1:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(false);
                health_list[3].gameObject.SetActive(false);
                health_list[4].gameObject.SetActive(false);
                health_list[5].gameObject.SetActive(false);
                health_list[6].gameObject.SetActive(false);
                health_list[7].gameObject.SetActive(false);
                break;
            case 2:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(true);
                health_list[3].gameObject.SetActive(false);
                health_list[4].gameObject.SetActive(false);
                health_list[5].gameObject.SetActive(false);
                health_list[6].gameObject.SetActive(false);
                health_list[7].gameObject.SetActive(false);
                break;
            case 3:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(true);
                health_list[3].gameObject.SetActive(true);
                health_list[4].gameObject.SetActive(false);
                health_list[5].gameObject.SetActive(false);
                health_list[6].gameObject.SetActive(false);
                health_list[7].gameObject.SetActive(false);
                break;
            case 4:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(true);
                health_list[3].gameObject.SetActive(true);
                health_list[4].gameObject.SetActive(true);
                health_list[5].gameObject.SetActive(false);
                health_list[6].gameObject.SetActive(false);
                health_list[7].gameObject.SetActive(false);
                break;
            case 5:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(true);
                health_list[3].gameObject.SetActive(true);
                health_list[4].gameObject.SetActive(true);
                health_list[5].gameObject.SetActive(true);
                health_list[6].gameObject.SetActive(false);
                health_list[7].gameObject.SetActive(false);
                break;
            case 6:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(true);
                health_list[3].gameObject.SetActive(true);
                health_list[4].gameObject.SetActive(true);
                health_list[5].gameObject.SetActive(true);
                health_list[6].gameObject.SetActive(true);
                health_list[7].gameObject.SetActive(false);
                break;
            case 7:
                health_list[1].gameObject.SetActive(true);
                health_list[2].gameObject.SetActive(true);
                health_list[3].gameObject.SetActive(true);
                health_list[4].gameObject.SetActive(true);
                health_list[5].gameObject.SetActive(true);
                health_list[6].gameObject.SetActive(true);
                health_list[7].gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Out of Health!");
                break;
        }
    }

    public void updateshoe()
    {
        string spriteName = "Speed";
        spriteName += speed_item;
        speedcount.sprite = Resources.Load<Sprite>(spriteName);
    }

    // 보스 체력 비율 반환
    public float GetNormalizedHealth()
    {
        return boss_health / boss_max_health;
    }

    public void GameOver()
    {
        if (is_ressurection)
        {
            audiomanager.Instance.reserrection.Play();
            health = 1;
            ressurection_item--;
            is_ressurection = false;
            item_list[4].gameObject.SetActive(false);
        }
        else if (is_running)
        {
            //StartCoroutine(WaitPointSecond());
            updatehealth();
            audiomanager.Instance.ingamebgm.Stop();
            audiomanager.Instance.bossstagebgm.Stop();
            audiomanager.Instance.bossdash.Stop();
            audiomanager.Instance.gameover.Play();
            if (pc != null) pc.Dead();
            is_running = false;
            is_ingame = false;
            Debug.Log("캐릭터 사망!");
            if (ui_list != null && ui_list.Length != 0) ui_list[7].gameObject.SetActive(true);
            //Time.timeScale = 0;
            speed = 0;
            StartCoroutine(WaitThreeSecond());
        }
    }
    public IEnumerator SetItemScripts()
    {
        yield return new WaitForSeconds(1f);
        // InnerItem 스크립트를 가진 모든 오브젝트 찾기
        innerItems = FindObjectsOfType<inneritem>(true);
        // 모든 상자에 키랑 아이템 할당
        if (innerItems.Length >= req_key) SetItems();
    }

    public IEnumerator SetBorder()
    {
        yield return new WaitForSeconds(1f);
        if (sc != null) sc.UpdateBorder();
    }

    public IEnumerator WaitPointSecond()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator WaitThreeSecond()
    {
        // 3초 기다리고 타이틀 화면으로 감
        yield return new WaitForSeconds(3f);
        if (is_ingame)
        {
            is_ingame = false;
        }
        if (is_boss)
        {
            is_boss = false;
        }
        SceneManager.LoadScene("MainMenuScene");
    }

    public IEnumerator WaitThreeSecond2()
    {
        // 3초 기다리기
        yield return new WaitForSeconds(3f);
        ui_list[8].gameObject.SetActive(false);
    }

    public void SetItems()
    {
        // 초기화 작업
        for (int k = 0; k < innerItems.Length; k++)
        {
            innerItems[k].is_set = false;
        }

        // 열쇠를 미리 세팅
        int[] rankey = mg.RanNumGenWithNum(req_key, innerItems.Length);

        foreach (int i in rankey)
        {
            innerItems[i].itemnumber = 8;
            innerItems[i].is_set = true;
        }

        // 나머지 아이템 랜덤 세팅
        for (int j = 0; j < innerItems.Length; j++)
        {
            if (!innerItems[j].is_set)
            {
                innerItems[j].itemnumber = SelectItem(UnityEngine.Random.Range(1, 101));
                innerItems[j].is_set = true;
            }
        }
    }

    public void SetItemIcon()
    {
        if (health_list != null && health_list.Length != 0)
        {
            if (armor_item >= 1) health_list[8].gameObject.SetActive(true);
        }
        if (item_list != null && item_list.Length != 0)
        {
            if (ressurection_item >= 1) item_list[4].gameObject.SetActive(true);
            if (is_attacked_speed) item_list[5].gameObject.SetActive(true);
            if (stealth_item >= 1) item_list[6].gameObject.SetActive(true);
            if (preview_item >= 1) item_list[7].gameObject.SetActive(true);
        }
    }

    public void updateitemui()
    {
        if (armor_item >= 1 && health_list != null) health_list[8].gameObject.SetActive(true); else health_list[8].gameObject.SetActive(false);
        if (is_ressurection && item_list != null) item_list[4].gameObject.SetActive(true); else item_list[4].gameObject.SetActive(false);
        if (is_attacked_speed && item_list != null) item_list[5].gameObject.SetActive(true); else item_list[5].gameObject.SetActive(false);
        if (is_stealth && item_list != null) item_list[6].gameObject.SetActive(true); else item_list[6].gameObject.SetActive(false);
        if (is_preview && item_list != null) item_list[7].gameObject.SetActive(true); else item_list[7].gameObject.SetActive(false);
    }

    public IEnumerator EndingCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        ui_list[9].gameObject.SetActive(true);
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
        tmpspeed = speed;
        speed *= 1.3f;
        yield return new WaitForSeconds(3f);
        speed = tmpspeed;
        pc.ASCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }

    public IEnumerator STCoroutine()
    {
        tmpspeed = speed;
        speed *= is_attacked_speed ? 1.3f : 1.2f;
        yield return new WaitForSeconds(1f);
        speed = tmpspeed;
        pc.STCoroutine = null; // 코루틴이 끝난 후 null로 초기화
    }
}
