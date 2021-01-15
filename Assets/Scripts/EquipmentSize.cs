using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSizeWxH", menuName = "ScriptableObjects/EquipmentSize", order = 1)]
public class EquipmentSize : ScriptableObject
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 colliderSize;
    [SerializeField] private Vector2 colliderOffset;

    public Vector2Int GridSize
    {
        get { return gridSize; }
    }

    public Vector2 ColliderSize
    {
        get { return colliderSize; }
    }

    public Vector2 ColliderOffset
    {
        get { return colliderOffset; }
    }
}
