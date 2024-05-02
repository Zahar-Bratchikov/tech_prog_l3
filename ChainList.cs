using System;

namespace lab3
{
    public class ChainList<T> : BaseList<T> where T : IComparable<T>
    {
        public class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node head;


        public ChainList()
        {
            head = null;
            count = 0;
        }

        public Node NodeFind(int pos)
        {
            if (pos >= count) return null;
            int i = 0;
            Node P = head;
            while (P != null && i < pos)
            {
                P = P.Next;
                i++;
            }
            if (i == pos) return P;
            else return null;
        }

        public override void Add(T item)
        {
            if (head == null)
            {
                head = new Node(item);
            }
            else
            {
                Node lastNode = NodeFind(count - 1);
                lastNode.Next = new Node(item);
            }
            count++;
            OnChange();
        }

        public override T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new BadIndexException();
                }

                Node current = NodeFind(index);
                return current.Data;
            }
            set
            {
                if (index < 0 || index >= count)
                {
                    throw new BadIndexException();
                }

                Node current = NodeFind(index);
                current.Data = value;
            }
        }

        public override void Insert(int pos, T item)
        {
            if (pos < 0 || pos > count)
            {
                throw new BadIndexException();
            }

            if (pos == 0)
            {
                Node newNode = new Node(item);
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                Node prevNode = NodeFind(pos - 1);
                Node newNode = new Node(item);
                newNode.Next = prevNode.Next;
                prevNode.Next = newNode;
            }
            count++;
            OnChange();
        }

        public override void Delete(int pos)
        {
            if (pos < 0 || pos >= count)
            {
                throw new BadIndexException();
            }

            if (pos == 0)
            {
                head = head.Next;
            }
            else
            {
                Node prevNode = NodeFind(pos - 1);
                prevNode.Next = prevNode.Next.Next;
            }
            count--;
            OnChange();
        }

        public override void Clear()
        {
            head = null;
            count = 0;
            OnChange();
        }

        public override void Sort()
        {
            if (count <= 1)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Node current = head;

                while (current != null && current.Next != null)
                {
                    if (current.Data.CompareTo(current.Next.Data) > 0)
                    {
                        T temp = current.Data;
                        current.Data = current.Next.Data;
                        current.Next.Data = temp;
                    }
                    current = current.Next;
                }
            }
        }

        protected override BaseList<T> EmptyClone()
        {
            return new ChainList<T>();
        }
    }
}
