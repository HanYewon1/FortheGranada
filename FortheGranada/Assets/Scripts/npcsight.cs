using UnityEngine;

public class npcsight : MonoBehaviour
{
    public float radius = 5f;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public MeshFilter viewMeshFilter;
    public int segments = 50;

    GameObject player;
    npccontroller Npccontroller;

    public bool DetectPlayer { get; private set; }

    private Mesh viewMesh;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Npccontroller = GetComponent<npccontroller>();

        if (viewMeshFilter == null)
        {
            Debug.LogError("MeshFilter가 설정되지 않았습니다. Inspector에서 추가해주세요.");
            return;
        }

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void Update()
    {
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        Vector3 forwardDirection = new Vector3(Npccontroller.movement.x, Npccontroller.movement.y, 0);
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // 중심점

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

        // 플레이어 감지 시 머티리얼 색상 변경
        MeshRenderer renderer = viewMeshFilter.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = DetectPlayer ? Color.green : Color.red;
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
