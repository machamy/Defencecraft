using System;

namespace _02.Scirpts.Ingame.Entity
{
    [Serializable]
    public class WorldMemory
    {
        public TileMemory[][] world;
        public int width;
        public int height;
        
        public WorldMemory(int width, int height)
        {
            this.width = width;
            this.height = height;
            for (int i = 0; i < 100; i++)
            {
                world[i] = new TileMemory[100];
            }
        }
    }
    
    [Serializable]
    public struct TileMemory{
        public int x;
        public int y;
        public bool isObstacle;
        public bool isConstruct;
        public int priority;
    }
}