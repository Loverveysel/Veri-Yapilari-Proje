using System.Collections.Generic;

namespace CircularLinkedListSimulator.Models
{

    public class CircularLinkedList
    {
        private Node head;
        private Node tail;
        private int count;

        public CircularLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public List<Node> GetAllNodes()
        {
            var result = new List<Node>();
            if (head == null) return result;

            Node current = head;
            do
            {
                result.Add(current);
                current = current.Next;
            } while (current != head);

            return result;
        }

        public void InsertFront(string data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = tail = newNode;
                newNode.Next = head;
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
            if (head == null)
            {
                head = tail = newNode;
                newNode.Next = head;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
                tail.Next = head;
            }
            count++;
        }

        public void InsertAt(string data, int index)
        {
            if (index < 0 || index > count) return;
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
                current = current.Next;

            newNode.Next = current.Next;
            current.Next = newNode;
            count++;
        }

        public void Remove()
        {
            if (head == null) return;

            if (head == tail)
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

        public void RemoveFront() => Remove();

        public void RemoveBack()
        {
            if (head == null) return;

            if (head == tail)
            {
                head = tail = null;
            }
            else
            {
                Node current = head;
                while (current.Next != tail)
                    current = current.Next;

                current.Next = head;
                tail = current;
            }
            count--;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count) return;

            if (index == 0)
            {
                RemoveFront();
                return;
            }

            Node current = head;
            for (int i = 0; i < index - 1; i++)
                current = current.Next;

            current.Next = current.Next.Next;

            if (index == count - 1)
                tail = current;

            count--;
        }

        public void UpdateData(int index, string newData)
        {
            if (index < 0 || index >= count) return;

            Node current = head;
            for (int i = 0; i < index; i++)
                current = current.Next;

            current.Data = newData;
        }

        public void Clear()
        {
            head = tail = null;
            count = 0;
        }

    }

}
