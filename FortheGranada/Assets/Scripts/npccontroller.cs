using UnityEngine;

public class npccontroller : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;

    float npc_x; //�¿� ������
    float npc_y; //���� ������
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
        npc_x = Input.GetAxisRaw("Horizontal"); //�¿� �̵�
        npc_y = Input.GetAxisRaw("Vertical"); //���� �̵�

        //�ִϸ��̼�

    }
}
