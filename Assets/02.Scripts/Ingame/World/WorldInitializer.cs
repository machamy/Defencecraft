using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.World
{
    [ExecuteInEditMode]
    public class WorldInitializer : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject tilePrefab;
        [Header("TileData")]
        [SerializeField] private World world;
        [SerializeField] private Tilemap buildableTilemap; // 건설 가능 여부 Tilemap
        [SerializeField] private Tilemap visualTilemap; // 타일 외형Tilemap

        [SerializeField] private TileBase buildableTile;
        [SerializeField] private TileBase unbuildableTile; 
        // [SerializeField] private TileBase voidTile;
        [SerializeField] private TileBase nexusTile; 

        public void LoadTilemap()
        {
            print($"Start LoadTilemap");
            if (buildableTilemap == null || visualTilemap == null)
            {
                Debug.LogError("Tilemaps are not assigned.");
                return;
            }

            Vector2Int gridSize = world.gridSize;
            Tile[][] tiles = new Tile[gridSize.x][];
            for (int index = 0; index < gridSize.x; index++)
            {
                tiles[index] = new Tile[gridSize.y];
            }

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3Int tilemapPosition = new Vector3Int(x, y, 0);
                    TileBase buildableBase = buildableTilemap.GetTile(tilemapPosition);
                    TileBase visualBase = visualTilemap.GetTile(tilemapPosition);
                    Vector3 worldPosition = buildableTilemap.GetCellCenterWorld(tilemapPosition);

                    // 타일 오브젝트 생성 및 초기화
                    Tile tile = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform).GetComponent<Tile>();
                    tile.Initialize(new Vector2Int(x, y));
                    tile.name = $"Tile_{x}_{y}";
                    // 건설 가능 여부 설정
                    if (buildableBase == buildableTile)
                    {
                        tile.SetBuildable(true);
                    }
                    else if (buildableBase == unbuildableTile)
                    {
                        tile.SetBuildable(false);
                    }
                    else if (buildableBase == nexusTile)
                    {
                        tile.name = $"Nexus_{x}_{y}";
                        tile.SetBuildable(false);
                        tile.SetWalkable(false);
                    }else{
                        tile.SetBuildable(false);
                        tile.SetWalkable(false);
                    }
                    

                    tiles[x][y] = tile;
                }
            }

            world.SetTiles(tiles);
        }

#if UNITY_EDITOR

        public void Initialize()
        {
            if (world != null)
            {
                Clear();
                LoadTilemap();
            }
            else
            {
                Debug.LogError("World 없음");
            }
        }
        
        public void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
#endif
    }
}
