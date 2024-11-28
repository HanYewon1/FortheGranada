using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{

    public float player_speed;//??��踝蕭??��踝蕭??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭??��踝蕭??��踝蕭
    public Sprite deadSprite;


    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    Vector3 add_door_position;

    float player_x;//鮈�? ???鴔�?
    float player_y;//??��踝蕭??��踝蕭 ???鴔�?

    bool isDead = false;
    bool is_door;
    bool is_horizon_move; //4방향 결정


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

        if(Input.GetKeyDown(KeyCode.F))

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
        player_x = Input.GetAxisRaw("Horizontal"); //鮈�? ??��踝蕭??��踝蕭
        player_y = Input.GetAxisRaw("Vertical"); //??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭

        //??��踝蕭?���?? 貒?�� �?渣
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //??��踝蕭??��踝蕭鮈�? ??��踝蕭??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭 魽國?��
        if (player_x_down)
        {
            is_horizon_move = true; //鮈�? ??��踝蕭??��踝蕭
        }
        else if (player_y_down)
            is_horizon_move = false; //??��踝蕭??��踝蕭 ??��踝蕭??��踝蕭
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //??��踝蕭??��踝蕭諰�???��踝蕭
        if (animator.GetInteger("player_move_x") != player_x) //鮈�?
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y) //??��踝蕭??��踝蕭
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }
        else //idle
            animator.SetBool("is_change", false);
    }

    //?��踝蕭?��豎嫡橘蕭?��踝蕭 ?��踝蕭
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
        spriteRenderer.color = Color.red; //?��踝蕭?��踝蕭?��踝蕭?��踝蕭?��踝蕭 ?��踝蕭?��踝蕭
        yield return new WaitForSeconds(1f); //1?��褊蛛?��?��踝蕭 ?��踝蕭?��踝蕭
        spriteRenderer.color = originalColor; //?��踝蕭?��踝蕭 ?��踝蕭?��踝蕭?��踝蕭 ?��踝蕭?��複選?��
    }

    //?�踝??��豎對??��踝蕭 ?�踝??���?
 
    public void Dead()

    {
        if (isDead) return; // ?�諒對蕭 ?�踝??��踝蕭 ?�踝??��?�塚??���??�踝??��踝蕭?�踝??��踝蕭 ?�踝??��踝蕭

        isDead = true; // ?�踝??��踝蕭 ?�踝??��踝蕭 ?�踝??��踝蕭
        spriteRenderer.color = Color.gray; // ?�踝??��踝蕭 ?�踝??��踝蕭
        spriteRenderer.sprite = deadSprite; // ?�踝??��踝蕭?�踝??��踝蕭???�踝??��踝蕭
        animator.enabled = false; // ?�誰棲賂??��諒潘???�踝??��?�踝??��
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
