using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size; // 건물의 크기 (예: 2x2, 3x3)
    public Vector2Int position; // 건물이 배치된 좌표

    public void Initialize(Vector2Int position, Vector2Int size)
    {
        this.position = position;
        this.size = size;
    }
}