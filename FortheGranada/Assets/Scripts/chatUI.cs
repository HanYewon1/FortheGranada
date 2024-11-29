using UnityEngine;
using UnityEngine.UI;

public class chatUI : MonoBehaviour
{
    public string[] chats;
    public int idx = 0;
    public Text txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        txt = GameObject.Find("chat").GetComponent<Text>();
        chats = new string[5];
        chats[0] = "게임 설명\r\n모든 상호 작용은 F키를 통해 이루어집니다.";
        chats[1] = "상자와 상호 작용 시 미니게임을 진행하며\r\n정답을 맞출 시 랜덤 아이템 또는 열쇠 조각을 얻을 수 있습니다.";
        chats[2] = "미니게임에서 오답을 선택 시 패널티로\r\n5초 간 상자 이용이 금지되면서 넉백됩니다.";
        chats[3] = "미니맵은 M키, 메뉴창은 ESC키를 통해 키고 끌 수 있습니다.";
        chats[4] = "필요한 열쇠 조각을 다 모으면 큰 상자에서 소지품을 얻고\r\n다음 스테이지로 진입이 가능해집니다.";
    }

    // Update is called once per frame
    void Update()
    {
        if (chats.Length > idx)
        {
            txt.text = chats[idx];
        }
        else
        {
            idx = 0;
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            idx++;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            idx++;
        }
    }

    public void OnClickNextPageButton()
    {
        idx++;
    }
}
