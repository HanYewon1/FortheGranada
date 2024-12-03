using UnityEngine;
using System.Collections;

public class bossblock : MonoBehaviour
{
    public GameObject ITEM;
    public bool ishp = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ITEM = transform.Find("ITEM").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            StartCoroutine(BossDamage());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.maxHealth > GameManager.Instance.health) GameManager.Instance.health++;
            ishp = false;
            ITEM.SetActive(false);
        }
    }

    private IEnumerator BossDamage()
    {
        GameManager.Instance.boss_health -= 5;
        int hp = Random.Range(1, 101);
        if (!ishp && hp > 75)
        {
            ishp = true;
            ITEM.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
    }
}
