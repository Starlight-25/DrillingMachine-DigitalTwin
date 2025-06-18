using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TerrainLayer : MonoBehaviour
{
    private InputAction TerrainLayerVisibilityInputAction;
    private MeshRenderer[] MeshRenderers;
    
    
    
    
    
    private void Start()
    {
        TerrainLayerVisibilityInputAction = GetComponent<PlayerInput>().actions["TerrainLayerVisibility"];
        MeshRenderers = new MeshRenderer[transform.childCount];
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }
    }

    
    
    

    private void Update()
    {
        if (TerrainLayerVisibilityInputAction.triggered) ChangeTerrainLayerVisibility();
    }


    
    
    
    private void ChangeTerrainLayerVisibility()
    {
        foreach (MeshRenderer mesh in MeshRenderers)
        {
            mesh.enabled = !mesh.enabled;
        }
    }
}