using UnityEngine;
using UnityEngine.InputSystem;

public class WeightManagement : MonoBehaviour
{
    private TerrainLayer[] TerrainLayers;
    private int[] WeightNeededList = new[] { 20, 40, 60, 80, 100 };
    private InputAction TerrainLayerVisibilityInputAction;
    private MeshRenderer[] MeshRenderers;
    private Transform InfoCanvas;
    private Transform Camera;
    [SerializeField] private Parameters Parameters;
    
    [SerializeField] private Transform DrillBit;
    private MeshRenderer DrillBitMeshRenderer;
    [SerializeField] private Gradient colorGradient;
    private float curGradientFactor;
    private float excavatedDepth = 0f;
    private bool isDigging = false;
    
    
    
    

    private void Start()
    {
        SetTerrainLayer();
        
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
        SetDrillBitColor();
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




    

    private void SetTerrainLayer()
    {
        TerrainLayers = new TerrainLayer[transform.childCount - 1]; //5 layers
        Transform terrainLayerNames = transform.Find("Info Canvas").Find("TerrainLayersNames");
        for (int i = 0; i < TerrainLayers.Length; i++)
            TerrainLayers[i] = new TerrainLayer(transform.GetChild(i), WeightNeededList[i],
                terrainLayerNames.GetChild(i).GetComponent<RectTransform>());
        Parameters.TerrainLayers = TerrainLayers;
    }

    
    
    
    
    public void UpdateTerrainLayer()
    {
        TerrainLayers = Parameters.TerrainLayers;
        for (int i = 0; i < TerrainLayers.Length; i++)
        {
            TerrainLayer terrainLayer = TerrainLayers[i];
            float nextDepth = i == TerrainLayers.Length -1 ? 30 : TerrainLayers[i + 1].Depth;
            Transform terrainTransform = terrainLayer.Transform;
            terrainTransform.localScale = new Vector3(9f, (nextDepth - terrainLayer.Depth) / 2, 9f);
            terrainTransform.position = Vector3.up * (-terrainLayer.Depth - terrainTransform.localScale.y);

            terrainLayer.TMPName.anchoredPosition = terrainTransform.position * 20f;
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


    


    private void SetDrillBitColor()
    {
        float t = Mathf.InverseLerp(0, 100, GetWeightNeeded());
        curGradientFactor = Mathf.Lerp(curGradientFactor, t, Time.deltaTime * 5f);
        DrillBitMeshRenderer.material.color = colorGradient.Evaluate(curGradientFactor);;
    }
    
    private int GetWeightNeeded() => isDigging ? TerrainLayers[GetCurrentDiggingLayerIndex()].WeightNeeded : 0;
    
    private int GetCurrentDiggingLayerIndex()
    {
        int i = 0;
        while (i < TerrainLayers.Length && excavatedDepth < TerrainLayers[i].Depth)
            i++;
        return i-1;
    }
}