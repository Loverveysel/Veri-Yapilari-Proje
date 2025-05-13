using System.Text.Json;

namespace LinkedListCircular.Models
{
    public class CircularLinkedList
    {
        public Node Head { get; private set; }
        public Node Tail { get; private set; }
        public int Count { get; private set; }

        public void InsertFront(string data)
        {
            Node newNode = new Node(data);
            if (Head == null)
            {
                Head = Tail = newNode;
                newNode.Next = Head;
            }
            else
            {
                newNode.Next = Head;
                Head = newNode;
                Tail.Next = Head;
            }
            Count++;
            UpdateIndexes();
        }

        public void InsertBack(string data)
        {
            Node newNode = new Node(data);
            if (Head == null)
            {
                Head = Tail = newNode;
                newNode.Next = Head;
            }
            else
            {
                Tail.Next = newNode;
                Tail = newNode;
                Tail.Next = Head;
            }
            Count++;
            UpdateIndexes();
        }

        public void InsertAt(string data, int index)
        {
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException("Index out of range");

            if (index == 0)
            {
                InsertFront(data);
                return;
            }

            if (index == Count)
            {
                InsertBack(data);
                return;
            }

            Node newNode = new Node(data);
            Node current = Head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }
            newNode.Next = current.Next;
            current.Next = newNode;
            Count++;
            UpdateIndexes();
        }

        public void RemoveFront()
        {
            if (Head == null) return;

            if (Head == Tail)
            {
                Head = Tail = null;
            }
            else
            {
                Head = Head.Next;
                Tail.Next = Head;
            }
            Count--;
            UpdateIndexes();
        }

        public void RemoveBack()
        {
            if (Head == null) return;

            if (Head == Tail)
            {
                Head = Tail = null;
            }
            else
            {
                Node current = Head;
                while (current.Next != Tail)
                {
                    current = current.Next;
                }
                Tail = current;
                Tail.Next = Head;
            }
            Count--;
            UpdateIndexes();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("Index out of range");

            if (index == 0)
            {
                RemoveFront();
                return;
            }

            if (index == Count - 1)
            {
                RemoveBack();
                return;
            }

            Node current = Head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }
            current.Next = current.Next.Next;
            Count--;
            UpdateIndexes();
        }

        public void UpdateData(int index, string newData)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("Index out of range");

            Node current = Head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            current.Data = newData;
        }

        public string Search(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("Index out of range");

            Node current = Head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            return current.Data;
        }

        public void Clear()
        {
            Head = Tail = null;
            Count = 0;
        }

        private void UpdateIndexes()
        {
            if (Head == null) return;

            Node current = Head;
            int index = 0;
            do
            {
                current.Index = index++;
                current = current.Next;
            } while (current != Head);
        }

        public string ToJson()
        {
            var nodes = new List<object>();
            if (Head != null)
            {
                Node current = Head;
                do
                {
                    nodes.Add(new
                    {
                        data = current.Data,
                        index = current.Index,
                        nextIndex = current.Next?.Index
                    });
                    current = current.Next;
                } while (current != Head);
            }
            return JsonSerializer.Serialize(nodes);
        }
    }
}