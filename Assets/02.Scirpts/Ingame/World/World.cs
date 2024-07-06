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
                   Direction dir;
                    dir = LightTile.GetDir(unityTile);
                    if (dir is not Direction.None)
                    {
                        tile.TileType = LightTile;
                    }
                    else
                   {
                       dir = HeavyTile.GetDir(unityTile);
                       tile.TileType = HeavyTile;
                   }
                   tile.Init(worldPoint, i, j, tileSize,dir,dir == Direction.All ? TileInfo.None : TileInfo.NotConstructable);
                   
                   map[i][j] = tile;
                    
                   tile.name = $"{unityTile?.name} {tile.TileType.name}({i},{j})[direction : {dir}]";
               }
               
           }

        }
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