using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenuUI : MonoBehaviour
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
        audiomanager.Instance.mainmenubgm.Play();
        audiomanager.Instance.mainmenubgm.loop = true;
    }

    public void OnClickLevelSelectButton()
    {
        audiomanager.Instance.menusfx.Play();
        if (GameManager.Instance.ui_list[1] != null) GameManager.Instance.ui_list[1].gameObject.SetActive(true);
    }

    public void OnClickQuitButton()
    {
        audiomanager.Instance.menusfx.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
