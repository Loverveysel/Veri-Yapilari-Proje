using System;

namespace Veri_Yapilari_Proje.Models
{
    public class CircularLinkedList
    {
        public Node Head { get; set; }

        public CircularLinkedList()
        {
            Head = null;
        }

        public void Add(int value)
        {
            Node newNode = new Node(value);

            if (Head == null)
            {
                Head = newNode;
                Head.Next = Head; // Kendi kendine döngü
            }
            else
            {
                Node current = Head;

                while (current.Next != Head)
                {
                    current = current.Next;
                }

                current.Next = newNode;
                newNode.Next = Head;
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
            Node previous = null;

            do
            {
                if (current.Value == value)
                {
                    if (previous == null)
                    {
                        // Tek elemanlı liste
                        if (current.Next == Head)
                        {
                            Head = null;
                        }
                        else
                        {
                            // Head silinecekse son düğümü bulup onun Next'ini güncelle
                            Node tail = Head;
                            while (tail.Next != Head)
                            {
                                tail = tail.Next;
                            }
                            Head = Head.Next;
                            tail.Next = Head;
                        }
                    }
                    else
                    {
                        previous.Next = current.Next;

                        // Head silinirse güncelle
                        if (current == Head)
                        {
                            Head = current.Next;
                        }
                    }
                    return;
                }

                previous = current;
                current = current.Next;

            } while (current != Head);

            Console.WriteLine("Değer bulunamadı.");
        }

        public Node Search(int value)
        {
            if (Head == null)
                return null;

            Node current = Head;

            do
            {
                if (current.Value == value)
                    return current;

                current = current.Next;

            } while (current != Head);

            return null;
        }

        public void Traverse()
        {
            if (Head == null)
            {
                Console.WriteLine("Liste boş.");
                return;
            }

            Node current = Head;

            Console.Write("Liste: ");

            do
            {
                Console.Write(current.Value + " ");
                current = current.Next;
            } while (current != Head);

            Console.WriteLine();
        }
    }
}
