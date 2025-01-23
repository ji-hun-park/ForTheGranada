using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static APIManager Instance;
    
    // 상수들
    private const int maxTokens = 6;
    private static readonly string path;
    private static readonly string promptMessage;
    private static readonly string apiUrl;
    private static readonly string apiKey;
    
    // 변수들
    public string APIResponse{get; set;}

    [System.Serializable]
    private class ApiKeyData
    {
        public string apikey;
    }

    static APIManager()
    {
        path = Path.Combine(Application.streamingAssetsPath, "config.json");
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            apiKey = JsonUtility.FromJson<ApiKeyData>(json).apikey;
        }

        apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + apiKey;
        promptMessage = "3개의 이미지 공통점을 너무 포괄적이지 않은 단어로 단 1개만! 출력해 뒤에 입니다 붙이지 마! 답변으로 판타지, 픽셀아트 금지!";
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
    }

    public void RequestStart()
    {
        StartCoroutine(LLMAPIRequest());
    }
    
    private IEnumerator LLMAPIRequest()
    {
        // 전송할 이미지 3장 배열에 담기
        string[] imageNames = new string[3];
        if (GameManager.Instance.rannum3 != null && GameManager.Instance.rannum3.Length != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                string num;
                if (GameManager.Instance.rannum3[i] == 100)
                {
                    num = GameManager.Instance.rannum3[i].ToString();
                }
                else if (GameManager.Instance.rannum3[i] >= 10)
                {
                    num = "0" + GameManager.Instance.rannum3[i].ToString();
                }
                else
                {
                    num = "00" + GameManager.Instance.rannum3[i].ToString();
                }
                imageNames[i] = "MG_1_" + num;
            }
        }

        List<string> base64Images = new List<string>();

        if (imageNames.Length != 0)
        {
            foreach (string imageName in imageNames)
            {
                // Resources에서 이미지 가져오기
                Texture2D image = Resources.Load<Texture2D>(imageName);

                if (image == null)
                {
                    Debug.LogError($"Image '{imageName}' not found in Resources folder.");
                    yield break; // 로그에러 띄우고 코루틴 끝내기
                }

                Texture2D uncompressedImage = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
                uncompressedImage.SetPixels(image.GetPixels());
                uncompressedImage.Apply();

                // 이미지를 인라인 데이터로 보내기 위해 바이트형 배열에 담음
                byte[] imageBytes = image.EncodeToJPG();
                string base64Image = Convert.ToBase64String(imageBytes); // Base64 스트링으로 변환
                base64Images.Add(base64Image);
            }
        }

        // POST로 보내기 위해 JSON 형식 데이터로 만듬
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + promptMessage + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[0] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[1] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[2] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

        // UnityWebRequest 보내기 위해 필요한 것 들
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Header 작성
        request.SetRequestHeader("Content-Type", "application/json");

        // 리퀘스트 보냄
        yield return request.SendWebRequest();

        // 성공하면 응답받고 텍스트 파싱
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            ParseResponse(responseText);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
            GameManager.Instance.mg.FailRequest();
            GameManager.Instance.is_catch = true;
            if (GameManager.Instance.mgui != null) GameManager.Instance.mgui.UpdateMinigame();
        }
    }
    
    void ParseResponse(string jsonResponse)
    {
        // JSON 파싱
        JObject response = JObject.Parse(jsonResponse);

        // candidates[0].content.parts[0].text 파싱
        string modelResponse = response["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

        if (modelResponse != null)
        {
            Debug.Log("Model Response: " + modelResponse);
            APIResponse = modelResponse;
            GameManager.Instance.is_catch = true;
            if (GameManager.Instance.mgui != null) GameManager.Instance.mgui.UpdateMinigame();
        }
        else
        {
            Debug.LogError("Could not parse the response.");
            GameManager.Instance.mg.FailRequest();
            GameManager.Instance.is_catch = true;
            if (GameManager.Instance.mgui != null) GameManager.Instance.mgui.UpdateMinigame();
        }
    }
}
