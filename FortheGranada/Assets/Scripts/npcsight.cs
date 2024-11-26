<<<<<<< Updated upstream
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering;

public class npcsight : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh;
        mesh = new Mesh();
        vertices = new Vector3[3];
        uv = new Vector2[3];
        triangles = new int[3];

        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(50, 0);
        vertices[2] = new Vector3(0, -50);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcsight : MonoBehaviour
{
    public float radius = 5f;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    public GameObject player;

    public bool DetectPlayer { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if(rangeCheck.Length > 0 ) 
        {
            //타겟 방향 계산
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                //타겟과의 거리 계산
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);

        if (DetectPlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
>>>>>>> Stashed changes
