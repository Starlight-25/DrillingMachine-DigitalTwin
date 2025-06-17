using UnityEngine;

public class TargetPlane : MonoBehaviour
{
    private Mesh Mesh;
    private MeshFilter MeshFilter;
    [SerializeField] private Vector2 planeSize = new Vector2(20, 20);
    [SerializeField] private int planeResolution = 50;

    private Vector3[] verticles;
    private int[] triangles;
    private Vector2[] uvs;

    private float radius = 4.75f;
    
    
    
    private void Start()
    {
        Mesh = new Mesh();
        MeshFilter = GetComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
        GeneratePlane(planeSize, planeResolution);
        Hole();
        AssignMesh();
    }

    
    
    
    
    private void GeneratePlane(Vector2 size, int resolution)
    {
        int vertCount = (resolution + 1) * (resolution + 1);
        verticles = new Vector3[vertCount];
        uvs = new Vector2[vertCount];
        triangles = new int[resolution * resolution * 6];
        
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;

        int vertexIndex = 0;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution+1; x++, vertexIndex++)
            {
                verticles[vertexIndex] = new Vector3(x * xPerStep - size.x / 2f, 0, y * yPerStep - size.y / 2f);
                uvs[vertexIndex] = new Vector2((float)x / resolution, (float)y / resolution);
            }
        }

        int triangleIntex = 0;
        for (int row = 0; row < resolution; row++)
        {
            for (int col = 0; col < resolution; col++)
            {
                int i = row * resolution + row + col;
                triangles[triangleIntex++] = i;
                triangles[triangleIntex++] = i + resolution + 1;
                triangles[triangleIntex++] = i + resolution + 2;

                triangles[triangleIntex++] = i;
                triangles[triangleIntex++] = i + resolution + 2;
                triangles[triangleIntex++] = i + 1;
            }
        }
    }

    
    
    
    
    private void Hole()
    {
        for (int i = 0; i < verticles.Length; i++)
        {
            Vector3 vertex = verticles[i];
            float distance = new Vector2(vertex.x, vertex.z).magnitude;
            if (distance < radius) vertex.y = -30.25f;
            verticles[i] = vertex;
        }
    }

    
    
    
    
    private void AssignMesh()
    {
        Mesh.Clear();
        Mesh.vertices = verticles;
        Mesh.triangles = triangles;
        Mesh.uv = uvs;
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();
    }
}