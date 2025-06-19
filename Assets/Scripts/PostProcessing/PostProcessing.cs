using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour, ISettingsUpdater
{
    private Volume Volume;
    [SerializeField] private VolumeProfile SurfacePostProcessing;
    [SerializeField] private VolumeProfile UnderWaterPostProcessing;
    
    private float WaterSurface = 30f;
    private Color SurfaceFogColor = new(0xA1 / 255f, 0xBA / 255f, 0xC1 / 255f); //A1BAC1
    private Color UnderwaterFogColor = new(0x56 / 255f, 0xBF / 255f, 0xCF / 255f); //56BFCF

    [SerializeField] private SettingsHandler SettingsHandler;
    private int FogDistance;
    
    
    
    
    
    private void Start()
    {
        SettingsHandler.Add(this);
        UpdateFromSettings();
        
        Volume = GetComponent<Volume>();
    }

    
    
    
    
    private void Update()
    {
        float CameraY = transform.position.y;
        ChangeEffect(CameraY >= 0 && CameraY <= WaterSurface);
    }

    
    
    
    
    public void UpdateFromSettings()
    {
        FogDistance = SettingsHandler.Settings.Graphics.FogDistance;
    }

    
    
    

    private void ChangeEffect(bool underwater)
    {
        if (underwater)
        {
            Volume.profile = UnderWaterPostProcessing;
            RenderSettings.fogColor = UnderwaterFogColor;
            RenderSettings.fogStartDistance = 25f;
            RenderSettings.fogEndDistance = FogDistance;
        }
        else
        {
            Volume.profile = SurfacePostProcessing;
            RenderSettings.fogColor = SurfaceFogColor;
            RenderSettings.fogStartDistance = 100f;
            RenderSettings.fogEndDistance = 450f;
        }
    }
}