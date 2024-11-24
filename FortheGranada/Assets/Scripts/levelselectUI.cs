using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelselectUI : MonoBehaviour
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
        GameManager.Instance.is_running = true;
        GameManager.Instance.level = 1;
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickNormalButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        GameManager.Instance.is_running = true;
        GameManager.Instance.level = 2;
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickChallengeButton()
    {
        if (!GameManager.Instance.is_ingame)
        {
            GameManager.Instance.is_ingame = true;
        }
        GameManager.Instance.is_running = true;
        GameManager.Instance.level = 3;
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickMapTest()
    {

        SceneManager.LoadScene("SampleScene");
    }
}
