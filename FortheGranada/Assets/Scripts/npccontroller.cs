using UnityEngine;

public class npccontroller : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public float moveSpeed;
    public GameObject[] points;
    public Vector2 movement = Vector2.zero;

    int nextPoint = 0;
    float distToPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        NPCMoveDefault();
    }
    void NPCMoveDefault() //��ο� ���� ������
    {

        //point ���� �Ÿ�
        distToPoint = Vector2.Distance(transform.position, points[nextPoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position, points[nextPoint].transform.position, moveSpeed * Time.deltaTime);

        //������ ���� ���
        movement = (points[nextPoint].transform.position - transform.position).normalized;

        //point�� �������� �� ȸ��
        if (distToPoint < 0.2f)
        {

            //���� point
            nextPoint++;
            if (nextPoint == points.Length) //point �� ���� 0���� �ٽ� ����
                nextPoint = 0;
        }

        //�ִϸ��̼�
        animator.SetInteger("npc_x", Mathf.RoundToInt(movement.x)); 
        animator.SetInteger("npc_y", Mathf.RoundToInt(movement.y)); 

    }

}
