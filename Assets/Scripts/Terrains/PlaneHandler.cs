using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHandler : MonoBehaviour
{
    private Mesh Mesh;
    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;
    [SerializeField] private Vector2 planeSize = new Vector2(10, 10);
    [SerializeField] private int planeResolution = 10;

    private Vector3[] verticles;
    private int[] triangles;
    private Vector2[] uvs;
    
    private float radius = 4.5f;
    private float curDepth = 0f;

    
    
    
    private void Start()
    {
        Mesh = new Mesh();
        MeshFilter = GetComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
        MeshCollider = GetComponent<MeshCollider>();
        GeneratePlane(planeSize, planeResolution);
        AssignMesh();
    }


    
    
    
    private void OnCollisionEnter(Collision other)
    {
        Hole(other.transform.position.y);
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

    
    
    

    private void AssignMesh()
    {
        Mesh.Clear();
        Mesh.vertices = verticles;
        Mesh.triangles = triangles;
        Mesh.uv = uvs;
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();

        MeshCollider.sharedMesh = null;
        MeshCollider.sharedMesh = Mesh;
    }
    
    
    
    
    
    private void Hole(float depth)
    {
        if (curDepth > depth) curDepth = depth;
        for (int i = 0; i < verticles.Length; i++)
        {
            Vector3 vertex = verticles[i];
            float distance = new Vector2(vertex.x, vertex.z).magnitude;
            if (distance < radius) vertex.y = curDepth;
            verticles[i] = vertex;
        }
    }
}