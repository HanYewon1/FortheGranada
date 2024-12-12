using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class settingsUI : MonoBehaviour
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
        //GameManager.Instance.is_ingame = false;
    }
}
