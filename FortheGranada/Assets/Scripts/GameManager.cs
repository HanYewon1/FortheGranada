using TMPro;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
    public int health = 1;
    public float speed = 0;
    public float originspeed = 0;
    public float speed_for_boss_stage = 0;
    public int maxHealth = 10;
    public int armor = 0;
    public int stealthTime;
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
    public int maxtokens = 0;
    public string promptmessage = null;
    public string APIResponse = null;
    private string apiUrl = null;
    private string apiKey;
    [System.Serializable]
    private class ApiKeyData
    {
        public string apiKey;
    }

    [Header("Flags")]
    public bool is_ressurection;
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
    [SerializeField]
    private bool _is_boss = false;

    [Header("GetComponents")]
    private GameObject tmp;
    public itemboxcontroller currentbox;
    public minigamemanager mg;
    public itemmanager im;
    public timer tm;
    public scanner sc;
    public playercontroller pc;
    public bosscontroller boscon;
    public TMP_Text hint_count;
    public Image speedcount;
    public Slider healthSlider;
    public Transform player;
    public inneritem[] innerItems;
    public RectTransform[] health_list;
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
            Debug.Log($"is_ingame �� �����: {_is_ingame}");
        }
    }
    public bool is_boss
    {
        get => _is_boss;
        set
        {
            _is_boss = value;
            Debug.Log($"is_boss �� �����: {_is_boss}");
        }
    }

    void Awake()
    {
        // Instance ���� ������ ���� ���� �Ŵ��� �ı� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ������ ���� ���ϸ� �̰ɷ� ��ü�ϰ� �ı����� �ʱ�
        }
        else
        {
            Destroy(gameObject); // ������ �����ϸ� �ڽ��ı�
        }

        string path = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            apiKey = JsonUtility.FromJson<ApiKeyData>(json).apiKey;
        }

        apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + apiKey;

        interactKey = KeyCode.F;
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
        // ���� �ʱ�ȭ�Ǹ� �α� ���
        Debug.Log($"Initializing scene: {scene.name}");

        // �� ������ �ʱ�ȭ
        maxstage = 3;
        speed = 3;
        originspeed = 3;
        speed_for_boss_stage = 1.5f;

        // Ingame ���� �ʱ�ȭ �۾� ����
        if (is_ingame == true)
        {
            // API ���� ������ �ʱ�ȭ
            maxtokens = 6;
            promptmessage = "3���� �̹��� �������� �ʹ� ���������� ���� �ܾ�� �� 1���� �����! �ڿ� �Դϴ� ������ ��! ��Ÿ��, �ȼ���Ʈ ����!";

            // ���̵� ���ÿ� ���� ���� ������ ����
            switch (diff)
            {
                case 1:
                    health = 5;
                    maxHealth = 5;
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
                    maxHealth = 3;
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
                    maxHealth = 1;
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

            // �ʿ��� ������Ʈ�� ��������
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
            tmp = GameObject.Find("Player");
            if (tmp != null) player = tmp.GetComponent<Transform>();
            if (player != null) pc = player.GetComponent<playercontroller>();
            // InnerItem ��ũ��Ʈ�� ���� ��� ������Ʈ ã��
            innerItems = FindObjectsOfType<inneritem>(true);

            // ui_list�� �ʿ��� UI�� �̸� ��������
            ui_list = new RectTransform[8];
            tmp = GameObject.Find("InGameUI");
            if (tmp != null) ui_list[0] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("MiniGameUI");
            if (tmp != null) ui_list[1] = tmp.GetComponent<RectTransform>();
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

            // ü�°� ������ UI �ڽĵ� ��������
            tmp = GameObject.Find("HPUI");
            health_list = tmp.GetComponentsInChildren<RectTransform>();
            tmp = GameObject.Find("ITEMUI");
            item_list = tmp.GetComponentsInChildren<RectTransform>();

            //���ǵ� ī��Ʈ ������
            if (item_list != null) speedcount = item_list[3].GetComponent<Image>();

            // Find�� ã������ UI List�� �ٽ� ��Ȱ��ȭ
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
            if (item_list != null) item_list[4].gameObject.SetActive(false);
            if (item_list != null) item_list[5].gameObject.SetActive(false);
            if (item_list != null) item_list[6].gameObject.SetActive(false);
            if (item_list != null) item_list[7].gameObject.SetActive(false);

            // �ð� ����ȭ, �̴ϰ��� OFF
            Time.timeScale = 1;
            is_minigame = false;

            // �̴ϰ��ӿ� �̹����� ���� ����Ʈ ��������
            if (spr_list.Length == 0) spr_list = mg.ImageSet();
            if (ans_list.Length == 0) ans_list = mg.AnswerSet();

            // ��� ���ڿ� Ű�� ������ �Ҵ�
            if (innerItems.Length >= req_key) SetItems();
        }

        if (is_boss)
        {
            tmp = GameObject.Find("Player");
            if (tmp != null) player = tmp.GetComponent<Transform>();
            if (player != null) pc = player.GetComponent<playercontroller>();
            tmp = GameObject.Find("Slider");
            if (tmp != null) healthSlider = tmp.GetComponent<Slider>();
            tmp = GameObject.Find("BOSS");
            if (tmp != null) boscon = tmp.GetComponent<bosscontroller>();
            ui_list = new RectTransform[8];
            tmp = GameObject.Find("PauseMenuUI");
            if (tmp != null) ui_list[2] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("ChatUI");
            if (tmp != null) ui_list[6] = tmp.GetComponent<RectTransform>();
            tmp = GameObject.Find("OverUI");
            if (tmp != null) ui_list[7] = tmp.GetComponent<RectTransform>();
            if (ui_list != null) ui_list[2].gameObject.SetActive(false);
            if (ui_list != null) ui_list[6].gameObject.SetActive(false);
            if (ui_list != null) ui_list[7].gameObject.SetActive(false);
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
            }

            if (is_catch && ans_list != null && ans_list.Length != 0)
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
                // ESC �޴� ���ݱ�
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf);
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                // ���� �� ���̵� �� ǥ���� ��
                switch (diff)
                {
                    case 1:
                        // ��1�� �� ������ �ڵ�
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
                        // ��2�� �� ������ �ڵ�
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
                        // ��3�� �� ������ �ڵ�
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
            }

            if (is_closebox == true && is_minigame == false && is_delay == false && is_mgset == true && is_catch == true && !currentbox.isOpen && currentbox.ii.is_set)
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
                StartCoroutine(WaitFiveSecond());
            }

            if (hint_count != null) hint_count.text = key + " / " + req_key;
            if (health_list != null && health_list.Length != 0) updatehealth();
            if (item_list != null && item_list.Length != 0) updateshoe();
        }

        if (is_boss)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ESC �޴� ���ݱ�
                if (ui_list[2] != null)
                {
                    ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
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
            speed = originspeed;
            SceneManager.LoadScene("Test");
        }

        if (Input.GetKeyDown(KeyCode.Y))
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
            is_running = true;
            speed = speed_for_boss_stage;
            SceneManager.LoadScene("Stage_Boss");
        }

        if (Input.GetKeyDown(KeyCode.U))
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
            speed = originspeed;
            SceneManager.LoadScene("PlayScene");
        }
    }

    private IEnumerator LLMAPIRequest(string prompt, int maxTokens)
    {
        // ������ �̹��� 3�� �迭�� ���
        string[] imageNames = new string[3];
        if (rannum3 != null && rannum3.Length != 0)
        {
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
        }

        List<string> base64Images = new List<string>();

        if (imageNames.Length != 0)
        {
            foreach (string imageName in imageNames)
            {
                // Resources���� �̹��� ��������
                Texture2D image = Resources.Load<Texture2D>(imageName);

                if (image == null)
                {
                    Debug.LogError($"Image '{imageName}' not found in Resources folder.");
                    yield break; // �α׿��� ���� �ڷ�ƾ ������
                }

                Texture2D uncompressedImage = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
                uncompressedImage.SetPixels(image.GetPixels());
                uncompressedImage.Apply();

                // �̹����� �ζ��� �����ͷ� ������ ���� ����Ʈ�� �迭�� ����
                byte[] imageBytes = image.EncodeToJPG();
                string base64Image = Convert.ToBase64String(imageBytes); // Base64�? ?��코딩
                base64Images.Add(base64Image);
            }
        }

        // POST�� ������ ���� JSON ���� �����ͷ� ����
        string jsonData = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[0] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[1] + "\"}},{\"inlineData\": {\"mimeType\": \"image/png\",\"data\": \"" + base64Images[2] + "\"}}]}], \"generationConfig\": {\"maxOutputTokens\": " + maxTokens + "}}";

        // UnityWebRequest ������ ���� �ʿ��� �� ��
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Header �ۼ�
        request.SetRequestHeader("Content-Type", "application/json");

        // ������Ʈ ����
        yield return request.SendWebRequest();

        // �����ϸ� ����ް� �ؽ�Ʈ �Ľ�
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
        // JSON �Ľ�
        JObject response = JObject.Parse(jsonResponse);

        // candidates[0].content.parts[0].text �Ľ�
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

    private IEnumerator WaitFiveSecond()
    {
        // 5�� ��ٸ��� ��������� ������ ����
        yield return new WaitForSeconds(5f);
        if (!is_catch)
        {
            Debug.Log("���� �ʹ� ����");
            mg.FailRequest();
            is_catch = true;
        }
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

    public int SelectItem(int rannum1)
    {
        int itemnum = 10;

        if (rannum1 >= 1 && rannum1 <= 50)
        {
            itemnum = 1;
            //Debug.Log("ü�� ȸ��");
        }
        else if (rannum1 >= 51 && rannum1 <= 69)
        {
            itemnum = 2;
            //Debug.Log("���� ȹ��");
        }
        else if (rannum1 >= 70 && rannum1 <= 79)
        {
            itemnum = 3;
            //Debug.Log("�̼� ����");
        }
        else if (rannum1 >= 80 && rannum1 <= 84)
        {
            itemnum = 0;
            //Debug.Log("�ִ� ü�� ����");
        }
        else if (rannum1 >= 85 && rannum1 <= 89)
        {
            itemnum = 5;
            //Debug.Log("�ǰ� �� �̼� ����");
        }
        else if (rannum1 >= 90 && rannum1 <= 94)
        {
            itemnum = 6;
            //Debug.Log("���� �ð� ����");
        }
        else if (rannum1 >= 95 && rannum1 <= 99)
        {
            itemnum = 7;
            //Debug.Log("���� ����");
        }
        else if (rannum1 == 100)
        {
            itemnum = 4;
            //Debug.Log("��Ȱ �� ȹ��!");
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

    // ���� ü�� ���� ��ȯ
    public float GetNormalizedHealth()
    {
        return boss_health / boss_max_health;
    }

    public void GameOver()
    {
        if (is_ressurection)
        {
            health = 1;
            ressurection_item--;
            is_ressurection = false;
            item_list[4].gameObject.SetActive(false);
        }
        else if (is_running)
        {
            if (pc != null) pc.Dead();
            is_running = false;
            Debug.Log("ĳ���� ���!");
            if (ui_list != null && ui_list.Length != 0) ui_list[7].gameObject.SetActive(true);
            //Time.timeScale = 0;
            speed = 0;
            StartCoroutine(WaitThreeSecond());
        }
    }

    public IEnumerator WaitThreeSecond()
    {
        // 5�� ��ٸ��� Ÿ��Ʋ ȭ������ ��
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

    public void SetItems()
    {
        int[] rankey = mg.RanNumGenWithNum(req_key, innerItems.Length);
        foreach (int i in rankey)
        {
            innerItems[i].itemnumber = 8;
            innerItems[i].is_set = true;
        }
        for (int j = 0; j < innerItems.Length; j++)
        {
            if (!innerItems[j].is_set)
            {
                innerItems[j].itemnumber = SelectItem(UnityEngine.Random.Range(1, 101));
                innerItems[j].is_set = true;
            }
        }
    }
}
