using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Flags")]
    public bool is_attacked_speed;
    public bool is_preview;
    public bool is_closebox = false;
    public bool is_minigame = false;
    public bool is_delay = false;
    public bool is_CoroutineRunning = false;
    [SerializeField]
    private bool _is_ingame = false;
    
    [Header("GetComponents")]
    public Transform player;
    public RectTransform[] ui_list;

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
            player = GameObject.Find("Player").GetComponent<Transform>();

            ui_list = new RectTransform[6];
            ui_list[0] = GameObject.Find("InGameUI").GetComponent<RectTransform>();
            ui_list[1] = GameObject.Find("MiniGameUI").GetComponent<RectTransform>();
            ui_list[2] = GameObject.Find("PauseMenuUI").GetComponent<RectTransform>();
            ui_list[3] = GameObject.Find("GRayout5X5").GetComponent<RectTransform>();
            ui_list[4] = GameObject.Find("GRayout6X6").GetComponent<RectTransform>();
            ui_list[5] = GameObject.Find("GRayout7X7").GetComponent<RectTransform>();

            // ë¶ˆí•„?š”?•œ ui ë¹„í™œ?„±
            ui_list[1].gameObject.SetActive(false);
            ui_list[2].gameObject.SetActive(false);
            ui_list[3].gameObject.SetActive(false);
            ui_list[4].gameObject.SetActive(false);
            ui_list[5].gameObject.SetActive(false);
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

            if (is_closebox == true && is_minigame == false && is_delay == false)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    ui_list[1].gameObject.SetActive(true);
                }
            }
            if(is_delay && is_CoroutineRunning == false)
            {
                StartCoroutine(SelectedIncurrect());
            }
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
