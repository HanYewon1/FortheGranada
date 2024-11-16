using UnityEngine;

public class npccontroller : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;

    float npc_x; //좌우 움직임
    float npc_y; //상하 움직임
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        NPCMove();
    }
    void NPCMove()
    {
        npc_x = Input.GetAxisRaw("Horizontal"); //좌우 이동
        npc_y = Input.GetAxisRaw("Vertical"); //상하 이동

        //애니메이션

    }
}
