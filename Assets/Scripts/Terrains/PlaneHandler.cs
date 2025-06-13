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

    private List<Vector3> verticles;
    private List<int> triangles;
    private List<Vector2> uvs;
    
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
        GeneratePlane(planeSize, planeResolution);
        Hole(other.transform.position.y);
        AssignMesh();
    }

    
    
    

    private void GeneratePlane(Vector2 size, int resolution)
    {
        verticles = new List<Vector3>();
        uvs = new List<Vector2>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution+1; x++)
            {
                verticles.Add(new Vector3(x * xPerStep - size.x / 2f, 0, y * yPerStep - size.y / 2f));
                uvs.Add(new Vector2((float)x / resolution, (float)y / resolution));
            }
        }

        triangles = new List<int>();
        for (int row = 0; row < resolution; row++)
        {
            for (int col = 0; col < resolution; col++)
            {
                int i = row * resolution + row + col;
                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);

                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }
    }

    
    
    

    private void AssignMesh()
    {
        Mesh.Clear();
        Mesh.vertices = verticles.ToArray();
        Mesh.triangles = triangles.ToArray();
        Mesh.uv = uvs.ToArray();
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();

        MeshCollider.sharedMesh = null;
        MeshCollider.sharedMesh = Mesh;
    }
    
    
    
    
    
    private void Hole(float depth)
    {
        if (curDepth > depth) curDepth = depth;
        for (int i = 0; i < verticles.Count; i++)
        {
            Vector3 vertex = verticles[i];
            float distance = new Vector2(vertex.x, vertex.z).magnitude;
            if (distance < radius) vertex.y += curDepth;
            verticles[i] = vertex;
        }
    }
}