using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcsight : MonoBehaviour
{
    public float radius = 5f;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    Vector3 forwardDirection;
    GameObject player;
    npccontroller Npccontroller;

    public bool DetectPlayer { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Npccontroller = GetComponent<npccontroller>();
        StartCoroutine(FOVCheck());

    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        //플레이어 감지
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if(rangeCheck.Length > 0 ) 
        {
            //타겟 방향 계산
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(Npccontroller.movement, directionToTarget) < angle / 2)
            {
                //타겟과의 거리 계산
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (distanceToTarget <= radius && !Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                    DetectPlayer = true;
                else 
                    DetectPlayer = false;
            }
            else 
                DetectPlayer= false;
        }
        else if(DetectPlayer)
            DetectPlayer = false;
    }
    private void OnDrawGizmos() //시야 표시
    {
        // Npccontroller가 null일 때는 메서드 실행 중단
        if (Npccontroller == null)
        {
            return;
        }
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        forwardDirection = new Vector3(Npccontroller.movement.x, Npccontroller.movement.y, 0);
        Vector3 angle01 = RotateVector(forwardDirection, -angle / 2);
        Vector3 angle02 = RotateVector(forwardDirection, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);

        if (DetectPlayer)
        { 
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    private Vector2 RotateVector(Vector3 direction, float offsetAngle)
    {
        float angleRadius = offsetAngle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angleRadius);
        float sin = Mathf.Sin(angleRadius);

        return new Vector3(direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos,
            0);
    }
}

