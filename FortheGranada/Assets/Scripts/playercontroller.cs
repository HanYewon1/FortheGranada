using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float player_speed;//플레이어 이동속도

    Rigidbody2D rigidbody2d;
    Animator animator;

    float player_x;//좌우 움직임
    float player_y;//상하 움직임

    bool is_horizon_move; //4방향 결정

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        PlayerMove();   

    }

    private void FixedUpdate()
    {
        Vector2 move_vec = is_horizon_move ? new Vector2(player_x, 0) : new Vector2(0, player_y);
        rigidbody2d.linearVelocity = move_vec * player_speed;
    }

    private void PlayerMove()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //좌우 이동
        player_y = Input.GetAxisRaw("Vertical"); //상하 이동

        //눌린 버튼 체크
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //상하좌우 이동을 위한 조건
        if (player_x_down)
        {
            is_horizon_move = true; //좌우 이동
        }
        else if (player_y_down)
            is_horizon_move = false; //상하 이동
        else if (player_x_up || player_y_up)
            is_horizon_move = player_x != 0;

        //애니메이션
        if (animator.GetInteger("player_move_x") != player_x) //좌우
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_x", (int)player_x);
        }
        else if (animator.GetInteger("player_move_y") != player_y)
        {
            animator.SetBool("is_change", true);
            animator.SetInteger("player_move_y", (int)player_y);
        }//상하
        else //idle
                    animator.SetBool("is_change", false);
    }
}
