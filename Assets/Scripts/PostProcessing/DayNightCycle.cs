using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private Transform Light;
    [SerializeField] private Material skyboxDay;
    [SerializeField] private Material skyboxNight;
    [SerializeField] private MainUIHandler MainUIHandler;
    
    
    
    
    
    private void Start()
    {
        Light = transform;
    }

    
    
    

    private void Update()
    {
        float curTime = MainUIHandler.GetCurTime();
        float timeInHour = curTime / 3600 % 24;
        
        LightRotation(timeInHour);
        ChangeSkybox(timeInHour);
    }

    
    
    

    private void LightRotation(float timeInHour)
    {
        float sunAngle = timeInHour / 24f * 360f - 90f;
        Light.rotation = Quaternion.Euler(sunAngle, 0, 0);
    }



    

    private void ChangeSkybox(float timeInHour)
    {
        Material targetSkybox = (timeInHour >= 6f && timeInHour < 18f) ? skyboxDay : skyboxNight;
        if (RenderSettings.skybox != targetSkybox)
        {
            RenderSettings.skybox = targetSkybox;
            DynamicGI.UpdateEnvironment();
        }
    }
}