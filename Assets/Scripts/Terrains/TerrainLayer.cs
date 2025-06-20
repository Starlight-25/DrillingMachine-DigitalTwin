using UnityEngine;

public class TerrainLayer
{
    public string Name;
    public Transform Transform;
    public float Depth;
    public int WeightNeeded;

    public TerrainLayer(Transform transform, int weightNeeded)
    {
        Name = transform.name;
        Transform = transform;
        WeightNeeded = weightNeeded;
        Depth = transform.position.y + transform.localScale.y;
    }
}
