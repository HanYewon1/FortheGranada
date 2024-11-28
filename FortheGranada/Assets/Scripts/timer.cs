using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour
{
    public TMP_Text timerText; // UI Text 컴포넌트에 연결
    public float totalTime = 900f; // 총 타이머 시간 (900초)
    private float timeLeft; // 남은 시간
    //private bool is_Running = true; // 타이머 상태 (동작 중인지 여부)

    public void Awake()
    {
        timerText = this.transform.GetComponentInChildren<TMP_Text>();
        totalTime = 900f;
        timeLeft = totalTime; // 초기화
    }

    void Start()
    {
        //timeLeft = totalTime; // 초기화
        //UpdateTimerText();
    }

    public void OnEnable()
    {
        //timeLeft = totalTime; // 초기화
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //timeLeft = totalTime; // 초기화
    }

    void Update()
    {
        if (GameManager.Instance.is_running && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                GameManager.Instance.is_running = false;
                Debug.Log("시간 초과!");
                GameManager.Instance.ui_list[7].gameObject.SetActive(true);
                //Time.timeScale = 0;
                GameManager.Instance.speed = 0;
                StartCoroutine(GameManager.Instance.WaitThreeSecond());
            }
        }
    }

    // 타이머 멈추기
    public void PauseTimer()
    {
        GameManager.Instance.is_running = false;
    }

    // 타이머 다시 시작하기
    public void ResumeTimer()
    {
        GameManager.Instance.is_running = true;
    }

    // 남은 시간을 텍스트에 업데이트
    private void UpdateTimerText()
    {
        int m, s, t;
        string timestring = null;

        t = (int)Mathf.Ceil(timeLeft);
        m = t / 60;
        s = t % 60;

        if (s >= 10)
        {
            timestring = m.ToString() + " : " + s.ToString();
        }
        else
        {
            timestring = m.ToString() + " : 0" + s.ToString();
        }

        timerText.text = timestring;
    }
}
