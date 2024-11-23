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
    // ½Ì±ÛÅæ ÆĞÅÏ Àû¿ë
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxtokens = 500;
    public string promptmessage = "´ÙÀ½ ÀÎ¶óÀÎ ÀÌ¹ÌÁö¸¦ º¸°í ÇÁ·ÒÇÁÆ®¿¡ ³»¿ëÀ» ¼³Á¤À¸·Î ÇØ¼­ ½ºÅä¸®¸¦ ¸¸µé¾î Áà! ¼³Á¤ : ";
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
            Debug.Log($"is_ingame °ª º¯°æµÊ: {_is_ingame}");
        }
    }

    void Awake()
    {
        // ?‹±ê¸??†¤ ?¸?Š¤?„´?Š¤ ?„¤? •
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ?”¬ ? „?™˜ ?‹œ ?‚­? œ?˜ì§? ?•Š?„ë¡? ?„¤? •
        }
        else
        {
            Destroy(gameObject); // ì¤‘ë³µ?œ GameManagerê°? ?ƒ?„±?˜ì§? ?•Š?„ë¡? ?‚­? œ
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
        // ?”¬?´ ë¡œë“œ?  ?•Œ ?˜¸ì¶?
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ?”¬ ë¡œë“œ ?´ë²¤íŠ¸ ?•´? œ
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // ?”¬ ë¡œë“œ ?›„ ?•„?š”?•œ ì´ˆê¸°?™” ë¡œì§
        InitializeScene(scene);
    }

    private void InitializeScene(Scene scene)
    {
        // ?”¬ë³? ì´ˆê¸°?™” ë¡œì§
        Debug.Log($"Initializing scene: {scene.name}");
        
        /*try
        {
            // GameObject.Find »ç¿ë
            mg = GameObject.Find("MinigameManager").GetComponent<minigamemanager>();

            // null Ã¼Å©
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

            // ë¶ˆí•„?š”?•œ ui ë¹„í™œ?„±
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
                Debug.Log("¿äÃ» Àü¼Û");
                StartCoroutine(LLMAPIRequest(promptmessage + concept, maxtokens, IMGNUM - 1));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ë©”ë‰´ê°? ?™œ?„±?™”?˜?–´ ?ˆ?œ¼ë©? ë¹„í™œ?„±?™”?•˜ê³?, ë¹„í™œ?„±?™”?˜?—ˆ?œ¼ë©? ?™œ?„±?™”
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf); // ë©”ë‰´?˜ ?™œ?„±?™”/ë¹„í™œ?„±?™” ?ƒ?ƒœ ? „?™˜
                }
            }
        }
    }

    private IEnumerator LLMAPIRequest(string prompt, int maxTokens, int imagenumber)
    {
        // ?´ë¯¸ì?? ?ŒŒ?¼ ?´ë¦? ë°°ì—´ (Resources ?´?” ?‚´?˜ ?´ë¯¸ì?? ?´ë¦?)
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
            // Resources ?´?”?—?„œ ?´ë¯¸ì??ë¥? ë¶ˆëŸ¬?˜´
            Texture2D image = Resources.Load<Texture2D>(imageName);

            if (image == null)
            {
                Debug.LogError($"Image '{imageName}' not found in Resources folder.");
                yield break; // ?—?Ÿ¬ ë°œìƒ ?‹œ ë£¨í”„ ì¢…ë£Œ
            }

            Texture2D uncompressedImage = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
            uncompressedImage.SetPixels(image.GetPixels());
            uncompressedImage.Apply();

            // ?´ë¯¸ì?? ?°?´?„°ë¥? byte ë°°ì—´ë¡? ë³??™˜ (JPG ?¬ë§·ìœ¼ë¡? ?¸ì½”ë”©)
            byte[] imageBytes = image.EncodeToJPG();
            string base64Image = Convert.ToBase64String(imageBytes); // Base64ë¡? ?¸ì½”ë”©
            base64Images.Add(base64Image);
        }

        // ?š”ì²??•  JSON ?°?´?„° ?ƒ?„±
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[imagenumber] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

        // UnityWebRequest ?ƒ?„±
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // ?—¤?” ?„¤? •
        request.SetRequestHeader("Content-Type", "application/json");

        // ?š”ì²? ? „?†¡
        yield return request.SendWebRequest();

        // ?‘?‹µ ì²˜ë¦¬
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
        // JSON ?ŒŒ?‹±
        JObject response = JObject.Parse(jsonResponse);

        // candidates[0].content.parts[0].text ?•„?“œë¥? ì¶”ì¶œ
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
