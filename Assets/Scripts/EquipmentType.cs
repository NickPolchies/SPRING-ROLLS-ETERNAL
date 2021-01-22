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

    [SerializeField] private EquipmentSize size;
    [SerializeField] private GameObject graphics;

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

    public EquipmentSize Size
    {
        get { return size; }
    }

    public GameObject Graphics
    {
        get { return graphics; }
    }
}
