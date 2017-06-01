using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities
{
    public class Bag<T> : IEnumerable<T>
    {
        private class Node
        {
            public T item { get; set; }
            public Node next { get; set; }
        }
        private Node first;
        private int N;
        public Bag() { N = 0; }
        public void Add(T item)
        {
            Node oldFirst = first;
            first = new Node();
            first.item = item;
            first.next = oldFirst;
            N++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] arr = new T[N];
            Node current = first;
            int i = 0;
            while (current != null)
            {
                arr[i] = current.item;
                current = current.next;
                i++;
            }
            return arr.Take(N).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
