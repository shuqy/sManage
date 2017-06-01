
namespace Core.Utilities
{
    public class Graph
    {
        private int _v;//顶点
        private int _e;//边
        private Bag<int>[] adj;//邻接边

        public Graph(int v)
        {
            _v = v;
            _e = 0;
            adj = new Bag<int>[v];
            for (int i = 0; i < v; i++)
            {
                adj[i] = new Bag<int>();
            }
        }

        public void AddEdge(int v, int w)
        {
            adj[v].Add(w);
            adj[w].Add(v);
            _e++;
        }

        public int V { get { return _v; } }
        public int E { get { return _e; } }

        public Bag<int> Adj(int v)
        {
            return adj[v];
        }
    }
}
