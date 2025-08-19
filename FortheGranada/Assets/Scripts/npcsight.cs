using System.Collections;
using UnityEngine;

public class npcsight : MonoBehaviour
{
    [Range(1, 360)] public float angle = 60f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public MeshFilter viewMeshFilter;
    public int segments = 50;
    public float radius = 6f;

    npccontroller npc_controller;

    public Transform Target { get; private set; }
    public bool DetectPlayer { get; private set; }



    private Mesh viewMesh;

    void Start()
    {
        //vieMeshFilter 연결 안되어있을 때 자식 객체에서 가져옴
        npc_controller = GetComponent<npccontroller>();
        if (viewMeshFilter == null)
            viewMeshFilter = GetComponentInChildren<MeshFilter>();


        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void Update()
    {
        Detect();
        DrawFieldOfView();
    }

    private void Detect() //플레이어 탐지
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            //첫번째로 감지된 타겟
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // NPC가 바라보는 방향과 플레이어 방향 각도가 시야 각도 이내인지
            if (Vector3.Angle(new Vector3(npc_controller.movement.x, npc_controller.movement.y, 0), directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // NPC와 플레이어 사이에 장애물 없으면 탐지 성공
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    DetectPlayer = true;
                    Target = target;
                    return;

                }
            }
        }

        DetectPlayer = false;
        Target = null;

      }
    private void DrawFieldOfView() //시야메쉬 그리기
    {
        Vector3 forwardDirection = new Vector3(npc_controller.movement.x, npc_controller.movement.y, 0);
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // 메쉬 중심

        for (int i = 0; i <= segments; i++) //시야 각도를 기준으로 분할해서 메쉬 꼭짓점 계산
        {
            float currentAngle = -angle / 2 + (angle / segments) * i;
            Vector3 direction = RotateVector(forwardDirection, currentAngle).normalized;
            vertices[i + 1] = direction * radius;

            if (i < segments) //메쉬 그릴 인덱스 지정
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        //메쉬 갱신
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    //NPC가 보는 방향으로 시야 각도 변경
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
