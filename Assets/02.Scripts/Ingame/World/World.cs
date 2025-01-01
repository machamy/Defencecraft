using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.World
{
    [ExecuteInEditMode]
    public class World : MonoBehaviour
    {
        public Vector2Int gridSize;
        public Tile[][] tiles;

        public void SetTiles(in Tile[][] tiles)
        {
            this.tiles = new Tile[gridSize.x][];
            for (int x = 0; x < gridSize.x; x++)
            {
                this.tiles[x] = new Tile[gridSize.y];
                for (int y = 0; y < gridSize.y; y++)
                {
                    this.tiles[x][y] = tiles[x][y];
                }
            }
        }

        public Tile GetTile(in int x,in int y)
        {
            if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y) return null;
            return tiles[x][y];
        }
        public Tile GetTile(in Vector2Int position)
        {
            return GetTile(position.x, position.y);
        }
        
        
        public bool CanPlaceBuilding(in Vector2Int position,in Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var tile = GetTile(position + new Vector2Int(x, y));
                    if (tile == null || !tile.IsBuildable) return false;
                }
            }
            return true;
        }
        
        public bool PlaceBuilding(in Building building,in Vector2Int position)
        {
            if (!CanPlaceBuilding(position, building.size)) return false;

            for (int x = 0; x < building.size.x; x++)
            {
                for (int y = 0; y < building.size.y; y++)
                {
                    var tile = GetTile(position + new Vector2Int(x, y));
                    tile.SetBuilding(building);
                }
            }
            return true;
        }
        
        

    }

}
