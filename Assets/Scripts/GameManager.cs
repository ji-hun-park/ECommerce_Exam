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
    // 싱글톤 패턴 적용
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxtokens = 800;
    public string promptmessage = "다음 인라인 이미지를 보고 프롬프트에 내용을 설정으로 해서 스토리를 만들어 줘! 설정 : ";
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
            Debug.Log($"is_ingame 값 변경됨: {_is_ingame}");
        }
    }

    void Awake()
    {
        // ?떛湲??넠 ?씤?뒪?꽩?뒪 ?꽕?젙
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ?뵮 ?쟾?솚 ?떆 ?궘?젣?릺吏? ?븡?룄濡? ?꽕?젙
        }
        else
        {
            Destroy(gameObject); // 以묐났?맂 GameManager媛? ?깮?꽦?릺吏? ?븡?룄濡? ?궘?젣
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
        // ?뵮?씠 濡쒕뱶?맆 ?븣 ?샇異?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?뵮 濡쒕뱶 ?씠踰ㅽ듃 ?빐?젣
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // ?뵮 濡쒕뱶 ?썑 ?븘?슂?븳 珥덇린?솕 濡쒖쭅
        InitializeScene(scene);
    }

    private void InitializeScene(Scene scene)
    {
        // ?뵮蹂? 珥덇린?솕 濡쒖쭅
        Debug.Log($"Initializing scene: {scene.name}");
        
        /*try
        {
            // GameObject.Find 사용
            mg = GameObject.Find("MinigameManager").GetComponent<minigamemanager>();

            // null 체크
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

            // 遺덊븘?슂?븳 ui 鍮꾪솢?꽦
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
                Debug.Log("요청 전송");
                StartCoroutine(LLMAPIRequest(promptmessage + concept, maxtokens, IMGNUM - 1));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 硫붾돱媛? ?솢?꽦?솕?릺?뼱 ?엳?쑝硫? 鍮꾪솢?꽦?솕?븯怨?, 鍮꾪솢?꽦?솕?릺?뿀?쑝硫? ?솢?꽦?솕
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf); // 硫붾돱?쓽 ?솢?꽦?솕/鍮꾪솢?꽦?솕 ?긽?깭 ?쟾?솚
                }
            }
        }
    }

    private IEnumerator LLMAPIRequest(string prompt, int maxTokens, int imagenumber)
    {
        // ?씠誘몄?? ?뙆?씪 ?씠由? 諛곗뿴 (Resources ?뤃?뜑 ?궡?쓽 ?씠誘몄?? ?씠由?)
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
            // Resources ?뤃?뜑?뿉?꽌 ?씠誘몄??瑜? 遺덈윭?샂
            Texture2D image = Resources.Load<Texture2D>(imageName);

            if (image == null)
            {
                Debug.LogError($"Image '{imageName}' not found in Resources folder.");
                yield break; // ?뿉?윭 諛쒖깮 ?떆 猷⑦봽 醫낅즺
            }

            Texture2D uncompressedImage = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
            uncompressedImage.SetPixels(image.GetPixels());
            uncompressedImage.Apply();

            // ?씠誘몄?? ?뜲?씠?꽣瑜? byte 諛곗뿴濡? 蹂??솚 (JPG ?룷留룹쑝濡? ?씤肄붾뵫)
            byte[] imageBytes = image.EncodeToJPG();
            string base64Image = Convert.ToBase64String(imageBytes); // Base64濡? ?씤肄붾뵫
            base64Images.Add(base64Image);
        }

        // ?슂泥??븷 JSON ?뜲?씠?꽣 ?깮?꽦
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[imagenumber] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

        // UnityWebRequest ?깮?꽦
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // ?뿤?뜑 ?꽕?젙
        request.SetRequestHeader("Content-Type", "application/json");

        // ?슂泥? ?쟾?넚
        yield return request.SendWebRequest();

        // ?쓳?떟 泥섎━
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            ParseResponse(responseText);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
            is_mgset = true;
        }
    }

    void ParseResponse(string jsonResponse)
    {
        // JSON ?뙆?떛
        JObject response = JObject.Parse(jsonResponse);

        // candidates[0].content.parts[0].text ?븘?뱶瑜? 異붿텧
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
            is_mgset = true;
        }
    }
}
