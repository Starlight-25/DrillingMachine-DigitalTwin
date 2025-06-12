
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    private float radius = 4.5f;
    private float depth = 10;
    private Mesh Mesh;
    private Vector3[] curVerticles;
    private Vector3[] newVerticles;

    private void Start()
    {
        Mesh = GetComponent<MeshFilter>().mesh;
        curVerticles = Mesh.vertices;
        newVerticles = new Vector3[curVerticles.Length];
        Hole();
    }



    private void Hole()
    {
        for (int i = 0; i < curVerticles.Length; i++)
        {
            Vector3 vertex = curVerticles[i];
            float distance = new Vector2(vertex.x, vertex.z).magnitude;
            if (distance < radius) vertex.y -= depth;
            newVerticles[i] = vertex;
        }

        Mesh.vertices = newVerticles;
    }
}