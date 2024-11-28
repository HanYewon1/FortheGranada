using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{

    public float player_speed;//?嚙踝蕭?嚙踝蕭?嚙踝蕭?嚙踝蕭 ?嚙踝蕭?嚙踝蕭?嚙踝蕭?嚙踝蕭
    public Sprite deadSprite;


    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    float player_x;//鮈 ???鴔�
    float player_y;//?嚙踝蕭?嚙踝蕭 ???鴔�

    bool is_horizon_move; //4諻拗 窶域�
    bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    void Update()
    {
        PlayerMove();
        Damaged();
    }

    private void FixedUpdate()
    {
        Vector2 move_vec = is_horizon_move ? new Vector2(player_x, 0) : new Vector2(0, player_y);
        rigidbody2d.linearVelocity = move_vec * GameManager.Instance.speed;
    }

    private void PlayerMove()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //鮈 ?嚙踝蕭?嚙踝蕭
        player_y = Input.GetAxisRaw("Vertical"); //?嚙踝蕭?嚙踝蕭 ?嚙踝蕭?嚙踝蕭

        //?嚙踝蕭嚙�? 貒 麮渣
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //?嚙踝蕭?嚙踝蕭鮈 ?嚙踝蕭?嚙踝蕭?嚙踝蕭 ?嚙踝蕭?嚙踝蕭 魽國探
        if (player_x_down)
        {
            is_horizon_move = true; //鮈 ?嚙踝蕭?嚙踝蕭
        }
        else if (player_y_down)
            is_horizon_move = false; //?嚙踝蕭?嚙踝蕭 ?嚙踝蕭?嚙踝蕭
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //?嚙踝蕭?嚙踝蕭諰?嚙踝蕭
        if (animator.GetInteger("player_move_x") != player_x) //鮈
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y) //?嚙踝蕭?嚙踝蕭
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }
        else //idle
            animator.SetBool("is_change", false);
    }

    //嚙踝蕭嚙豎嫡橘蕭嚙踝蕭 嚙踝蕭
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(ChangeColor());

        if (collision.gameObject.CompareTag("Chest"))
        {
            Transform target = collision.gameObject.GetComponent<Transform>();
            if (target != null)
            {
                GameManager.Instance.currentbox = target.gameObject.GetComponent<itemboxcontroller>();
                Debug.Log("高 渡脾");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("trigger: " + collision.name);
        if (collision.tag == "Door")
        {
            Debug.Log(2);
        }
    }
    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red; //嚙踝蕭嚙踝蕭嚙踝蕭嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭
        yield return new WaitForSeconds(1f); //1嚙褊蛛蕭嚙踝蕭 嚙踝蕭嚙踝蕭
        spriteRenderer.color = originalColor; //嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭嚙踝蕭 嚙踝蕭嚙複選蕭
    }
    //嚙踝蕭嚙豎對蕭嚙踝蕭 嚙踝蕭嚙�
    void Damaged()
    {

    }
    public void Dead()
    {
        if (isDead) return; // 嚙諒對蕭 嚙踝蕭嚙踝蕭 嚙踝蕭嚙蝓塚蕭嚙� 嚙踝蕭嚙踝蕭嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭

        isDead = true; // 嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭
        spriteRenderer.color = Color.gray; // 嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭
        spriteRenderer.sprite = deadSprite; // 嚙踝蕭嚙踝蕭嚙踝蕭嚙踝蕭 嚙踝蕭嚙踝蕭
        animator.enabled = false; // 嚙誰棲賂蕭嚙諒潘蕭 嚙踝蕭嚙踝蕭
        Debug.Log("Game Over");
    }
}
