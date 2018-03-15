using System;

namespace pathfinding_test
{
    class Grid
    {

        public int Width = 100;
        public int Height = 50;
        private Node[,] nodes;

        public Grid()
        {
            Random rand = new Random();
            nodes = new Node[Width, Height];
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    nodes[i, j] = new Node();
                    nodes[i, j].i = i;
                    nodes[i, j].j = j;
                    if ((i != 0 && j != 0) &&
                        rand.Next(0, 100) < 25)
                    {
                        nodes[i, j].Passable = false;
                    }

                    if (i > Width/4 && i < 7 * Width / 8 && j == Height/2)
                    {
                        nodes[i, j].Passable = false;
                    }
                    if (i == 7*Width/8 && j < 7*Height/8  && j > Height/4 )
                    {
                        nodes[i, j].Passable = false;
                    }
                }
        }

        public Node Node(int i, int j)
        {
            if (i >= 0 && i < Width && j >= 0 && j < Height)
                return nodes[i, j];
            else
                return null;
        }

        public void ResetState()
        {
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    nodes[i, j].f =  int.MaxValue;
                    nodes[i, j].g = 0;
                    nodes[i, j].h = 0;
                    nodes[i, j].Previous = null;
                    nodes[i, j].Path = false;
                    nodes[i, j].Open = false;
                    nodes[i, j].Closed = false;
                }
        }
    }
}
