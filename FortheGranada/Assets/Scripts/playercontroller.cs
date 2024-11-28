using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float player_speed;//?”Œ? ˆ?´?–´ ?´?™?†?„

    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    public Sprite deadSprite;

    float player_x;//ì¢Œìš° ???ì§ì„
    float player_y;//?ƒ?•˜ ???ì§ì„

    bool is_horizon_move; //4ë°©í–¥ ê²°ì •

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
        rigidbody2d.linearVelocity = move_vec * player_speed;
    }

    private void PlayerMove()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //ì¢Œìš° ?´?™
        player_y = Input.GetAxisRaw("Vertical"); //?ƒ?•˜ ?´?™

        //?ˆŒë¦? ë²„íŠ¼ ì²´í¬
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //?ƒ?•˜ì¢Œìš° ?´?™?„ ?œ„?•œ ì¡°ê±´
        if (player_x_down)
        {
            is_horizon_move = true; //ì¢Œìš° ?´?™
        }
        else if (player_y_down)
            is_horizon_move = false; //?ƒ?•˜ ?´?™
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //?• ?‹ˆë©”ì´?…˜
        if (animator.GetInteger("player_move_x") != player_x) //ì¢Œìš°
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y) //?ƒ?•˜
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }
        else //idle
            animator.SetBool("is_change", false);
    }

    //°ø°İ¹Ş¾ÒÀ» ¶§
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red; //»¡°£»öÀ¸·Î º¯ÇÔ
        yield return new WaitForSeconds(1f); //1ÃÊµ¿¾È À¯Áö
        spriteRenderer.color = originalColor; //¿ø·¡ »öÀ¸·Î µ¹¾Æ¿È
    }
    //°ø°İ¹ŞÀ» °æ¿ì
    void Damaged()
    {
        
    }

    void isDead()
    {
        spriteRenderer.color = Color.gray; //È¸»öÀ¸·Î º¯ÇÔ
        spriteRenderer.sprite = deadSprite; //½ºÇÁ¶óÀÌÆ® º¯°æ
        animator.enabled = false; //¾Ö´Ï¸ŞÀÌ¼Ç Áß´Ü
    }
}
