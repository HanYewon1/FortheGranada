using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float player_speed;//플레이어 이동속도

    Rigidbody2D rigidbody2d;
    float player_x;//좌우 움직임
    float player_y;//상하 움직임

    bool is_horizon_move; //4방향 결정

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        player_x = Input.GetAxisRaw("Horizontal"); //좌우 이동
        player_y = Input.GetAxisRaw("Vertical"); //상하 이동

        //눌린 버튼 체크
        bool player_x_down = Input.GetButtonDown("Horizontal");
        bool player_x_up = Input.GetButtonUp("Horizontal");
        bool player_y_down = Input.GetButtonDown("Vertical");
        bool player_y_up = Input.GetButtonUp("Vertical");

        //상하좌우 이동을 위한 조건
        if (player_x_down || player_y_up)
        {
            is_horizon_move = true; //좌우 이동
<<<<<<< HEAD:FortheGranada/Assets/Scripts/playercontroller.cs
        }
        else if (player_y_down || player_x_up)
        {
            is_horizon_move = false; //상하 이동
        }
    }
    private void FixedUpdate()
    {
        Vector2 move_vec = is_horizon_move ? new Vector2(player_x, 0) : new Vector2(0, player_y);
        rigidbody2d.linearVelocity = move_vec * player_speed;
=======

            animator.SetFloat("player_move_x", player_x);

        }
        else if (player_y_down || player_x_up)
        {
            is_horizon_move = false; //상하 이동
            animator.SetFloat("player_move_y", player_y);
        }
>>>>>>> parent of 51f330be2 (플레이어 이동 구현 완료 -예원):FortheGranada/Assets/Script/playercontroller.cs
    }
}
