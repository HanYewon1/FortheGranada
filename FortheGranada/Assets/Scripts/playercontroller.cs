using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{

    public float player_speed;//??š™è¸è•­??š™è¸è•­??š™è¸è•­??š™è¸è•­ ??š™è¸è•­??š™è¸è•­??š™è¸è•­??š™è¸è•­
    public Sprite deadSprite;


    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    Vector3 add_door_position;

    float player_x;//é®ˆï˜îª? ???é´”î½‚ï¿?
    float player_y;//??š™è¸è•­??š™è¸è•­ ???é´”î½‚ï¿?


    bool is_horizon_move; //4è«»æ‹—? ¼ çª¶åŸŸï¿?
    bool isDead = false;
    bool is_door;
    //bool is_horizon_move; //4ë°©í–¥ ê²°ì •

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(1);
            useDoor();
        }

    }

    private void FixedUpdate()
    {
        Vector2 move_vec = is_horizon_move ? new Vector2(player_x, 0) : new Vector2(0, player_y);
        rigidbody2d.linearVelocity = move_vec * GameManager.Instance.speed;
    }

    private void PlayerMove()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //é®ˆï˜îª? ??š™è¸è•­??š™è¸è•­
        player_y = Input.GetAxisRaw("Vertical"); //??š™è¸è•­??š™è¸è•­ ??š™è¸è•­??š™è¸è•­

        //??š™è¸è•­?š™ï¿?? è²’ï„š?’— éº?æ¸£î¼‚
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //??š™è¸è•­??š™è¸è•­é®ˆï˜îª? ??š™è¸è•­??š™è¸è•­??š™è¸è•­ ??š™è¸è•­??š™è¸è•­ é­½åœ‹?¢
        if (player_x_down)
        {
            is_horizon_move = true; //é®ˆï˜îª? ??š™è¸è•­??š™è¸è•­
        }
        else if (player_y_down)
            is_horizon_move = false; //??š™è¸è•­??š™è¸è•­ ??š™è¸è•­??š™è¸è•­
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //??š™è¸è•­??š™è¸è•­è«°î‰î²???š™è¸è•­
        if (animator.GetInteger("player_move_x") != player_x) //é®ˆï˜îª?
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y) //??š™è¸è•­??š™è¸è•­
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }
        else //idle
            animator.SetBool("is_change", false);
    }

    //?š™è¸è•­?š™è±å«¡æ©˜è•­?š™è¸è•­ ?š™è¸è•­
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
                Debug.Log("Near Box");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            is_door = true;
            Debug.Log("door: " + is_door);
            if (collision.name == "door_up")
            {
                add_door_position = new Vector3(0, 7, 0);
            }
            else if (collision.name == "door_down")
            {
                add_door_position = new Vector3(0, -7, 0);
            }
            else if (collision.name == "door_right")
            {
                add_door_position = new Vector3(7, 0, 0);
            }
            else if (collision.name == "door_left")
            {
                add_door_position = new Vector3(-7, 0, 0);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            Debug.Log("EXit");
            is_door = false;
        }
    }
    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red; //?š™è¸è•­?š™è¸è•­?š™è¸è•­?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­
        yield return new WaitForSeconds(1f); //1?š™è¤Šè››?•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­
        spriteRenderer.color = originalColor; //?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¤‡é¸?•­
    }
    //?š™è¸è•­?š™è±å°?•­?š™è¸è•­ ?š™è¸è•­?š™ï¿?
    void Damaged()
    {

    }
    public void Dead()
    {
        if (isDead) return; // ?š™è«’å°?•­ ?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™?“å¡šè•­?š™ï¿? ?š™è¸è•­?š™è¸è•­?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­

        isDead = true; // ?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­
        spriteRenderer.color = Color.gray; // ?š™è¸è•­?š™è¸è•­ ?š™è¸è•­?š™è¸è•­
        spriteRenderer.sprite = deadSprite; // ?š™è¸è•­?š™è¸è•­?š™è¸è•­?š™è¸è•­?š¾ ?š™è¸è•­?š™è¸è•­
        animator.enabled = false; // ?š™èª°æ£²è³‚è•­?š™è«’æ½˜?•­ ?š™è¸è•­?Ÿº?š™è¸è•­?Ÿ·
        Debug.Log("Game Over");
    }

    void useDoor()
    {
        Debug.Log("use_door: " + is_door);
        if (is_door)
        {
            Debug.Log(2);
            this.transform.position = this.transform.position + add_door_position;
        }

    }
}
