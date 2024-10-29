using System;
using System.Collections;
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
        [SerializeField] private bool doNotInitialize = false;
        

        private CustomTilemap map;

        [Serializable]
        public class CustomTilemap : ISerializationCallbackReceiver, IEnumerable
        {
            [Serializable]
            class TileRow
            {
                public Tile[] row;
            }
            [SerializeField]
            private TileRow[] rows;

            [SerializeField] private Tile[][] map;

            [SerializeField]
            private int width;
            [SerializeField]
            private int height;

            public CustomTilemap(int width, int height)
            {
                Initialize(width, height);
            }
            
            public void OnBeforeSerialize()
            {
                for (int i = 0; i < width; i++)
                {
                    rows[i].row = map[i];
                }
            }

            public void OnAfterDeserialize()
            {
                for (int i = 0; i < width; i++)
                {
                    map[i] = rows[i].row;
                }
            }

            public void Initialize(int width, int height)
            {
                this.width = width;
                this.height = height;
                map = new Tile[width][];
                for (int i = 0; i < width; i++)
                {
                    map[i] = new Tile[height];
                }
            }

            public Tile this[int i, int j]
            {
                get => map[i][j];
                set => map[i][j] = value;
            }

            public Tile[] this[int i] => map[i];

            public IEnumerator GetEnumerator()
            {
                foreach (var tiles in map)
                {
                    foreach (var tile in tiles)
                    {
                        yield return tile;
                    }
                }
            }
        }

        [Header("맵의 크기")] 
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [Header("타일")]
        [SerializeField] private float tileSize;
        [SerializeField] private GameObject tileBase;
        [SerializeField] private TileSpriteSO LightTile;
        [SerializeField] private TileSpriteSO HeavyTile;
        [Header("넥서스")]
        [SerializeField] private Nexus nexus;
        [Header("스프라이트 타일맵")]
        [SerializeField] private Tilemap mainTileMap;
        [Header("속성 타일맵")]
        [SerializeField] private Tilemap subTileMap;

        [SerializeField] private TileBase ConstructableUnityTile;
        [SerializeField] private TileBase NotConstructableUnityTile;
        [SerializeField] private TileBase NexusUnityTile;
        public GameObject Tile { get { return tileBase; } set { tileBase = value; } }


        public int Width => width;
        public int Height => height;

        /// <summary>
        /// 타일의 크기
        /// </summary>
        public float TileSize => tileSize;

        private void Awake()
        {
            if(!doNotInitialize)
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
            return map[i,j];
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
            map = new CustomTilemap(width,height);

            Tile nexusTile = null;
            // mainTileMap.CellToWorld
            for (int i = 0; i < height; i++)
            {
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
                    if(info == TileInfo.Nexus)
                    {
                        nexus = Instantiate(nexus,worldPoint,Quaternion.identity);
                        nexus.transform.parent = this.transform;
                        nexusTile = tile;
                    }

                    info = TileInfo.None;
                    
                    tile.Init(worldPoint, i, j, tileSize,type,dir,info);
                    
                       
                    map[i,j] = tile;
                        
                    tile.name = $"{unityTile?.name} {tile.TileType.name}({i},{j})[{info}||direction : {dir}]";
                }
            }

            if (nexusTile != null)
            {
                GetNeighbours(nexusTile);
            }
            else
            {
                Debug.LogError("No Nexus");
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
            if (unitySubTile == NotConstructableUnityTile)
                return TileInfo.NotConstructable;
            if (unitySubTile == NexusUnityTile)
                return TileInfo.Nexus;
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
                        neighbours.Add(map[checkX,checkY]);
                    }
                }
            }
            return neighbours;
        }
        public Tile TileFromWorldPoint(Vector3 worldPosition)
        {
            // float percentX = (worldPosition.x) / ((width - 1) * TileSize);
            // float percentY = (worldPosition.z) / ((height - 1) * TileSize);
            //
            // percentX = Mathf.Clamp01(percentX);
            // percentY = Mathf.Clamp01(percentY);
            //
            // int x = Mathf.RoundToInt((width - 1) * percentX);
            // int y = Mathf.RoundToInt((height - 1) * percentY);
            // Debug.Log(x+ ", "+y);
            //
            // return map[x][y];
            var cell = mainTileMap.WorldToCell(worldPosition);
            return map[cell.x,cell.y];
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