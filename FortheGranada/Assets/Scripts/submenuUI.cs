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

    public void OnClickSaveAndReturnButton()
    {
        GameManager.Instance.is_ingame = false;
        SceneManager.LoadScene("MainMenuScene");
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
