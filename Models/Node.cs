namespace LinkedListCircular.Models
{
    public class Node
    {
        public string Data { get; set; }
        public Node Next { get; set; }
        public int Index { get; set; }

        public Node(string data)
        {
            Data = data;
            Next = null;
        }
    }
}