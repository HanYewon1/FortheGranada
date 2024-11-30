using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class submenuUI : MonoBehaviour
{
    void Awake()
    {
        this.enabled = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
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

    public void OnClickReturnButton()
    {
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_running = false;
        GameManager.Instance.is_boss = false;
        SceneManager.LoadScene("MainMenuScene");
    }

    /*public void OnClickSaveButton()
    {
        Debug.Log("����Ǿ����ϴ�!");
    }*/

    public void OnClickInfoButton()
    {
        if (GameManager.Instance.ui_list[6] != null)
        {
            GameManager.Instance.ui_list[6].gameObject.SetActive(true);
        }
    }

    public void OnClickCloseButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
