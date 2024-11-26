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
