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
    public string promptmessage = "3���� �̹��� �������� �ʹ� ���������� ���� �ܾ�� �� 1���� �����! �ڿ� �Դϴ� ������ ��! ��Ÿ��, �ȼ���Ʈ ����!";
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

            // 불필?��?�� ui 비활?��
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
                Debug.Log("��û ����");
                StartCoroutine(LLMAPIRequest(promptmessage, maxtokens));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 메뉴�? ?��?��?��?��?�� ?��?���? 비활?��?��?���?, 비활?��?��?��?��?���? ?��?��?��
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf); // 메뉴?�� ?��?��?��/비활?��?�� ?��?�� ?��?��
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                // ���� �� ���̵� �� ǥ���� ��
                switch (level)
                {
                    case 1:
                        // ��1�� �� ������ �ڵ�
                        if (ui_list[3] != null)
                        {
                            ui_list[3].gameObject.SetActive(!ui_list[3].gameObject.activeSelf);
                        }
                        break;
                    case 2:
                        // ��2�� �� ������ �ڵ�
                        if (ui_list[4] != null)
                        {
                            ui_list[4].gameObject.SetActive(!ui_list[4].gameObject.activeSelf);
                        }
                        break;
                    case 3:
                        // ��3�� �� ������ �ڵ�
                        if (ui_list[5] != null)
                        {
                            ui_list[5].gameObject.SetActive(!ui_list[5].gameObject.activeSelf);
                        }
                        break;
                    default:
                        // ��� case�� �ش����� ���� �� ������ �ڵ�
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
        // ?��미�?? ?��?�� ?���? 배열 (Resources ?��?�� ?��?�� ?��미�?? ?���?)
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
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[0] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[1] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[2] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

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
            mg.FailRequest();
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
            mg.FailRequest();
            is_catch = true;
        }
    }

    private IEnumerator WaitOneSecond()
    {
        // 1�? ???�?
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SelectedIncurrect()
    {
        is_CoroutineRunning = true;
        // �̴ϰ��� ���� �г�Ƽ
        yield return new WaitForSeconds(5f);
        is_delay = false;
        is_CoroutineRunning = false;
        Debug.Log("�г�Ƽ ����");
    }

    public void getItem(Item item)
    {
        if (item.GetItemType == ItemType.Expendables)
        {
            if (item.GetItemID == 1)//ü�� ������
            {
                if (health_item < item.GetNumNesting)
                {
                    maxHealth++;
                    health++;
                }
                else if (maxHealth > health)//�ִ� ���� �ʰ��� ȸ�� �����۰� ���� ȿ��
                {
                    health++;
                }
            }
            else if (item.GetItemID == 2)//ȸ�� ������
            {
                if (maxHealth > health)
                {
                    health++;
                }
            }
        }
        else if (item.GetItemType == ItemType.Passive)
        {
            if (item.GetItemID == 4 && speed_item < item.GetNumNesting)//�ӵ� ������
            {
                speed += 0.1f;
            }
            else if (item.GetItemID == 6)//�ǰ� ������
            {
                is_attacked_speed = true;
            }
            else if (item.GetItemID == 7 && stealthTime < item.GetNumNesting)//���� ������
            {
                stealthTime++;
            }
            else if (item.GetItemID == 8)//���� ������
            {
                is_preview = true;
            }
        }
        else if (item.GetItemType == ItemType.Temporary)
        {
            if (item.GetItemID == 3)//���� ������
            {
                armor++;
            }
        }
        else if (item.GetItemType == ItemType.Resurrection)
        {
            if (item.GetItemID == 5 && ressurectiom_item < item.GetNumNesting)//��Ȱ ������
            {
                ressurection++;
            }
        }
        else if (item.GetItemType == ItemType.Key)
        {
            if (item.GetItemID == 9)//���� ����
            {
                key++;
            }
        }
    }
}
