using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class minigameUI : MonoBehaviour
{

    void Awake()
    {
        this.enabled = true;
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
        //this.gameObject.SetActive(true);
    }
    
    public void OnClickRandomButton()
    {
        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }
        GameManager.Instance.is_delay = true;
        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

    public void OnClickCorrectButton()
    {
        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }

        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

    public void OnClickIncorrectButton()
    {
        if (GameManager.Instance.is_minigame)
        {
            GameManager.Instance.is_minigame = false;
        }

        GameManager.Instance.ui_list[1].gameObject.SetActive(false);
    }

}
