using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pathfinding_test
{

    class Node
    {
        public bool Passable = true;
        public bool Closed = false;
        public bool Open = false;
        public bool Path = false;

        public int Cost = 1;

        public int i = 0;
        public int j = 0;
        public double f = 0;
        public int g = 0;
        public double h = 0;

        public Node Previous = null;

    }

}
