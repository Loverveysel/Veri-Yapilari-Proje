using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using CircularLinkedListSimulator.Models;

namespace CircularLinkedListSimulator.Pages
{
    public class IndexModel : PageModel
    {
        private static CircularLinkedList LinkedList = new CircularLinkedList();

        [BindProperty]
        public string Operation { get; set; }

        [BindProperty]
        public string SubOperation { get; set; }

        [BindProperty]
        public string Data { get; set; }

        [BindProperty]
        public int Index { get; set; }

        public JsonResult OnPostExecute()
        {
            switch (SubOperation.ToLower())
            {
                case "insertfront":
                    LinkedList.InsertFront(Data);
                    break;
                case "insertback":
                    LinkedList.InsertBack(Data);
                    break;
                case "insertat":
                    LinkedList.InsertAt(Data, Index);
                    break;
                case "remove":
                    LinkedList.Remove();
                    break;
                case "removeat":
                    LinkedList.RemoveAt(Index);
                    break;
                case "removefront":
                    LinkedList.RemoveFront();
                    break;
                case "removeback":
                    LinkedList.RemoveBack();
                    break;
                case "updatedata":
                    LinkedList.UpdateData(Index, Data);
                    break;
                case "clear":
                    LinkedList.Clear();
                    break;
                case "search":
                    // Arama görsel olacak, burada sadece liste döneriz
                    break;
                default:
                    break;
            }

            return new JsonResult(LinkedList.GetAllNodes().Select((n, i) => new { i, data = n.Data }));
        }
    }
}
