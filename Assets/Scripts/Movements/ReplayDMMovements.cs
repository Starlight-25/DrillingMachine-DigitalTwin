using UnityEngine;

public class ReplayDMMovements : MonoBehaviour
{
    [SerializeField] private Transform DLT_B;
    [SerializeField] private Transform DLT_C;
    [SerializeField] private Transform[] DM;
    [SerializeField] private DrillingDataManager DrillingDataManager;
    
    private const float EquipmentStarPos = 40f;


    
    
    
    private void Start()
    {
        InitEquipmentPostition();
    }

    
    
    
    
    private void InitEquipmentPostition()
    {
        DLT_B.position = Vector3.up * EquipmentStarPos;
        DLT_C.position = Vector3.up * EquipmentStarPos;
        foreach (Transform eq in DM)
            eq.position += Vector3.up * EquipmentStarPos;;
    }
}