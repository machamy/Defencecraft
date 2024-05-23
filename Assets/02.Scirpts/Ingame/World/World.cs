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

        
        
        public int Width => width;
        public int Height => height;

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
            
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Tile tile =  Instantiate(tileBase, this.transform).GetComponent<Tile>();
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
    }
}