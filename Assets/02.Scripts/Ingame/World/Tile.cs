using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int position; // 격자 좌표
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private MeshRenderer _meshRenderer;
    [field:SerializeField]public bool IsBuildable { get; private set; } = true;
    [field:SerializeField]public bool IsWalkable { get; private set; } = true;
    public Building CurrentBuilding { get; private set; } = null;

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="position"></param>
    public void Initialize(Vector2Int position)
    {
        this.position = position;
    }
    
    public void SetBuildable(bool isBuildable)
    {
        IsBuildable = isBuildable;
    }
    
    public void SetWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;
    }
    public void SetBuilding(Building building)
    {
        CurrentBuilding = building;
        IsBuildable = false;
        IsWalkable = false;
    }

}