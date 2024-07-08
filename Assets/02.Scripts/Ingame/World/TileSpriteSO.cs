using _02.Scirpts.Dictionary;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _02.Scirpts.Ingame
{
    [CreateAssetMenu(menuName = "TileSprites")]
    public class TileSpriteSO: ScriptableObject
    {
        public TileBase TileDefualt;
        public TileBase TileDown;
        public TileBase TileUp;
        public TileBase TileLeft;
        public TileBase TileRight;
        public Sprite Defualt;
        public Sprite Down;
        public Sprite Up;
        public Sprite Left;
        public Sprite Right;

        public Sprite GetSprite(Direction dir) => dir switch
        {
            Direction.All => Defualt,
            Direction.Down => Down,
            Direction.Up => Up,
            Direction.Left => Left,
            Direction.Right => Right,
            Direction.None => null,
            _ => Defualt
        };
        public Direction GetDir(TileBase tile)
        {
            if (tile == TileDefualt)
                return Direction.All;
            if (tile == TileDown)
                return Direction.Down;
            if (tile == TileUp)
                return Direction.Up;
            if (tile == TileLeft)
                return Direction.Left;
            if (tile == TileRight)
                return Direction.Right;
            return Direction.None;
        }
    }
}