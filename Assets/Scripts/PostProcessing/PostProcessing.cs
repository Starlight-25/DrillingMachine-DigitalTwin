using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    private Volume Volume;
    [SerializeField] private VolumeProfile SurfacePostProcessing;
    [SerializeField] private VolumeProfile UnderWaterPostProcessing;
    
    private float WaterSurface = 30f;

    private void Start()
    {
        Volume = GetComponent<Volume>();
    }

    private void Update()
    {
        float CameraY = transform.position.y;
        ChangeEffect(CameraY >= 0 && CameraY <= WaterSurface);
    }

    private void ChangeEffect(bool underwater)
    {
        Volume.profile = underwater ? UnderWaterPostProcessing : SurfacePostProcessing;
    }
}