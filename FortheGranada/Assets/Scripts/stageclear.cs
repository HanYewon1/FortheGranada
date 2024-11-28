using UnityEngine;
using UnityEngine.SceneManagement;

public class stageclear : MonoBehaviour
{
    public bool is_inRange;
    public string stagename;
    public belongings blg;

    public SpriteRenderer[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blg = GetComponentInChildren<belongings>();
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        blg.Alpha0();
    }

    // Update is called once per frame
    void Update()
    {
        BoxInteraction();
    }

    void BoxInteraction()
    {
        if (is_inRange)
        {
            if (Input.GetKeyDown(GameManager.Instance.interactKey))
            {
                if (GameManager.Instance.stage == GameManager.Instance.maxstage)
                {
                    GameManager.Instance.is_ingame = false;
                    GameManager.Instance.is_boss = true;
                    GameManager.Instance.speed = GameManager.Instance.speed_for_boss_stage;
                    SceneManager.LoadScene("Stage_Boss");
                    Debug.Log("Enter BossStage!");
                }
                else
                {
                    GameManager.Instance.stage++;
                    stagename = "Stage_" + GameManager.Instance.stage;
                    SceneManager.LoadScene(stagename);
                    Debug.Log("Stage Clear!");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            is_inRange = true;
            sprites[0].gameObject.SetActive(false);
            sprites[1].gameObject.SetActive(true);
            blg.Alpha255();
            Debug.Log("in range");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            is_inRange = false;
            sprites[0].gameObject.SetActive(true);
            sprites[1].gameObject.SetActive(false);
            blg.Alpha0();
            Debug.Log("not in range");
        }
    }
}
