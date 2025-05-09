using System;

namespace Veri_Yapilari_Proje.Models
{
    public class DoublyLinkedList
    {
        public Node Head { get; set; }
        public Node Tail { get; set; }

        public DoublyLinkedList()
        {
            Head = null;
            Tail = null;
        }

        public void Add(int value)
        {
            Node newNode = new Node(value);

            if (Head == null)
            {
                Head = newNode;
                Tail = newNode;
            }
            else
            {
                Tail.Next = newNode;
                newNode.Prev = Tail;
                Tail = newNode;
            }
        }

        public void Remove(int value)
        {
            if (Head == null)
            {
                Console.WriteLine("Liste boş.");
                return;
            }

            Node current = Head;

            while (current != null && current.Value != value)
            {
                current = current.Next;
            }

            if (current == null)
            {
                Console.WriteLine("Değer bulunamadı.");
                return;
            }

            if (current == Head)
            {
                Head = Head.Next;
                if (Head != null) Head.Prev = null;
                else Tail = null; // Liste tamamen boşaldı
            }
            else if (current == Tail)
            {
                Tail = Tail.Prev;
                if (Tail != null) Tail.Next = null;
            }
            else
            {
                current.Prev.Next = current.Next;
                current.Next.Prev = current.Prev;
            }
        }

        public Node Search(int value)
        {
            Node current = Head;

            while (current != null)
            {
                if (current.Value == value)
                    return current;

                current = current.Next;
            }

            return null;
        }

        public void Traverse()
        {
            Node current = Head;

            if (current == null)
            {
                Console.WriteLine("Liste boş.");
                return;
            }

            Console.Write("Liste: ");
            while (current != null)
            {
                Console.Write(current.Value + " ");
                current = current.Next;
            }
            Console.WriteLine();
        }
    }
}
