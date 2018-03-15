using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pathfinding_test
{
    enum Heuristics
    {
        Euclid,
        Manhatten,
        Dijkstra
    }


    class AStar
    {
        private List<Node> closedList = new List<Node>();
        private List<Node> openList = new List<Node>();

        bool finished = true;
        Heuristics h;

        public AStar(Heuristics h = Heuristics.Euclid)
        {
            this.h = h;
        }

        public void Start(Node start)
        {
            if (finished)
            {
                openList.Clear();
                closedList.Clear();
                openList.Add(start);
                finished = false;
            }
        }

        private Node[] GetNeighbors(Grid grid, Node node)
        {
            int i = node.i;
            int j = node.j;

            int w = grid.Width;
            int h = grid.Height;

            Node[] neighbors = { null, null, null, null, null, null, null, null };
            if (j - 1 > 0)
            {
                neighbors[0] = grid.Node(i, j - 1);
            }
            if (j + 1 < h)
            {
                neighbors[1] = grid.Node(i, j + 1);
            }
            if (i - 1 > 0)
            {
                neighbors[2] = grid.Node(i - 1, j);
            }
            if (i + 1 < w)
            {
                neighbors[3] = grid.Node(i + 1, j);
            }

            if (j - 1 > 0 && i - 1 > 0)
            {
                neighbors[4] = grid.Node(i - 1, j - 1);
            }
            if (i + 1 < w && j + 1 < h)
            {
                neighbors[5] = grid.Node(i + 1, j + 1);
            }
            if (i - 1 > 0 && j + 1 < h)
            {
                neighbors[6] = grid.Node(i - 1, j + 1);
            }
            if (i + 1 < w && j - 1 > 0)
            {
                neighbors[7] = grid.Node(i + 1, j - 1);
            }
            return neighbors;
    }


        private void UpdateNeighbors(ref Node current, Grid grid, Node end)
        {

            Node[] neighbors = GetNeighbors(grid, current);

            foreach (Node neighbor in neighbors)
            {
                if (neighbor == null)
                {
                    continue;
                }
                else if (!neighbor.Passable)
                {
                    continue;
                }
                else if (closedList.Contains(neighbor))
                {
                    continue;
                }
                else 
                {
                    int tempG = current.g + neighbor.Cost;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    else if (tempG >= neighbor.g)
                    {
                        // not a better path
                        continue;
                    }

                    neighbor.g = tempG;

                    neighbor.h = this.h == Heuristics.Euclid ? EuclidDistance(neighbor.i, neighbor.j, end.i, end.j) : 0;
                    neighbor.f = neighbor.g + neighbor.h;

                    neighbor.Previous = current;
                }
            }
        }

        // Heuristics
        private int EuclidDistance(int x0, int y0, int x1, int y1)
        {
            int x = Math.Abs(x1 - x0);
            int y = Math.Abs(y1 - y0);
            return (int)Math.Sqrt(x * x + y * y);
        }
        private int ManhattenDistance(int x0, int y0, int x1, int y1)
        {
            return Math.Abs(x1 - x0) + Math.Abs(y1 - y0);
        }

        private int Dijkstra(int x0, int y0, int x1, int y1)
        {
            return 0;
        }

        public void Search(Grid grid, Node start, Node end, Stack<Node> path)
        {

            start.f = this.h == Heuristics.Euclid ? EuclidDistance(start.i, start.j, end.i, end.j) : 0;

            while (!finished)
            {
                // find the node in open list with the lowest f
                int lowestIndex = 0;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].f < openList[lowestIndex].f)
                    {
                        lowestIndex = i;
                    }

                    if (openList[i].f == openList[lowestIndex].f)
                    {
                        if (openList[i].g > openList[lowestIndex].g)
                        {
                            lowestIndex = i;
                        }
                    }
                }


                // node with lowest f becomes the current node
                Node current = openList[lowestIndex];
                if (current == end)
                {
                    Finish(current, path);
                }

                // could use better data structures for these
                openList.Remove(current);
                closedList.Add(current);

                current.Open = false;
                current.Closed = true;

                UpdateNeighbors(ref current, grid, end);


                // solution not found, return path to node with greatest g value
                // 
                if (closedList.Count > (grid.Width*grid.Height) / 4)
                {
                    int greatestG = 0;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList[i].g > openList[greatestG].g)
                        {
                            greatestG = i;
                        }

                    }
                    current = openList[greatestG];
                    Finish(current, path);

                    return;
                }

            }

        }
        /*
                public void Search(Node[,] grid, Node start, Node end, Stack<Node> path)
                {

                    start.f = this.h == Heuristics.Euclid ? EuclidDistance(start.i, start.j, end.i, end.j) : 0;

                    while (!finished)
                    {
                        // find the node in open list with the lowest f
                        int lowestIndex = 0;
                        for (int i = 0; i < openList.Count; i++)
                        {
                            if (openList[i].f < openList[lowestIndex].f)
                            {
                                lowestIndex = i;
                            }

                            if (openList[i].f == openList[lowestIndex].f)
                            {
                                if (openList[i].g > openList[lowestIndex].g)
                                {
                                    lowestIndex = i;
                                }
                            }
                        }


                        // node with lowest f becomes the current node
                        Node current = openList[lowestIndex];
                        if (current == end)
                        {
                            Finish(current, path);
                        }

                        // could use better data structures for these
                        openList.Remove(current);
                        closedList.Add(current);

                        current.Open = false;
                        current.Closed = true;

                        UpdateNeighbors(ref current, grid, end);


                        // solution not found, return path to node with greatest g value
                        // 
                        if (closedList.Count > grid.Length / 4)
                        {
                            int greatestG = 0;
                            for (int i = 0; i < openList.Count; i++)
                            {
                                if (openList[i].g > openList[greatestG].g)
                                {
                                    greatestG = i;
                                }

                            }
                            current = openList[greatestG];
                            Finish(current, path);

                            return;
                        } 

                    }

                }
        */
        private void Finish(Node current, Stack<Node> path)
        {
            while (current.Previous != null)
            {
                current.Path = true;
                path.Push(current);
                current = current.Previous;
            }
            openList.Clear();
            closedList.Clear();
            finished = true;
        }

        /*
        public void SearchStep(Node[,] grid, Node start, Node end, ref Node current)
        {
            if (!finished)
            {
                // find the node in open list with the lowest f
                int lowestIndex = 0;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].f < openList[lowestIndex].f)
                    {
                        lowestIndex = i;
                    }
                }

                // node with lowest f becomes the current node
                current = openList[lowestIndex];

                if (current == end)
                {
                    // done
                    while (current.Previous != null)
                    {
                        current.Path = true;
                        current = current.Previous;
                    }

                    finished = true;
                    foreach (Node n in openList)
                    {
                        n.Open = false;
                    }
                    foreach (Node n in closedList)
                    {
                        n.Closed = false;
                    }
                    openList.Clear();
                    closedList.Clear();
                }
                openList.Remove(current);
                closedList.Add(current);
                current.Open = false;
                current.Closed = true;

                UpdateNeighbors(ref current, grid, end);

            }
            else
            {
                return;
                // no solution
            }
        }
        */
    }
}
