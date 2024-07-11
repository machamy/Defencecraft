using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

namespace _02.Scirpts.Ingame
{
    
    /// <summary>
    /// 맵의 월드를 관리하는 클래스
    /// </summary>
    public class World : MonoBehaviour
    {
        [Header("디버그")]
        [SerializeField] private bool debug = false;
        

        private Tile[][] map;

        [Header("맵의 크기")] 
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [Header("타일")]
        [SerializeField] private float tileSize;
        [SerializeField] private GameObject tileBase;
        [SerializeField] private TileSpriteSO LightTile;
        [SerializeField] private TileSpriteSO HeavyTile;
        [Header("스프라이트 타일맵")]
        [SerializeField] private Tilemap mainTileMap;
        [Header("속성 타일맵")]
        [SerializeField] private Tilemap subTileMap;

        [SerializeField] private TileBase ConstructableUnityTile;
        [SerializeField] private TileBase NotConstructableUnityTile;
        public GameObject Tile { get { return tileBase; } set { tileBase = value; } }


        public int Width => width;
        public int Height => height;

        /// <summary>
        /// 타일의 크기
        /// </summary>
        public float TileSize => tileSize;

        private void Awake()
        {
            Initialize();
            CheckDebug(debug);
        }

        private void Start()
        {
            GameManager.Instance.world = this;
        }

        void Update()
        {
            
        }

        /// <summary>
        /// 해당 좌표의 타일을 가져온다.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>해당 타일</returns>
        public Tile GetTile(int i, int j)
        {
            return map[i][j];
        }

        public int MaxSize
        {
            get
            {
                return width * height;
            }
            
        }

        #region init
        /// <summary>
        /// 월드 초기화
        /// </summary>
        void Initialize()
        {
            map = new Tile[height][];
            // mainTileMap.CellToWorld
            for (int i = 0; i < height; i++)
            {
                map[i] = new Tile[width];
                for (int j = 0; j < width; j++)
                {
                    Vector3Int cell = new Vector3Int(i,j);
                    TileBase unityTile =  mainTileMap.GetTile(cell);
                    Vector3 worldPoint = mainTileMap.GetCellCenterWorld(cell);
                    Tile tile = Instantiate(Tile,worldPoint, Quaternion.identity).GetComponent<Tile>();
                    tile.transform.parent = this.transform;
                   
                    // 방향 받아오기
                    Direction dir;
                    TileSpriteSO type;
                    GetTileDirenctionType(unityTile,out dir,out type);

                    //건설가능여부 받아오기
                    TileBase unitySubTile =  subTileMap.GetTile(cell);
                    TileInfo info = GetTileInfo(unitySubTile);
                    
                    tile.Init(worldPoint, i, j, tileSize,type,dir,info);
                    
                       
                    map[i][j] = tile;
                        
                    tile.name = $"{unityTile?.name} {tile.TileType.name}({i},{j})[{info}||direction : {dir}]";
                }
            }
        }

        /// <summary>
        /// 방향과 종류를 받아옴
        /// </summary>
        /// <param name="unityTile"></param>
        /// <param name="direction"></param>
        /// <param name="tileType"></param>
        /// <returns></returns>
        private bool GetTileDirenctionType(TileBase unityTile, out Direction direction, out TileSpriteSO tileType)
        {
            direction = LightTile.GetDir(unityTile);
            if (direction is not Direction.None)
            {
                tileType = LightTile;
            }
            else if((direction = HeavyTile.GetDir(unityTile)) is not Direction.None)
            {
                direction = HeavyTile.GetDir(unityTile);
                tileType = HeavyTile;
            }
            else // 해당사항을 못찾음
            {
                tileType = HeavyTile;
                direction = Direction.None;
                return false;
            }
            return true;
        }

        private TileInfo GetTileInfo(TileBase unitySubTile)
        {
            if (unitySubTile == ConstructableUnityTile)
                return TileInfo.None;
            else if (unitySubTile == NotConstructableUnityTile)
                return TileInfo.NotConstructable;
            return TileInfo.Void;
        }

        #endregion
        
        public List<Tile> GetNeighbours(Tile tile)
        {
            List<Tile> neighbours = new List<Tile>();
            
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++) { 
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = tile.getTileX() + x;
                    int checkY = tile.getTileY() + y;

                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                    {
                        neighbours.Add(map[checkX][checkY]);
                    }
                }
            }
            return neighbours;
        }
        public Tile TileFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x) / ((width - 1) * TileSize);
            float percentY = (worldPosition.z) / ((height - 1) * TileSize);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((width - 1) * percentX);
            int y = Mathf.RoundToInt((height - 1) * percentY);
            Debug.Log(x+ ", "+y);

            return map[x][y];
        }

        void CheckDebug(bool debug)
        {
            if (map == null)
                return;
            foreach (var tile in map)
            {
                if(tile != null)
                {
                    // tile.CheckDebug(debug);
                }
            }
        }

        private void OnValidate()
        {
            CheckDebug(debug);
        }

        
    }
}