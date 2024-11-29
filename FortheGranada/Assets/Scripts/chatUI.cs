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
        chats[0] = "���� ����\r\n��� ��ȣ �ۿ��� FŰ�� ���� �̷�����ϴ�.";
        chats[1] = "���ڿ� ��ȣ �ۿ� �� �̴ϰ����� �����ϸ�\r\n������ ���� �� ���� ������ �Ǵ� ���� ������ ���� �� �ֽ��ϴ�.";
        chats[2] = "�̴ϰ��ӿ��� ������ ���� �� �г�Ƽ��\r\n5�� �� ���� �̿��� �����Ǹ鼭 �˹�˴ϴ�.";
        chats[3] = "�̴ϸ��� MŰ, �޴�â�� ESCŰ�� ���� Ű�� �� �� �ֽ��ϴ�.";
        chats[4] = "�ʿ��� ���� ������ �� ������ ū ���ڿ��� ����ǰ�� ���\r\n���� ���������� ������ ���������ϴ�.";
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
