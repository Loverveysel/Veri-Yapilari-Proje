using System;

namespace Veri_Yapilari_Proje.Models
{
    public class ListManager
    {
        // Farklı liste türleri için nesneler
        private SinglyLinkedList singlyList = new SinglyLinkedList();
        private DoublyLinkedList doublyList = new DoublyLinkedList();
        private CircularLinkedList circularList = new CircularLinkedList();

        // Kullanıcıdan gelen işlem komutunu çalıştırır
        public void ProcessCommand(string listType, string action, int value)
        {
            switch (listType)
            {
                case "singly":
                    HandleAction(singlyList, action, value);
                    break;
                case "doubly":
                    HandleAction(doublyList, action, value);
                    break;
                case "circular":
                    HandleAction(circularList, action, value);
                    break;
                default:
                    Console.WriteLine("Geçersiz liste türü.");
                    break;
            }
        }

        // İşlem türüne göre ekle, sil, ara, gez fonksiyonlarını çalıştırır
        private void HandleAction(dynamic list, string action, int value)
        {
            switch (action)
            {
                case "add":
                    list.Add(value);
                    break;
                case "remove":
                    list.Remove(value);
                    break;
                case "search":
                    list.Search(value);
                    break;
                case "traverse":
                    list.Traverse();
                    break;
                default:
                    Console.WriteLine("Geçersiz işlem.");
                    break;
            }
        }

        // Listeyi JSON formatına çevirip döndürür
        public string GetListAsJson(string listType)
        {
            switch (listType)
            {
                case "singly":
                    return ListSerializer.SerializeSinglyList(singlyList);
                case "doubly":
                    return ListSerializer.SerializeDoublyList(doublyList);
                case "circular":
                    return ListSerializer.SerializeCircularList(circularList);
                default:
                    return "{}"; // Hatalı tür gelirse boş JSON döndür
            }
        }
    }
}
