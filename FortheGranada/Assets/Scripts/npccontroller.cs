using UnityEngine;

public class npccontroller : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;

    public float moveSpeed;
    public GameObject[] points;

    int nextPoint = 0;
    float distToPoint;

    Vector2 lastPosition;
    Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        
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

        //현재 프레임 움직임 계산
        movement = (Vector2)transform.position - lastPosition;
        lastPosition = transform.position;

        //point에 도달했을 때 회전
        if (distToPoint < 0.2f )
        {
            Vector3 currRot = transform.eulerAngles;
            currRot.z += points[nextPoint].transform.eulerAngles.z;
            transform.eulerAngles = currRot;

            //다음 point
            nextPoint++;
            if (nextPoint == points.Length) //point 다 돌면 0부터 다시 시작
                nextPoint = 0;
        }

        //애니메이션
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            animator.SetInteger("npc_x", (int)movement.x);
        }
        else
        {
            animator.SetInteger("npc_y", (int)movement.y);
        }
    }
}
