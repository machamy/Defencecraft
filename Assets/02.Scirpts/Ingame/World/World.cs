using System;
using UnityEngine;

namespace _02.Scirpts.Ingame
{
    public class World : MonoBehaviour
    {
        private Tile[,] map;

        [SerializeField]
        private int width = 10, height = 10;

        [SerializeField] private float tileSize;
        [SerializeField] private GameObject tileBase;

        public int Width => width;
        public int Height => height;

        public float TileSize => tileSize;

        private void Start()
        {
            Initialize();
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
                    tile.Init(i,j,tileSize);
                    tile.name = $"Tile({i},{j})[C : {tile.IsConstructable}]";
                }
            }
        }
    }
}