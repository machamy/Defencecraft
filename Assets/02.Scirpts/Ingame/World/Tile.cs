using _02.Scirpts.Ingame.Entity;

namespace _02.Scirpts.Ingame
{
    public class Tile
    {
        private int i, j;
        private bool isConstructable;

        /// <summary>
        /// 건축가능 여부
        /// </summary>
        public bool IsConstructable => isConstructable || Construct != null;

        public AbstractConstruct Construct;
    }
}