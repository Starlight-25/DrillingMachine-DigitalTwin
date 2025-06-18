using TMPro;
using UnityEngine;

public class ParametersMenuHandler : MonoBehaviour
{
    [SerializeField] private Parameters Parameters;
    private int[] timeAccelerationMap = { 1, 30, 60, 300, 900, 1800, 3600, 7200, 18000, 43200, 86400 };
    [SerializeField] private TextMeshProUGUI DrillingVelocityValueText;
    [SerializeField] private TextMeshProUGUI RotationVelocityValueText;

    
    
    

    public void UpdateTimeSpeedDropDown(int index)
    {
        Parameters.TimeAcceleration = timeAccelerationMap[index];
    }

    
    
    

    public void UpdateDrillingVelocitySlider(float value)
    {
        float velocity = value / 60;
        Parameters.DrillingVelocity = velocity;
        Parameters.HeightNavVelocity = 2 * velocity;
        DrillingVelocityValueText.text = $"{(int)value}cm/min";
    }


    
    
    
    public void UpdateRotationVelocitySlider(float value)
    {
        Parameters.RotationVelocity = value;
        RotationVelocityValueText.text = $"{(int)value}RPM";
    }
}