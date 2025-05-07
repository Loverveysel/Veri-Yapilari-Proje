namespace Veri_Yapilari_Proje.Models
{
    public class Node
    {
        public int Value { get; set; }       // Düğümün taşıdığı değer
        public Node Next { get; set; }       // Sonraki düğüme bağlantı (tek, çift, döngüsel için)
        public Node Prev { get; set; }       // Önceki düğüme bağlantı (çift yönlü için)

        public Node(int value)
        {
            Value = value;
            Next = null;
            Prev = null;
        }
    }
}
