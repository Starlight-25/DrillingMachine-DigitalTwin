using UnityEngine;
using UnityEngine.InputSystem;

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





public class WeightManagement : MonoBehaviour
{
    private TerrainLayer[] TerrainLayers;
    private int[] WeightNeededList = new[] { 20, 40, 60, 80, 100 };
    private InputAction TerrainLayerVisibilityInputAction;
    private MeshRenderer[] MeshRenderers;
    private Transform InfoCanvas;
    private Transform Camera;
    
    [SerializeField] private Transform DrillBit;
    private MeshRenderer DrillBitMeshRenderer;
    [SerializeField] private Gradient colorGradient;
    private float excavatedDepth = 0f;
    private bool isDigging = false;
    
    
    

    private void Start()
    {
        UpdateTerrainLayer();
        
        TerrainLayerVisibilityInputAction = GetComponent<PlayerInput>().actions["TerrainLayerVisibility"];
        
        MeshRenderers = new MeshRenderer[transform.childCount - 1];
        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }

        InfoCanvas = transform.GetChild(transform.childCount - 1);
        Camera = UnityEngine.Camera.main.transform;
        
        DrillBitMeshRenderer = DrillBit.GetComponent<MeshRenderer>();
    }

    
    
    
    
    private void Update()
    {
        UpdateIsDigging();
        if (TerrainLayerVisibilityInputAction.triggered) ChangeTerrainLayerVisibility();
        InfoCanvasLookAtCamera();
        SetDrillBitColor(GetWeightNeeded());
    }
    
    
    
    
    
    private void UpdateIsDigging()
    {
        if (DrillBit.position.y < excavatedDepth)
        {
            isDigging = true;
            excavatedDepth = DrillBit.position.y;
        }
        else isDigging = false;
    }




    

    public void UpdateTerrainLayer()
    {
        TerrainLayers = new TerrainLayer[transform.childCount - 1]; //5 layers
        for (int i = 0; i < TerrainLayers.Length; i++)
        {
            TerrainLayers[i] = new TerrainLayer(transform.GetChild(i), WeightNeededList[i]);
        }
    }


    
    
    
    private void ChangeTerrainLayerVisibility()
    {
        foreach (MeshRenderer mesh in MeshRenderers)
        {
            mesh.enabled = !mesh.enabled;
        }
    }


    
    

    private void InfoCanvasLookAtCamera()
    {
        InfoCanvas.LookAt(Camera);
        var angles = InfoCanvas.rotation.eulerAngles;
        angles.x = 0f;
        InfoCanvas.rotation = Quaternion.Euler(angles);
    }





    private int GetCurrentDiggingLayerIndex()
    {
        int i = 0;
        while (i < TerrainLayers.Length && excavatedDepth < TerrainLayers[i].Depth)
            i++;
        return i-1;
    }

    
    
    
    
    private int GetWeightNeeded()
    {
        if (!isDigging) return 0;
        return TerrainLayers[GetCurrentDiggingLayerIndex()].WeightNeeded;
    }


    
    

    private void SetDrillBitColor(int weight)
    {
        float t = Mathf.InverseLerp(0, 100, weight);
        DrillBitMeshRenderer.material.color = colorGradient.Evaluate(t);
    }
}