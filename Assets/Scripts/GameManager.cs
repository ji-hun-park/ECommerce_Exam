using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    // �̱��� ���� ����
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxtokens = 500;
    public string promptmessage = "���� �ζ��� �̹����� ���� ������Ʈ�� ������ �������� �ؼ� ���丮�� ����� ��! ���� : ";
    public string concept;
    public string APIResponse = null;
    public int IMGNUM;
    public List<string> stories = new List<string>();
    private string apiUrl;
    private string apiKey;
    [System.Serializable]
    private class ApiKeyData
    {
        public string apiKey;
    }

    [Header("Flags")]
    public bool is_CoroutineRunning = false;
    public bool is_mgset = false;
    public bool is_catch = false;
    public bool is_contents = false;
    public bool is_rannum = true;
    [SerializeField]
    private bool _is_ingame = false;

    [Header("GetComponents")]
    public minigamemanager mg;
    public RectTransform[] ui_list;
    public int[] rannum8;
    public Sprite[] spr_list;

    public bool is_ingame
    {
        get => _is_ingame;
        set
        {
            _is_ingame = value;
            Debug.Log($"is_ingame �� �����: {_is_ingame}");
        }
    }

    void Awake()
    {
        // ?���??�� ?��?��?��?�� ?��?��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ?�� ?��?�� ?�� ?��?��?���? ?��?���? ?��?��
        }
        else
        {
            Destroy(gameObject); // 중복?�� GameManager�? ?��?��?���? ?��?���? ?��?��
        }

        string path = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            apiKey = JsonUtility.FromJson<ApiKeyData>(json).apiKey;
        }

        apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + apiKey;
    }

    private void OnEnable()
    {
        // ?��?�� 로드?�� ?�� ?���?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?�� 로드 ?��벤트 ?��?��
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // ?�� 로드 ?�� ?��?��?�� 초기?�� 로직
        InitializeScene(scene);
    }

    private void InitializeScene(Scene scene)
    {
        // ?���? 초기?�� 로직
        Debug.Log($"Initializing scene: {scene.name}");
        
        /*try
        {
            // GameObject.Find ���
            mg = GameObject.Find("MinigameManager").GetComponent<minigamemanager>();

            // null üũ
            if (mg == null)
            {
                throw new System.Exception("GameObject not found: NonExistentObject");
            }

            Debug.Log("Object found: " + mg.name);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
        }*/

        if (!is_contents) mg = GameObject.Find("MinigameManager").GetComponent<minigamemanager>();

        if (is_ingame == true)
        {
            ui_list = new RectTransform[5];
            ui_list[1] = GameObject.Find("MiniGameUI").GetComponent<RectTransform>();
            ui_list[2] = GameObject.Find("PauseMenuUI").GetComponent<RectTransform>();
            ui_list[3] = GameObject.Find("PromptUI").GetComponent<RectTransform>();
            ui_list[4] = GameObject.Find("ResultUI").GetComponent<RectTransform>();

            // 불필?��?�� ui 비활?��
            ui_list[2].gameObject.SetActive(false);
            ui_list[3].gameObject.SetActive(false);
            ui_list[4].gameObject.SetActive(false);

            if (spr_list.Length == 0) spr_list = mg.ImageSet();

            GameObject.Find("MiniGameUI").GetComponent<minigameUI>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager is initialized");
    }

    // Update is called once per frame
    void Update()
    {
        if (is_ingame == true)
        {
            if (is_rannum)
            {
                rannum8 = mg.RanNumGen();
                is_rannum = false;
            }

            if (is_mgset == true)
            {
                is_mgset = false;
                Debug.Log("��û ����");
                StartCoroutine(LLMAPIRequest(promptmessage + concept, maxtokens, IMGNUM - 1));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 메뉴�? ?��?��?��?��?�� ?��?���? 비활?��?��?���?, 비활?��?��?��?��?���? ?��?��?��
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf); // 메뉴?�� ?��?��?��/비활?��?�� ?��?�� ?��?��
                }
            }
        }
    }

    private IEnumerator LLMAPIRequest(string prompt, int maxTokens, int imagenumber)
    {
        // ?��미�?? ?��?�� ?���? 배열 (Resources ?��?�� ?��?�� ?��미�?? ?���?)
        string[] imageNames = new string[8];
        for (int i = 0; i < 8; i++)
        {
            string num;
            if (rannum8[i] == 100)
            {
                num = rannum8[i].ToString();
            }
            else if (rannum8[i] >= 10)
            {
                num = "0" + rannum8[i].ToString();
            }
            else
            {
                num = "00" + rannum8[i].ToString();
            }
            imageNames[i] = "MG_1_" + num;
        }

        List<string> base64Images = new List<string>();

        foreach (string imageName in imageNames)
        {
            // Resources ?��?��?��?�� ?��미�??�? 불러?��
            Texture2D image = Resources.Load<Texture2D>(imageName);

            if (image == null)
            {
                Debug.LogError($"Image '{imageName}' not found in Resources folder.");
                yield break; // ?��?�� 발생 ?�� 루프 종료
            }

            Texture2D uncompressedImage = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
            uncompressedImage.SetPixels(image.GetPixels());
            uncompressedImage.Apply();

            // ?��미�?? ?��?��?���? byte 배열�? �??�� (JPG ?��맷으�? ?��코딩)
            byte[] imageBytes = image.EncodeToJPG();
            string base64Image = Convert.ToBase64String(imageBytes); // Base64�? ?��코딩
            base64Images.Add(base64Image);
        }

        // ?���??�� JSON ?��?��?�� ?��?��
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[imagenumber] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

        // UnityWebRequest ?��?��
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // ?��?�� ?��?��
        request.SetRequestHeader("Content-Type", "application/json");

        // ?���? ?��?��
        yield return request.SendWebRequest();

        // ?��?�� 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            ParseResponse(responseText);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
            is_catch = true;
        }
    }

    void ParseResponse(string jsonResponse)
    {
        // JSON ?��?��
        JObject response = JObject.Parse(jsonResponse);

        // candidates[0].content.parts[0].text ?��?���? 추출
        string modelResponse = response["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

        if (modelResponse != null)
        {
            Debug.Log("Model Response: " + modelResponse);
            APIResponse = modelResponse;
            is_catch = true;
        }
        else
        {
            Debug.LogError("Could not parse the response.");
            is_catch = true;
        }
    }
}
