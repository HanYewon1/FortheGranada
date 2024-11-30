using UnityEngine;

public class npccontroller : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public float moveSpeed;
    public bool isChasing = false;
    public GameObject[] points;
    public Vector2 movement = Vector2.zero;


    private int nextPoint = 0;
    private float distToPoint;
    
    private bool returnDefault = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log($"isChasing: {isChasing}, returnDefault: {returnDefault}");

        if (isChasing)
        {
            Debug.Log("NPC is chasing, skipping patrol.");
            return;
        }

        if (returnDefault)
        {
            Debug.Log("NPC is returning to the closest patrol point.");
            ReturnDefault();
            if (!returnDefault)
            {
                Debug.Log("Switching to patrol mode after ReturnDefault.");
                NPCMoveDefault();
            }
        }
        else
        {
            Debug.Log("NPC is patrolling.");
            NPCMoveDefault();
        }
    }
    void NPCMoveDefault() //��ο� ���� ������
    {
        if(points.Length == 0)
        {
            Debug.Log("No patrol points available.");
            return;
        }
        //point ���� �Ÿ�
        distToPoint = Vector2.Distance(transform.position, points[nextPoint].transform.position);
        Debug.Log($"Patrolling to point {nextPoint}: {points[nextPoint].transform.position}, Distance: {distToPoint}");
        transform.position = Vector2.MoveTowards(transform.position, points[nextPoint].transform.position, moveSpeed * Time.deltaTime);

            //������ ���� ���
            movement = (points[nextPoint].transform.position - transform.position).normalized;

            //point�� �������� �� ȸ��
            if (distToPoint < 0.2f)
            {

            //���� point
            nextPoint = (nextPoint + 1) % points.Length;

        }

        //�ִϸ��̼�
        animator.SetInteger("npc_x", Mathf.RoundToInt(movement.x));
        animator.SetInteger("npc_y", Mathf.RoundToInt(movement.y));

    }

    public void StartChasing()
    {
        isChasing = true;
        returnDefault = false;
    }

    public void StopChasing()
    {
        if (returnDefault)
        {
            Debug.Log("StopChasing() ignored because returnDefault is already true.");
            return; // �̹� ���� ���¶�� ȣ�� �ߴ�
        }
        isChasing = false;
        returnDefault = true;
        Debug.Log("Chase stopped. Returning to patrol.");
        return;

    }

    void ReturnDefault()
    {
        int closestPoint = 0;
        float minDistance = float.MaxValue;

        for(int i=0;i<points.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, points[i].transform.position);
            if (distance < minDistance){
                minDistance = distance;
                closestPoint = i;
            }
        }
        //���� ��ġ�� ���� ����� ����Ʈ�� �̵�
        distToPoint = Vector2.Distance(transform.position, points[closestPoint].transform.position);
        transform.position = Vector2.MoveTowards(
            transform.position,
            points[closestPoint].transform.position,
            moveSpeed * Time.deltaTime
            );
        movement = (points[closestPoint].transform.position - transform.position).normalized;

        if (distToPoint < 1f)
        {
            Debug.Log("ReturnDefault completed. Switching to patrol mode.");
            returnDefault = false; // ���� ��ȯ
            nextPoint = (nextPoint + 1) % points.Length; // ���� ����Ʈ ����
        }
        else
        {
            Debug.Log($"Returning to point {closestPoint}. Distance remaining: {distToPoint}");
        }
        animator.SetInteger("npc_x", Mathf.RoundToInt(movement.x));
        animator.SetInteger("npc_y", Mathf.RoundToInt(movement.y));
    }

}
