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
    void NPCMoveDefault() //경로에 따른 움직임
    {

        //point 사이 거리
        distToPoint = Vector2.Distance(transform.position, points[nextPoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position, points[nextPoint].transform.position, moveSpeed * Time.deltaTime);

        //움직임 방향 계산
        movement = (points[nextPoint].transform.position - transform.position).normalized;

        //point에 도달했을 때 회전
        if (distToPoint < 0.2f)
        {

            //다음 point
            nextPoint++;
            if (nextPoint == points.Length) //point 다 돌면 0부터 다시 시작
                nextPoint = 0;
        }

        //애니메이션
        animator.SetInteger("npc_x", Mathf.RoundToInt(movement.x)); 
        animator.SetInteger("npc_y", Mathf.RoundToInt(movement.y)); 

    }

}
