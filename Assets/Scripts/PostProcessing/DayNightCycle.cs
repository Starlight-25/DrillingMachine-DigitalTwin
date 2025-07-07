using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private Transform Light;
    [SerializeField] private MainUIHandler MainUIHandler;
    
    private void Start()
    {
        Light = transform;
    }


    private void Update()
    {
        float curTime = MainUIHandler.GetCurTime();
        float timeInHour = curTime / 3600 % 24;
        float sunAngle = (timeInHour / 24f) * 360f - 90f;
        Light.rotation = Quaternion.Euler(sunAngle, 0, 0);
    }
}