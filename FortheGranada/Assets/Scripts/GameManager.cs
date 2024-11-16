using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

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
    public bool is_attacked_speed;
    public bool is_preview;
    public bool is_ingame = false;
    public RectTransform[] ui_list;
    public Transform player;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 삭제되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager가 생성되지 않도록 삭제
        }
    }

    private void OnEnable()
    {
        // 씬이 로드될 때 호출
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // 씬 로드 후 필요한 초기화 로직
        InitializeScene(scene);
    }

    private void InitializeScene(Scene scene)
    {
        // 씬별 초기화 로직
        Debug.Log($"Initializing scene: {scene.name}");
        
        //StartCoroutine(WaitOneSecond());
        if (is_ingame = true)
        {
            player = GameObject.Find("Player").GetComponent<Transform>();
            
            ui_list = new RectTransform[3];
            ui_list[0] = GameObject.Find("InGameUI").GetComponent<RectTransform>();
            ui_list[1] = GameObject.Find("MiniGameUI").GetComponent<RectTransform>();
            ui_list[2] = GameObject.Find("PauseMenuUI").GetComponent<RectTransform>();
            
            // 불필요한 ui 비활성
            ui_list[1].gameObject.SetActive(false);
            ui_list[2].gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        is_attacked_speed = false;
        is_preview = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 메뉴가 활성화되어 있으면 비활성화하고, 비활성화되었으면 활성화
            if (ui_list[2] != null)
            {
                ui_list[2].gameObject.SetActive(!ui_list[2].gameObject.activeSelf); // 메뉴의 활성화/비활성화 상태 전환
            }
        }
    }

    private IEnumerator WaitOneSecond()
    {
        // 1초 대기
        yield return new WaitForSeconds(1f);
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
