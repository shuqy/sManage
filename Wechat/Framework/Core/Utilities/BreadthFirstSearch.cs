using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities
{
    public class BreadthFirstSearch
    {
        private bool[] marked;
        private int[] edgeTo;
        private readonly int s;
        public BreadthFirstSearch(Graph G, int s)
        {
            marked = new bool[G.V];
            edgeTo = new int[G.V];
            this.s = s;
            bfs(G, s);
        }
        private void bfs(Graph G, int s)
        {
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            marked[s] = true;
            while (queue.Any())
            {
                int v = queue.Dequeue();
                foreach (int w in G.Adj(v))
                {
                    if (!marked[w])
                    {
                        marked[w] = true;
                        edgeTo[w] = v;
                        queue.Enqueue(w);
                    }
                }
            }
        }

        public bool HasPathTo(int v)
        {
            return marked[v];
        }

        public Stack<int> PathTo(int v)
        {
            if (!HasPathTo(v)) return null;
            Stack<int> path = new Stack<int>();
            for (int x = v; x != s; x = edgeTo[x])
                path.Push(x);
            path.Push(s);
            return path;
        }
    }
}
