using UnityEngine;

public class TerrainLayer
{
    public string Name;
    public Transform Transform;
    public float Depth;
    public int WeightNeeded;
    public RectTransform TMPName;

    public TerrainLayer(Transform transform, int weightNeeded, RectTransform tmpName)
    {
        Name = transform.name;
        Transform = transform;
        WeightNeeded = weightNeeded;
        TMPName = tmpName;
        Depth = transform.position.y + transform.localScale.y;
    }
}
