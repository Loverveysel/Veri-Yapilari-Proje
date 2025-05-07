using System;

namespace Veri_Yapilari_Proje.Models
{
    public class SinglyLinkedList
    {
        public Node Head { get; set; }

        public SinglyLinkedList()
        {
            Head = null;
        }
        public void Add(int value)
        {
            Node newNode = new Node(value);

            if (Head == null)
            {
                Head = newNode;
            }
            else
            {
                Node current = Head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }


        public void Remove(int value)
        {
            if (Head == null)
            {
                Console.WriteLine("Liste boş, silinecek bir şey yok.");
                return;
            }

            // Eğer silinecek düğüm Head ise
            if (Head.Value == value)
            {
                Head = Head.Next;
                return;
            }

            Node current = Head;

            while (current.Next != null && current.Next.Value != value)
            {
                current = current.Next;
            }

            if (current.Next == null)
            {
                Console.WriteLine("Değer listede bulunamadı.");
            }
            else
            {
                current.Next = current.Next.Next;
            }
        }


        public Node Search(int value)
        {
            Node current = Head;

            while (current != null)
            {
                if (current.Value == value)
                {
                    return current;
                }
                current = current.Next;
            }

            return null; // bulunamazsa
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





