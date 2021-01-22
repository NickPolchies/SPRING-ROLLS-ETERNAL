using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentType", menuName = "ScriptableObjects/EquipmentType", order = 1)]
public class EquipmentType : ScriptableObject
{
    [SerializeField] private string typeName;
    [SerializeField] private int cost;
    [SerializeField] private float cashFlow;
    [SerializeField] private float heat;
    [SerializeField] private int power;
    [SerializeField] private bool roof;

    [Space(10)]

    [SerializeField] private float cashFlowScaling;
    [SerializeField] private float heatScaling;
    [SerializeField] private int powerScaling;

    [Space(10)]

    [SerializeField] private EquipmentSize size;
    [SerializeField] private GameObject graphics;

    #region Properties
    public string TypeName
    {
        get { return typeName; }
    }
    public int Cost
    {
        get { return cost; }
    }
    public float CashFlow
    {
        get { return cashFlow; }
    }
    public float Heat
    {
        get { return heat; }
    }
    public int Power
    {
        get { return power; }
    }
    public bool Roof
    {
        get { return roof; }
    }
    #endregion

    #region Scaling Properties
    public float CashFlowScaling
    {
        get { return cashFlowScaling; }
    }

    public float HeatScaling
    {
        get { return heatScaling; }
    }

    public int PowerScaling
    {
        get { return powerScaling; }
    }
    #endregion

    #region Objects
    public EquipmentSize Size
    {
        get { return size; }
    }

    public GameObject Graphics
    {
        get { return graphics; }
    }
    #endregion
}
