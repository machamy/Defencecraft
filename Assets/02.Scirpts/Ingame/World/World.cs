using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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
        

        private Tile[,] map;

        [Header("맵의 크기")] 
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [Header("타일")]
        [SerializeField] private float tileSize;
        [SerializeField] private GameObject tileBase;
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
            return map[i,j];
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
            map = new Tile[width, height];
            Vector3 worldBottomLeft = transform.position - Vector3.right * (TileSize * width / 2) - Vector3.forward * (TileSize * height / 2);

            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * TileSize + TileSize/2) + Vector3.forward * (y * TileSize + TileSize/2);
                    Tile tile = Instantiate(Tile, worldPoint, Quaternion.identity).GetComponent<Tile>();
                    tile.transform.parent = this.transform;
                    
                    if(x == 1 && y ==5 || x==2 && y ==5 || x ==3 && y== 5)
                    {
                        tile.Init(worldPoint, x, y, tileSize, (TileInfo.Obstacle));

                    }
                    else
                    {
                        tile.Init(worldPoint, x, y, tileSize, (x == 0 || y == 0 ? TileInfo.NotConstructable : TileInfo.None));

                    }

                    map[x, y] = tile;
                    
                    tile.name = $"Tile({x},{y})[C : {tile.IsConstructable}]";
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
                        neighbours.Add(map[checkX,checkY]);
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

            return map[x, y];
        }

        void CheckDebug(bool debug)
        {
            if (map == null)
                return;
            foreach (var tile in map)
            {
                if(tile != null)
                {
                    tile.CheckDebug(debug);
                }
            }
        }

        private void OnValidate()
        {
            CheckDebug(debug);
        }

        
    }
}