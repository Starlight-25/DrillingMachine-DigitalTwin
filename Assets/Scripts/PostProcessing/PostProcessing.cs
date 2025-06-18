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
        if (underwater)
        {
            RenderSettings.fogColor = new Color(0x56/255f, 0xBF/255f, 0xCF/255f); //56BFCF
            RenderSettings.fogStartDistance = 25f;
            RenderSettings.fogEndDistance = 75f;
        }
        else
        {
            RenderSettings.fogColor = new Color(0xA1/255f, 0xBA/255f, 0xC1/255f); //A1BAC1
            RenderSettings.fogStartDistance = 500f;
            RenderSettings.fogEndDistance = 700f;
        }
    }
}