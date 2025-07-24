using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReplayTerrainLayerHandler : MonoBehaviour
{
    private TerrainLayer[] TerrainLayers;
    private MeshRenderer[] MeshRenderers;
    private Transform InfoCanvas;
    private Transform Camera;

    private InputAction TerrainLayerVisibilityInputAction;

    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;
    private float MaxDepth;
    
    
    

    private void Start()
    {
        InfoCanvas = transform.GetChild(transform.childCount - 1);
        Camera = UnityEngine.Camera.main.transform;
        TerrainLayerVisibilityInputAction = GetComponent<PlayerInput>().actions["TerrainLayerVisibility"];

        DrillingData = DrillingDataManager.DrillingData;

        MaxDepth = DrillingData.Min(data => data.DrillBit_Height) / 1000;
        SetTerrainLayers();
        InitTerrainLayer();
    }

    
    
    

    private void SetTerrainLayers()
    {
        TerrainLayers = new TerrainLayer[transform.childCount - 1]; //6 layers
        MeshRenderers = new MeshRenderer[transform.childCount - 1];
        Transform terrainLayerNames = InfoCanvas.Find("TerrainLayersNames");
        for (int i = 0; i < TerrainLayers.Length; i++)
        {
            Transform layer = transform.GetChild(i);
            TerrainLayers[i] = new TerrainLayer(layer, 0,
                terrainLayerNames.GetChild(i).GetComponent<RectTransform>());
            MeshRenderers[i] = layer.GetComponent<MeshRenderer>();
        }
    }

    
    
    
    
    private void InitTerrainLayer()
    {
        SetTerrainLayerDepth();
        UpdateTerrainLayerDepth();
    }
    private void SetTerrainLayerDepth()
    {
        int layers = 1;
        for (int i = 0; i < DrillingData.Count - 1; i++)
        {
            if (DrillingData[i].Soil_Type != DrillingData[i + 1].Soil_Type)
            {
                TerrainLayers[layers].Depth = DrillingData[i + 1].DrillBit_Height / 1000;
                if (++layers == 6) return;
            }
        }
    }
    private void UpdateTerrainLayerDepth()
    {
        for (int i = 0; i < TerrainLayers.Length; i++)
        {
            TerrainLayer terrainLayer = TerrainLayers[i];
            Transform terrainTransform = terrainLayer.Transform;
            float nextDepth = i == TerrainLayers.Length -1 ? MaxDepth : TerrainLayers[i + 1].Depth;
            float depth = terrainLayer.Depth;
            terrainTransform.localScale = new Vector3(9f, (depth - nextDepth) / 2, 9f);
            terrainTransform.position = Vector3.up * (depth - terrainTransform.localScale.y);

            terrainLayer.TMPName.anchoredPosition = terrainTransform.position * 20f;
        }
    }
    
    
    
    
    
    private void Update()
    {
        if (TerrainLayerVisibilityInputAction.triggered) ChangeTerrainLayerVisibility();
        InfoCanvasLookAtCamera();
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
}