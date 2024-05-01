namespace _02.Scirpts.Ingame
{
    public class World
    {
        private Tile[,] array;

        private int width, height;

        public int Width => width;
        public int Height => height;
        
        public World(int height, int width)
        {
            this.width = width;
            this.height = height;
            
            array = new Tile[height,width];
        }

        public Tile GetTile(int i, int j)
        {
            return array[i,j];
        }
    }
}