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

    [Header("Player Settings")]
    public int health;
    public float speed;
    public int maxHealth;
    public int armor;
    public int stealthTime;
    public int key;
    public int ressurection;
    public int health_item;
    public int armor_item;
    public int stealth_item;
    public int key_item;
    public int speed_item;
    public int ressurectiom_item;

    [Header("Game Settings")]
    public int level = 0;
    public int maxtokens = 5;
    public string promptmessage = "3°³ÀÇ ÀÌ¹ÌÁö °øÅëÁ¡À» ³Ê¹« Æ÷°ıÀûÀÌÁö ¾ÊÀº ´Ü¾î·Î ´Ü 1°³¸¸ Ãâ·ÂÇØ! µÚ¿¡ ÀÔ´Ï´Ù ºÙÀÌÁö ¸¶! ÆÇÅ¸Áö, ÇÈ¼¿¾ÆÆ® ±İÁö!";
    public string APIResponse = null;
    private string apiUrl;
    private string apiKey;
    [System.Serializable]
    private class ApiKeyData
    {
        public string apiKey;
    }

    [Header("Flags")]
    public bool is_attacked_speed;
    public bool is_preview;
    public bool is_running;
    public bool is_closebox = false;
    public bool is_minigame = false;
    public bool is_delay = false;
    public bool is_CoroutineRunning = false;
    public bool is_mgset = false;
    public bool is_catch = false;
    public bool is_rannum = true;
    public bool is_rannum2 = true;
    [SerializeField]
    private bool _is_ingame = false;

    [Header("GetComponents")]
    public minigamemanager mg;
    public timer tm;
    public Transform player;
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

        //StartCoroutine(WaitOneSecond());
        if (is_ingame == true)
        {
            mg = GameObject.Find("MinigameManager").GetComponent<minigamemanager>();
            player = GameObject.Find("Player").GetComponent<Transform>();

            ui_list = new RectTransform[6];
            ui_list[0] = GameObject.Find("InGameUI").GetComponent<RectTransform>();
            ui_list[1] = GameObject.Find("MiniGameUI").GetComponent<RectTransform>();
            ui_list[2] = GameObject.Find("PauseMenuUI").GetComponent<RectTransform>();
            ui_list[3] = GameObject.Find("GRayout5X5").GetComponent<RectTransform>();
            ui_list[4] = GameObject.Find("GRayout6X6").GetComponent<RectTransform>();
            ui_list[5] = GameObject.Find("GRayout7X7").GetComponent<RectTransform>();

            tm = ui_list[0].Find("TIME").GetComponent<timer>();

            // ë¶ˆí•„?š”?•œ ui ë¹„í™œ?„±
            ui_list[1].gameObject.SetActive(false);
            ui_list[2].gameObject.SetActive(false);
            ui_list[3].gameObject.SetActive(false);
            ui_list[4].gameObject.SetActive(false);
            ui_list[5].gameObject.SetActive(false);
            Time.timeScale = 1;
            is_minigame = false;

            if (spr_list.Length == 0) spr_list = mg.ImageSet();
            if (ans_list.Length == 0) ans_list = mg.AnswerSet();
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
        if (is_ingame == true)
        {
            if (is_rannum)
            {
                rannum3 = mg.RanNumGen();
                is_rannum = false;
            }

            if (is_rannum2)
            {
                rannum3_2 = mg.RanNumGen();
                is_rannum2 = false;
            }

            if (is_catch)
            {
                foreach (var rannum in rannum3_2)
                {
                    if (APIResponse == ans_list[rannum]) is_rannum2 = true;
                }
            }

            if (is_mgset == false)
            {
                is_mgset = true;
                Debug.Log("¿äÃ» Àü¼Û");
                StartCoroutine(LLMAPIRequest(promptmessage, maxtokens));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ë©”ë‰´ê°? ?™œ?„±?™”?˜?–´ ?ˆ?œ¼ë©? ë¹„í™œ?„±?™”?•˜ê³?, ë¹„í™œ?„±?™”?˜?—ˆ?œ¼ë©? ?™œ?„±?™”
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf); // ë©”ë‰´?˜ ?™œ?„±?™”/ë¹„í™œ?„±?™” ?ƒ?ƒœ ? „?™˜
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                // ·¹º§ ¹× ³­ÀÌµµ º° Ç¥½ÃÇÒ ¸Ê
                switch (level)
                {
                    case 1:
                        // °ª1ÀÏ ¶§ ½ÇÇàÇÒ ÄÚµå
                        if (ui_list[3] != null)
                        {
                            ui_list[3].gameObject.SetActive(!ui_list[3].gameObject.activeSelf);
                        }
                        break;
                    case 2:
                        // °ª2ÀÏ ¶§ ½ÇÇàÇÒ ÄÚµå
                        if (ui_list[4] != null)
                        {
                            ui_list[4].gameObject.SetActive(!ui_list[4].gameObject.activeSelf);
                        }
                        break;
                    case 3:
                        // °ª3ÀÏ ¶§ ½ÇÇàÇÒ ÄÚµå
                        if (ui_list[5] != null)
                        {
                            ui_list[5].gameObject.SetActive(!ui_list[5].gameObject.activeSelf);
                        }
                        break;
                    default:
                        // ¸ğµç case¿¡ ÇØ´çÇÏÁö ¾ÊÀ» ¶§ ½ÇÇàÇÒ ÄÚµå
                        break;
                }
            }

            if (is_closebox == true && is_minigame == false && is_delay == false && is_mgset == true && is_catch == true)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    is_running = false;
                    ui_list[1].gameObject.SetActive(true);
                }
            }
            if (is_delay && is_CoroutineRunning == false)
            {
                StartCoroutine(SelectedIncurrect());
            }
        }
    }

    private IEnumerator LLMAPIRequest(string prompt, int maxTokens)
    {
        // ?´ë¯¸ì?? ?ŒŒ?¼ ?´ë¦? ë°°ì—´ (Resources ?´?” ?‚´?˜ ?´ë¯¸ì?? ?´ë¦?)
        string[] imageNames = new string[3];
        for (int i = 0; i < 3; i++)
        {
            string num;
            if (rannum3[i] == 100)
            {
                num = rannum3[i].ToString();
            }
            else if (rannum3[i] >= 10)
            {
                num = "0" + rannum3[i].ToString();
            }
            else
            {
                num = "00" + rannum3[i].ToString();
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
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[0] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[1] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[2] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

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
            mg.FailRequest();
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
            mg.FailRequest();
            is_catch = true;
        }
    }

    private IEnumerator WaitOneSecond()
    {
        // 1ì´? ???ê¸?
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SelectedIncurrect()
    {
        is_CoroutineRunning = true;
        // ¹Ì´Ï°ÔÀÓ ¿À´ä ÆĞ³ÎÆ¼
        yield return new WaitForSeconds(5f);
        is_delay = false;
        is_CoroutineRunning = false;
        Debug.Log("ÆĞ³ÎÆ¼ ÇØÁ¦");
    }

    public void getItem(Item item)
    {
        if (item.GetItemType == ItemType.Expendables)
        {
            if (item.GetItemID == 1)//Ã¼ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                if (health_item < item.GetNumNesting)
                {
                    maxHealth++;
                    health++;
                }
                else if (maxHealth > health)//ï¿½Ö´ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ê°ï¿½ï¿½ï¿½ È¸ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Û°ï¿½ ï¿½ï¿½ï¿½ï¿½ È¿ï¿½ï¿½
                {
                    health++;
                }
            }
            else if (item.GetItemID == 2)//È¸ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                if (maxHealth > health)
                {
                    health++;
                }
            }
        }
        else if (item.GetItemType == ItemType.Passive)
        {
            if (item.GetItemID == 4 && speed_item < item.GetNumNesting)//ï¿½Óµï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                speed += 0.1f;
            }
            else if (item.GetItemID == 6)//ï¿½Ç°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                is_attacked_speed = true;
            }
            else if (item.GetItemID == 7 && stealthTime < item.GetNumNesting)//ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                stealthTime++;
            }
            else if (item.GetItemID == 8)//ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                is_preview = true;
            }
        }
        else if (item.GetItemType == ItemType.Temporary)
        {
            if (item.GetItemID == 3)//ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                armor++;
            }
        }
        else if (item.GetItemType == ItemType.Resurrection)
        {
            if (item.GetItemID == 5 && ressurectiom_item < item.GetNumNesting)//ï¿½ï¿½È° ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            {
                ressurection++;
            }
        }
        else if (item.GetItemType == ItemType.Key)
        {
            if (item.GetItemID == 9)//ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            {
                key++;
            }
        }
    }
}
