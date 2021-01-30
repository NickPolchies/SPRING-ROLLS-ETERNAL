using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentType", menuName = "ScriptableObjects/EquipmentType", order = 1)]
public class EquipmentType : ScriptableObject
{
    //Properties
    [SerializeField] private string typeName = "";
    [SerializeField] private int cost = 0;
    [SerializeField] private float cashFlow = 0;
    [SerializeField] private float heat = 0;
    [SerializeField] private int power = 0;
    [SerializeField] private bool roof = false;
    [SerializeField] private bool solar = false;

    [Space(10)]

    //Scaling Properties
    [SerializeField] private float cashFlowScaling = 0;
    [SerializeField] private float heatScaling = 0;
    [SerializeField] private int powerScaling = 0;

    [Space(10)]

    //Objects
    [SerializeField] private EquipmentSize size = null;
    [SerializeField] private GameObject graphics = null;

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

    public bool Solar
    {
        get { return solar; }
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
