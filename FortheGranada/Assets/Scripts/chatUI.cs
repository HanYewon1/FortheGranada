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
        chats[0] = "0";
        chats[1] = "1";
        chats[2] = "2";
        chats[3] = "3";
        chats[4] = "4";
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
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            idx++;
        }
    }

    public void OnClickNextPageButton()
    {
        idx++;
    }
}
