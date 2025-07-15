using TMPro;
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
    private int maxWeight;
    private float excavatedDepth = 0f;
    private bool isDigging = false;

    [SerializeField] private Transform ColorBar;
    private TextMeshProUGUI[] IndicatorsText;
    
    
    
    

    private void Start()
    {
        InfoCanvas = transform.GetChild(transform.childCount - 1);
        SetTerrainLayer();
        
        TerrainLayerVisibilityInputAction = GetComponent<PlayerInput>().actions["TerrainLayerVisibility"];
        
        MeshRenderers = new MeshRenderer[transform.childCount - 1];
        for (int i = 0; i < MeshRenderers.Length; i++)
            MeshRenderers[i] = transform.GetChild(i).GetComponent<MeshRenderer>();

        Camera = UnityEngine.Camera.main.transform;
        
        DrillBitMeshRenderer = DrillBit.GetComponent<MeshRenderer>();

        IndicatorsText = new[]
        {
            ColorBar.GetChild(1).GetComponent<TextMeshProUGUI>(), ColorBar.GetChild(2).GetComponent<TextMeshProUGUI>(),
            ColorBar.GetChild(3).GetComponent<TextMeshProUGUI>(), ColorBar.GetChild(4).GetComponent<TextMeshProUGUI>(),
            ColorBar.GetChild(5).GetComponent<TextMeshProUGUI>(), ColorBar.GetChild(6).GetComponent<TextMeshProUGUI>()
        };
        
        UpdateFromParameters();
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
        Transform terrainLayerNames = InfoCanvas.Find("TerrainLayersNames");
        for (int i = 0; i < TerrainLayers.Length; i++)
            TerrainLayers[i] = new TerrainLayer(transform.GetChild(i), WeightNeededList[i],
                terrainLayerNames.GetChild(i).GetComponent<RectTransform>());
        Parameters.TerrainLayers = TerrainLayers;
    }





    public void UpdateFromParameters()
    {
        UpdateTerrainLayer();
        ChangeMaxWeightTerrain();
        ChangeColorBarIndicators();
    }
    
    
    
    
    
    public void UpdateTerrainLayer()
    {
        TerrainLayers = Parameters.TerrainLayers;
        for (int i = 0; i < TerrainLayers.Length; i++)
        {
            TerrainLayer terrainLayer = TerrainLayers[i];
            Transform terrainTransform = terrainLayer.Transform;
            float nextDepth = i == TerrainLayers.Length -1 ? 30 : TerrainLayers[i + 1].Depth;
            float depth = terrainLayer.Depth;
            terrainTransform.localScale = new Vector3(9f, (nextDepth - depth) / 2, 9f);
            terrainTransform.position = Vector3.up * (-depth - terrainTransform.localScale.y);

            terrainLayer.TMPName.anchoredPosition = terrainTransform.position * 20f;
        }
    }





    public void ChangeMaxWeightTerrain() => maxWeight = Parameters.MaxWeight;



    

    private void ChangeColorBarIndicators()
    {
        IndicatorsText[0].text = "0";
        IndicatorsText[IndicatorsText.Length - 1].text = maxWeight.ToString("F0");
        float interval = maxWeight / 5f;
        for (int i = 1; i < IndicatorsText.Length - 1; i++)
        {
            Debug.Log(i);
            IndicatorsText[i].text = (i * interval).ToString("F0");
        }
    }


    
    
    
    private void ChangeTerrainLayerVisibility()
    {
        foreach (MeshRenderer mesh in MeshRenderers)
            mesh.enabled = !mesh.enabled;
        GameObject infoCanvasGameobject = InfoCanvas.gameObject;
        infoCanvasGameobject.SetActive(!infoCanvasGameobject.activeInHierarchy);
    }


    
    

    private void InfoCanvasLookAtCamera()
    {
        InfoCanvas.LookAt(Camera);
        Vector3 angles = InfoCanvas.rotation.eulerAngles;
        angles.x = 0f;
        InfoCanvas.rotation = Quaternion.Euler(angles);
    }


    


    private void SetDrillBitColor()
    {
        float t = Mathf.InverseLerp(0, maxWeight, GetWeightNeeded());
        curGradientFactor = Mathf.Lerp(curGradientFactor, t, Time.deltaTime * 5f);
        DrillBitMeshRenderer.material.color = colorGradient.Evaluate(curGradientFactor);;
    }
    
    public int GetWeightNeeded() => isDigging ? TerrainLayers[GetCurrentDiggingLayerIndex()].WeightNeeded : 0;
    
    private int GetCurrentDiggingLayerIndex()
    {
        int i = 0;
        while (i < TerrainLayers.Length && excavatedDepth < -TerrainLayers[i].Depth)
            i++;
        return i-1;
    }
}