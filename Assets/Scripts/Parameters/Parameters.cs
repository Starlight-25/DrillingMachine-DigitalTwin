using UnityEngine;

public class Parameters : MonoBehaviour
{
    public int TimeAcceleration = 1;
    public float DrillingVelocity = 0.05f/60f;
    public float HeightNavVelocity = 0.1f/60f;
    public float RotationVelocity = 5f;

    public TerrainLayer[] TerrainLayers;
    public float LLDepth = 2f;
    public float BELDepth = 9f;
    public float ClayDepth = 16f;
    public float RLDepth = 23f;
    public int MaxWeight = 100;

    public int WaterTemperature = 15;
}