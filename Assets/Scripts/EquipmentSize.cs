using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSizeWxH", menuName = "ScriptableObjects/EquipmentSize", order = 1)]
public class EquipmentSize : ScriptableObject
{
    [SerializeField] private Vector2Int gridSize = Vector2Int.zero;
    [SerializeField] private Vector2 colliderSize = Vector2.zero;
    [SerializeField] private Vector2 colliderOffset = Vector2.zero;

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
