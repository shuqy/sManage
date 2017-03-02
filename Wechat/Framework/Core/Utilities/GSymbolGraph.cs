using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities {
    public class GSymbolGraph<T> {
        private Dictionary<T, int> st;
        private T[] keys;
        private Graph G;
        public GSymbolGraph(IEnumerable<IEnumerable<T>> list) {
            st = new Dictionary<T, int>();
            foreach (IEnumerable<T> items in list) {
                foreach (T item in items) {
                    if (!st.ContainsKey(item))
                        st.Add(item, st.Count());
                }
            }
            keys = new T[st.Count()];
            G = new Graph(st.Count());
            foreach (T itemKey in st.Keys)
                keys[st[itemKey]] = itemKey;
            foreach (IEnumerable<T> items in list) {
                int v = st[items.FirstOrDefault()];
                foreach (T item in items.Skip(1)) {
                    G.AddEdge(v, st[item]);
                }
            }
        }
        public bool Conatins(T s) { return st.ContainsKey(s); }
        public int Index(T s) { return st[s]; }
        public T Name(int v) { return keys[v]; }
        public Graph Graph() { return G; }
    }
}
