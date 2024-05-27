using System;
using UnityEngine;

namespace _02.Scirpts.Ingame
{
    
    /// <summary>
    /// 맵의 월드를 관리하는 클래스
    /// </summary>
    public class World : MonoBehaviour
    {
        [Header("디버그")]
        [SerializeField] private bool debug = false;
        

        private Grid _grid;
        private Tile[,] map;

        [Header("맵의 크기")] 
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [Header("타일")]
        [SerializeField] private float tileSize;
        [SerializeField] private GameObject tileBase;
        public GameObject tile { get { return tileBase; } set { tileBase = value; } }


        public int Width => width;
        public int Height => height;

        /// <summary>
        /// 타일의 크기
        /// </summary>
        public float TileSize => tileSize;

        private void Start()
        {
            Initialize();
            CheckDebug(debug);
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

        /// <summary>
        /// 월드 초기화
        /// </summary>
        private void Initialize()
        {
            map = new Tile[width,height];
            Vector3 mapBottomleft = transform.position - Vector3.right * (TileSize * width / 2) - Vector3.forward * (TileSize * height / 2);


            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Vector3 tilePos = mapBottomleft + Vector3.right * (i * TileSize + TileSize/2) + Vector3.forward * (j * TileSize + TileSize/2);
                    
                    Tile tile =  Instantiate(tileBase, this.transform, false).GetComponent<Tile>();
                    
                    map[i, j] = tile;
                    tile.Init(i,j,tileSize, (i == 0 ||  j == 0 ? TileInfo.NotConstructable : TileInfo.None));
                    tile.name = $"Tile({i},{j})[C : {tile.IsConstructable}]";
                }
            }
        }
        
        void CheckDebug(bool debug)
        {
            if (map == null)
                return;
            foreach (var tile in map)
            {
                tile.CheckDebug(debug);
            }
        }

        private void OnValidate()
        {
            CheckDebug(debug);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Vector3.zero + Vector3.right * (TileSize * width - TileSize) / 2  + Vector3.forward * (TileSize * height - TileSize) / 2, new Vector3(TileSize * width, 1, TileSize * height));
            Gizmos.DrawCube(Vector3.zero + Vector3.right * (TileSize * width - TileSize) / 2 + Vector3.forward * (TileSize * height - TileSize) / 2, new Vector3(TileSize * width, 0.1f, TileSize * height));
            /*
            if(map != null)
            {
                foreach(Tile t in map)
                {
                    Gizmos.color = (t.IsWalkable) ? Color.cyan : Color.red;
                    Gizmos.DrawCube(t.transform.position, Vector3.one * (TileSize - .1f));
                }
            }
            else
            {
                Debug.Log("not yet");
            }
            
            */
        }
    }
}