using UnityEngine;

public class npcsight : MonoBehaviour
{
    public float radius = 5f;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public MeshFilter viewMeshFilter;
    public int segments = 50;

    npccontroller Npccontroller;
    MeshRenderer meshrenderer;

    public bool DetectPlayer { get; private set; }

    private Mesh viewMesh;

    void Start()
    {
        Npccontroller = GetComponent<npccontroller>();
        meshrenderer = viewMeshFilter.GetComponent<MeshRenderer>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void Update()
    {
        Detect();
        DrawFieldOfView();
    }

    private void Detect()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // �þ߰� ���ο� �ִ��� Ȯ��
            if (Vector3.Angle(new Vector3(Npccontroller.movement.x, Npccontroller.movement.y, 0), directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // ��ֹ��� ���� �������� �ʾҴ��� Ȯ��
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    DetectPlayer = true;
                    return;
                }
            }
        }

        DetectPlayer = false;
    
}

    //�þ� ����
    private void DrawFieldOfView()
    {
        Vector3 forwardDirection = new Vector3(Npccontroller.movement.x, Npccontroller.movement.y, 0);
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // �߽���

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -angle / 2 + (angle / segments) * i;
            Vector3 direction = RotateVector(forwardDirection, currentAngle).normalized;
            vertices[i + 1] = direction * radius;

            if (i < segments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

        // �÷��̾� ���� �� �þ� ���� ����
        if (meshrenderer != null)
        {
            meshrenderer.material.color = DetectPlayer ? Color.yellow : Color.red;
        }
    }

    //npc ���� ���� �þ� ����
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
