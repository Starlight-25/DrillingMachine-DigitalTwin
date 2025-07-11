using System;
using UnityEngine;

public class LightLayer : MonoBehaviour
{
    private Camera camera;
    private LayerMask affectedLayermask;
    private Light Light;


    
    
    
    private void Start()
    {
        camera = Camera.main;
        affectedLayermask = gameObject.layer;
        Light = GetComponent<Light>();
    }

    
    
    
    
    private void Update()
    {
        bool layerVisible = (camera.cullingMask & (1 << affectedLayermask.value)) != 0;
        Light.enabled = layerVisible;
    }
}