using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            player = GameObject.Find("Player").GetComponent<Transform>();

            ui_list = new RectTransform[6];
            ui_list[0] = GameObject.Find("InGameUI").GetComponent<RectTransform>();
            ui_list[1] = GameObject.Find("MiniGameUI").GetComponent<RectTransform>();
            ui_list[2] = GameObject.Find("PauseMenuUI").GetComponent<RectTransform>();
            ui_list[3] = GameObject.Find("GRayout5X5").GetComponent<RectTransform>();
            ui_list[4] = GameObject.Find("GRayout6X6").GetComponent<RectTransform>();
            ui_list[5] = GameObject.Find("GRayout7X7").GetComponent<RectTransform>();

            // 불필?��?�� ui 비활?��
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
