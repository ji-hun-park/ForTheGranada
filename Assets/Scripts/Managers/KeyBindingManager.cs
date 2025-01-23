using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyBindingManager : MonoBehaviour
{
    // 키 바인딩 데이터를 저장할 딕셔너리
    //private Dictionary<string, KeyCode> keyBindings;

    // 현재 키 설정 중인지 확인
    private string currentBindingKey = null;

    // UI 텍스트를 갱신하기 위한 딕셔너리 (옵션)
    //public Dictionary<string, Text> keyDisplayTexts;

    // 키 바인딩 텍스트
    public Text intkeytext;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 텍스트 초기화
        //intkeytext = GameObject.Find("intkeytext").GetComponent<Text>();
        //if (!keyDisplayTexts.ContainsKey("ITR")) keyDisplayTexts.Add("ITR", intkeytext);
        //keyDisplayTexts["ITR"] = intkeytext;
        // 기존에 저장된 설정이 있다면 불러오기
        //LoadKeyBindings();
        // UI 갱신
        //UpdateKeyDisplayTexts();
    }

    private void Awake()
    {
        // 초기 키 바인딩 설정 (기본값)
        if (!GameManager.Instance.keyBindings.ContainsKey("ITR")) GameManager.Instance.keyBindings["ITR"] = KeyCode.F;

        // 텍스트 초기화
        intkeytext = GameObject.Find("intkeytext").GetComponent<Text>();
        //if (intkeytext.text == "X") intkeytext.text = "F";
        //if (!GameManager.Instance.keyDisplayTexts.ContainsKey("ITR")) GameManager.Instance.keyDisplayTexts.Add("ITR", intkeytext);
        GameManager.Instance.keyDisplayTexts["ITR"] = intkeytext;

        //keyBindings["Jump"] = KeyCode.Space;
        //keyBindings["MoveLeft"] = KeyCode.A;
        //keyBindings["MoveRight"] = KeyCode.D;      
    }

    private void OnEnable()
    {
        intkeytext.text = GameManager.Instance.interactKey.ToString();
        // 기존에 저장된 설정이 있다면 불러오기
        LoadKeyBindings();
        // UI 갱신
        UpdateKeyDisplayTexts();
    }

    private void Update()
    {
        // 키 설정 중일 때 사용자의 입력을 기다림
        if (currentBindingKey != null)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        // 키 설정 저장
                        GameManager.Instance.keyBindings[currentBindingKey] = key;

                        // UI 갱신
                        UpdateKeyDisplayTexts();

                        // 설정 종료
                        currentBindingKey = null;

                        // 저장
                        SaveKeyBindings();

                        break;
                    }
                }
            }
        }
    }

    public void StartRebinding(string actionKey)
    {
        currentBindingKey = actionKey;
    }

    private void UpdateKeyDisplayTexts()
    {
        foreach (var binding in GameManager.Instance.keyBindings)
        {
            if (GameManager.Instance.keyDisplayTexts.ContainsKey(binding.Key))
            {
                GameManager.Instance.keyDisplayTexts[binding.Key].text = binding.Value.ToString();
            }
        }
    }

    private void SaveKeyBindings()
    {
        foreach (var binding in GameManager.Instance.keyBindings)
        {
            PlayerPrefs.SetString(binding.Key, binding.Value.ToString());
            if (binding.Key == "ITR") GameManager.Instance.interactKey = binding.Value;
        }
        PlayerPrefs.Save();
    }

    private void LoadKeyBindings()
    {
        foreach (var binding in GameManager.Instance.keyBindings)
        {
            if (PlayerPrefs.HasKey(binding.Key))
            {
                if (System.Enum.TryParse(PlayerPrefs.GetString(binding.Key), out KeyCode savedKey))
                {
                    GameManager.Instance.keyBindings[binding.Key] = savedKey;
                    if (binding.Key == "ITR") GameManager.Instance.interactKey = binding.Value;
                }
            }
        }
    }

    public KeyCode GetKey(string actionKey)
    {
        return GameManager.Instance.keyBindings.ContainsKey(actionKey) ? GameManager.Instance.keyBindings[actionKey] : KeyCode.None;
    }
}
