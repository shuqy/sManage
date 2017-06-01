using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities
{
    public class SymbolGraph
    {
        private Dictionary<string, int> st;//key:名称 value:设置的索引值，从0递增
        private string[] keys;//与st刚好相反，0-n存储对应的名称
        private Graph G;
        public SymbolGraph(IEnumerable<string> list, char sp)
        {
            st = new Dictionary<string, int>();
            foreach (string str in list)
            {
                string[] a = str.Split(sp);
                for (int i = 0; i < a.Length; i++)
                {
                    if (string.IsNullOrEmpty(a[i])) continue;
                    if (!st.ContainsKey(a[i]))
                        st.Add(a[i], st.Count());
                }
            }
            G = new Graph(st.Count());
            keys = new string[st.Count()];
            foreach (var item in st)
                keys[item.Value] = item.Key;
            foreach (string str in list)
            {//构造图
                string[] a = str.Split(sp);
                if (string.IsNullOrEmpty(a[0])) continue;
                for (int i = 1; i < a.Length; i++)
                {
                    if (string.IsNullOrEmpty(a[i])) continue;
                    G.AddEdge(st[a[0]], st[a[i]]);
                }
            }
        }

        public bool Contains(string s) { return st.ContainsKey(s); }
        public int Index(string s) { return st[s]; }
        public string Name(int v) { return keys[v]; }
        public Graph Graph() { return G; }

        public bool HasPath(string from, string to)
        {
            if (!st.ContainsKey(from) || !st.ContainsKey(to)) return false;
            BreadthFirstSearch bfp = new BreadthFirstSearch(G, Index(from));
            return bfp.HasPathTo(Index(to));
        }

        public Queue<string> BreadthFirstPath(string from, string to)
        {
            if (!HasPath(from, to)) return null;
            BreadthFirstSearch bfp = new BreadthFirstSearch(G, Index(from));
            Stack<int> path = bfp.PathTo(Index(to));
            Queue<string> queue = new Queue<string>();
            foreach (int p in path)
            {
                queue.Enqueue(Name(p));
            }
            return queue;
        }
    }
}
