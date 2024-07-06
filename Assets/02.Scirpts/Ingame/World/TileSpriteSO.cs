using UnityEngine;

namespace _02.Scirpts.Ingame
{
    [CreateAssetMenu(menuName = "TileSprites")]
    public class TileSpriteSO: ScriptableObject
    {
        public Sprite Defualt;
        public Sprite Down;
        public Sprite Up;
        public Sprite Left;
        public Sprite Right;

        public Sprite GetSprite(Direction dir) => dir switch
        {
            Direction.None => Defualt,
            Direction.Down => Down,
            Direction.Up => Up,
            Direction.Left => Left,
            Direction.Right => Right,
            _ => Defualt
        };
    }
}