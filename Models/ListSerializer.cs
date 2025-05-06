using System.Collections.Generic;
using System.Text.Json;

namespace Veri_Yapilari_Proje.Models
{
    public static class ListSerializer
    {
        public static string SerializeSinglyList(SinglyLinkedList list)
        {
            var nodes = new List<Dictionary<string, object>>();
            var current = list.Head; 

            while (current != null)
            {
                var nodeData = new Dictionary<string, object>
                {
                    { "Value", current.Value },
                    { "Next", current.Next != null ? current.Next.Value : (int?)null }
                };

                nodes.Add(nodeData);
                current = current.Next;
            }

            return JsonSerializer.Serialize(nodes);
        }



        public static string SerializeDoublyList(DoublyLinkedList list)
        {
            var nodes = new List<Dictionary<string, object>>();
            var current = list.Head; 

            while (current != null)
            {
                var nodeData = new Dictionary<string, object>
        {
            { "Value", current.Value },
            { "Prev", current.Prev != null ? current.Prev.Value : (int?)null },
            { "Next", current.Next != null ? current.Next.Value : (int?)null }
        };

                nodes.Add(nodeData);
                current = current.Next;
            }

            return JsonSerializer.Serialize(nodes);
        }

       
        public static string SerializeCircularList(CircularLinkedList list)
        {
            var nodes = new List<Dictionary<string, object>>();
            var current = list.Head;

            if (current == null)
                return JsonSerializer.Serialize(nodes); 

            var start = current; 

            do
            {
                var nodeData = new Dictionary<string, object>
        {
            { "Value", current.Value },
            { "Next", current.Next != null ? current.Next.Value : start.Value } // Döngüdeyiz, null olursa head'e dön
        };

                nodes.Add(nodeData);
                current = current.Next;

            } while (current != null && current != start); // Başladığımız yere gelene kadar devam et

            return JsonSerializer.Serialize(nodes);
        }
    }
}
