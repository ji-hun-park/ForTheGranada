using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static DataManager Instance;
    
    Scene current_scene;

    private void Awake()
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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveSettings();
    }
    
    private void SaveSettings()
    {
        current_scene = SceneManager.GetActiveScene();
        if (current_scene.name != "MainMenuScene")
        {
            GameSave();
        }
    }

    public void GameSave()
    {
        PlayerPrefs.SetInt("Stage", GameManager.Instance.stage);
        PlayerPrefs.SetInt("Diff", GameManager.Instance.diff);
        PlayerPrefs.SetInt("Health", GameManager.Instance.health);
        PlayerPrefs.SetInt("Armor", GameManager.Instance.armor);
        PlayerPrefs.SetInt("MaxHealth", GameManager.Instance.maxHealth);
        PlayerPrefs.SetFloat("Speed", GameManager.Instance.speed);
        PlayerPrefs.SetFloat("StealthTime", GameManager.Instance.stealthTime);
        PlayerPrefs.SetInt("HealthItem", GameManager.Instance.health_item);
        PlayerPrefs.SetInt("ArmorItem", GameManager.Instance.armor_item);
        PlayerPrefs.SetInt("StealthItem", GameManager.Instance.stealth_item);
        PlayerPrefs.SetInt("SpeedItem", GameManager.Instance.speed_item);
        PlayerPrefs.SetInt("HasteItem", GameManager.Instance.haste_item);
        PlayerPrefs.SetInt("PreviewItem", GameManager.Instance.preview_item);
        PlayerPrefs.SetInt("RessurectionItem", GameManager.Instance.resurrection_item);
        PlayerPrefs.SetString("AS", GameManager.Instance.is_attacked_speed ? "True" : "False");
        PlayerPrefs.SetString("RS", GameManager.Instance.is_resurrection ? "True" : "False");
        PlayerPrefs.SetString("PR", GameManager.Instance.is_preview ? "True" : "False");
        PlayerPrefs.SetString("ST", GameManager.Instance.is_stealth ? "True" : "False");
        PlayerPrefs.Save();
        Debug.Log("save");
    }

    public void GameLoad()
    {
        audiomanager.Instance.menusfx.Play();
        audiomanager.Instance.mainmenubgm.Stop();
        audiomanager.Instance.ingamebgm.Play();
        audiomanager.Instance.ingamebgm.loop = true;
        int stage = PlayerPrefs.GetInt("Stage");
        string stage_scene = "";
        GameManager.Instance.key = 0;
        GameManager.Instance.key_item = 0;
        if (stage <= 3)
        {
            stage_scene = "Stage_" + stage;
            if (!GameManager.Instance.is_ingame)
            {
                GameManager.Instance.is_ingame = true;
            }
            if (GameManager.Instance.is_boss)
            {
                GameManager.Instance.is_boss = false;
            }
        }
        else
        {
            stage_scene = "Stage_Boss";
            GameManager.Instance.boss_health = GameManager.Instance.boss_max_health;
            if (GameManager.Instance.is_ingame)
            {
                GameManager.Instance.is_ingame = false;
            }
            if (!GameManager.Instance.is_boss)
            {
                GameManager.Instance.is_boss = true;
            }
        }

        Scene scene = SceneManager.GetSceneByName(stage_scene);

        GameManager.Instance.is_running = true;
        GameManager.Instance.stage = stage;
        GameManager.Instance.diff = PlayerPrefs.GetInt("Diff");
        GameManager.Instance.maxHealth = PlayerPrefs.GetInt("MaxHealth");
        GameManager.Instance.health = PlayerPrefs.GetInt("Health");
        GameManager.Instance.armor = PlayerPrefs.GetInt("Armor");
        GameManager.Instance.speed = PlayerPrefs.GetFloat("Speed");
        SceneManager.LoadScene(stage_scene);

        GameManager.Instance.stealthTime = PlayerPrefs.GetInt("StealthTime");
        GameManager.Instance.health_item = PlayerPrefs.GetInt("HealthItem");
        GameManager.Instance.armor_item = PlayerPrefs.GetInt("ArmorItem");
        GameManager.Instance.stealthTime = PlayerPrefs.GetFloat("StealthTime");
        GameManager.Instance.speed_item = PlayerPrefs.GetInt("SpeedItem");

        GameManager.Instance.haste_item = PlayerPrefs.GetInt("HasteItem");
        if (GameManager.Instance.haste_item == 1)
        {
            GameManager.Instance.is_attacked_speed = true;
        }
        GameManager.Instance.stealth_item = PlayerPrefs.GetInt("StealthItem");
        if (GameManager.Instance.stealth_item == 1)
        {
            GameManager.Instance.is_stealth = true;
        }
        GameManager.Instance.preview_item = PlayerPrefs.GetInt("PreviewItem");
        if (GameManager.Instance.preview_item == 1)
        {
            GameManager.Instance.is_preview = true;
        }
        GameManager.Instance.resurrection_item = PlayerPrefs.GetInt("RessurectionItem");
        if (GameManager.Instance.resurrection_item == 1)
        {
            GameManager.Instance.is_resurrection = true;
        }

        if (PlayerPrefs.GetString("AS") == "True") GameManager.Instance.is_attacked_speed = true;
        if (PlayerPrefs.GetString("RS") == "True") GameManager.Instance.is_resurrection = true;
        if (PlayerPrefs.GetString("PR") == "True") GameManager.Instance.is_preview = true;
        if (PlayerPrefs.GetString("ST") == "True") GameManager.Instance.is_stealth = true;

        UIManager.Instance.UpdateHealth();
        UIManager.Instance.SetItemIcon();
        UIManager.Instance.UpdateItemUI();

        Debug.Log("load");
    }
}
