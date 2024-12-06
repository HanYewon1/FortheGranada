using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class diffselectUI : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }

    public void OnClickEasyButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        GameManager.Instance.is_running = true;
        GameManager.Instance.diff = 1;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.Instance.originspeed;
        GameManager.Instance.health = 5;
        Time.timeScale = 1;
        //SceneManager.LoadScene("PlayScene");
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickNormalButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        GameManager.Instance.is_running = true;
        GameManager.Instance.diff = 2;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.Instance.originspeed;
        GameManager.Instance.health = 3;
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickChallengeButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        if (GameManager.Instance.is_boss)
        {
            GameManager.Instance.is_boss = false;
        }
        GameManager.Instance.is_running = true;
        GameManager.Instance.diff = 3;
        GameManager.Instance.stage = 1;
        GameManager.Instance.speed = GameManager.Instance.originspeed;
        GameManager.Instance.health = 1;
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickMapTest()
    {

        SceneManager.LoadScene("SampleScene");
    }
}
