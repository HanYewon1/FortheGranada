using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float player_speed;//?��?��?��?�� ?��?��?��?��
    public Sprite deadSprite;

    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    float player_x;//좌우 ???직임
    float player_y;//?��?�� ???직임

    bool is_horizon_move; //4방향 결정
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
        rigidbody2d.linearVelocity = move_vec * player_speed;
    }

    private void PlayerMove()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //좌우 ?��?��
        player_y = Input.GetAxisRaw("Vertical"); //?��?�� ?��?��

        //?���? 버튼 체크
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //?��?��좌우 ?��?��?�� ?��?�� 조건
        if (player_x_down)
        {
            is_horizon_move = true; //좌우 ?��?��
        }
        else if (player_y_down)
            is_horizon_move = false; //?��?�� ?��?��
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //?��?��메이?��
        if (animator.GetInteger("player_move_x") != player_x) //좌우
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y) //?��?��
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }
        else //idle
            animator.SetBool("is_change", false);
    }

    //���ݹ޾��� ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red; //���������� ����
        yield return new WaitForSeconds(1f); //1�ʵ��� ����
        spriteRenderer.color = originalColor; //���� ������ ���ƿ�
    }
    //���ݹ��� ���
    void Damaged()
    {
        
    }
    public void Dead()
    {
        if (isDead) return; // �̹� ���� ���¶�� �������� ����

        isDead = true; // ���� ���� ����
        spriteRenderer.color = Color.gray; // ���� ����
        spriteRenderer.sprite = deadSprite; // ��������Ʈ ����
        animator.enabled = false; // �ִϸ��̼� ��Ȱ��ȭ
        Debug.Log("Game Over");
    }
}
