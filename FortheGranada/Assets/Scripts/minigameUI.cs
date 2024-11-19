using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class minigameUI : MonoBehaviour
{
    public Image[] img_list;
    public Text[] txt_list;
    public Button[] btn_list;
    public int LLM;

    void Awake()
    {
        this.enabled = true;
        img_list = GetComponentsInChildren<Image>();
        txt_list = GetComponentsInChildren<Text>();
        btn_list = GetComponentsInChildren<Button>();
    }

    public void UpdateMinigame()
    {
        if (GameManager.Instance.is_mgset == true && GameManager.Instance.APIRequest != null)
        {
            img_list[2].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum3[0]];
            img_list[3].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum3[1]];
            img_list[4].sprite = GameManager.Instance.spr_list[GameManager.Instance.rannum3[2]];
        }

        if (GameManager.Instance.is_mgset == true && GameManager.Instance.APIRequest != null)
        {
            for (int k = 1; k < 5; k++)
            {
                txt_list[k].text = "NULL";
            }

            LLM = Random.Range(1, 5);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    if (txt_list[j].text != "NULL")
                    {
                        Debug.Log("!NULL");
                    }
                    else if (j == LLM)
                    {
                        txt_list[j].text = GameManager.Instance.APIRequest;
                    }
                    else
                    {
                        txt_list[j].text = GameManager.Instance.ans_list[GameManager.Instance.rannum3[i]];
                        break;
                    }
                }
            }
            if (txt_list[4].text == "NULL" && LLM == 4) txt_list[4].text = GameManager.Instance.APIRequest;

            for (int i = 1; i < 5; i++)
            {
                if (i == LLM)
                {
                    btn_list[i].onClick.AddListener(OnClickCorrectButton);
                }
                else
                {
                    btn_list[i].onClick.AddListener(OnClickIncorrectButton);
                }
            }
        }
    }

    private void OnEnable()
    {
        // ?”¬?´ ë¡œë“œ?  ?•Œ ?˜¸ì¶?
        //SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateMinigame();
    }

    private void OnDisable()
    {
        // ?”¬ ë¡œë“œ ?´ë²¤íŠ¸ ?•´? œ
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this.gameObject.SetActive(true);
    }

    public void OnClickRandomButton()
    {
        btn_list[1].onClick.RemoveAllListeners();
        btn_list[2].onClick.RemoveAllListeners();
        btn_list[3].onClick.RemoveAllListeners();
        btn_list[4].onClick.RemoveAllListeners();

        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }
        GameManager.Instance.is_delay = true;
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

    public void OnClickCorrectButton()
    {
        btn_list[1].onClick.RemoveAllListeners();
        btn_list[2].onClick.RemoveAllListeners();
        btn_list[3].onClick.RemoveAllListeners();
        btn_list[4].onClick.RemoveAllListeners();

        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }

        if (GameManager.Instance.is_mgset)
        {
            GameManager.Instance.is_mgset = false;
        }

        GameManager.Instance.is_catch = false;
        GameManager.Instance.is_rannum = true;
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

    public void OnClickIncorrectButton()
    {
        btn_list[1].onClick.RemoveAllListeners();
        btn_list[2].onClick.RemoveAllListeners();
        btn_list[3].onClick.RemoveAllListeners();
        btn_list[4].onClick.RemoveAllListeners();

        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }

        if (GameManager.Instance.is_mgset)
        {
            GameManager.Instance.is_mgset = false;
        }

        GameManager.Instance.is_catch = false;
        GameManager.Instance.is_rannum = true;
        GameManager.Instance.is_delay = true;
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

}
