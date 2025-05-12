using System;

namespace CircularLinkedListVisualizer.Models
{
    public class CircularLinkedList
    {
        public class Node
        {
            public string Data { get; set; }
            public Node Next { get; set; }

            public Node(string data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node head;
        private Node tail;
        private int count;

        public bool IsEmpty => head == null;
        public int Count => count;
        public Node Head => head;

        public void InsertFront(string data)
        {
            Node newNode = new Node(data);

            if (IsEmpty)
            {
                head = tail = newNode;
                head.Next = head;
            }
            else
            {
                newNode.Next = head;
                head = newNode;
                tail.Next = head;
            }
            count++;
        }

        public void InsertBack(string data)
        {
            Node newNode = new Node(data);

            if (IsEmpty)
            {
                head = tail = newNode;
                head.Next = head;
            }
            else
            {
                tail.Next = newNode;
                newNode.Next = head;
                tail = newNode;
            }
            count++;
        }

        public void InsertAt(string data, int index)
        {
            if (index < 0 || index > count)
                throw new IndexOutOfRangeException();

            if (index == 0)
            {
                InsertFront(data);
                return;
            }

            if (index == count)
            {
                InsertBack(data);
                return;
            }

            Node newNode = new Node(data);
            Node current = head;

            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            newNode.Next = current.Next;
            current.Next = newNode;
            count++;
        }

        public void RemoveFront()
        {
            if (IsEmpty) return;

            if (count == 1)
            {
                head = tail = null;
            }
            else
            {
                head = head.Next;
                tail.Next = head;
            }
            count--;
        }

        public void RemoveBack()
        {
            if (IsEmpty) return;

            if (count == 1)
            {
                head = tail = null;
            }
            else
            {
                Node current = head;
                while (current.Next != tail)
                {
                    current = current.Next;
                }

                current.Next = head;
                tail = current;
            }
            count--;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            if (index == 0)
            {
                RemoveFront();
                return;
            }

            if (index == count - 1)
            {
                RemoveBack();
                return;
            }

            Node current = head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            current.Next = current.Next.Next;
            count--;
        }

        public void Clear()
        {
            head = tail = null;
            count = 0;
        }

        public string GetDataAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            Node current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            return current.Data;
        }

        public void UpdateData(int index, string newData)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            Node current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            current.Data = newData;
        }
    }
}