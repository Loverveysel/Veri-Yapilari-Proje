using LinkedListCircular.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LinkedListCircular.Controllers
{
    public class HomeController : Controller
    {
        private static CircularLinkedList _list = new CircularLinkedList();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecuteOperation([FromBody] OperationRequest request)
        {
            try
            {
                switch (request.Operation.ToLower())
                {
                    case "insert":
                        HandleInsertOperation(request);
                        break;
                    case "delete":
                        HandleDeleteOperation(request);
                        break;
                    case "utilities":
                        HandleUtilitiesOperation(request);
                        break;
                    default:
                        return BadRequest("Invalid operation");
                }

                return Ok(new
                {
                    success = true,
                    list = _list.ToJson(),
                    message = "Operation completed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private void HandleInsertOperation(OperationRequest request)
        {
            switch (request.SubOperation.ToLower())
            {
                case "insertfront":
                    _list.InsertFront(request.Data);
                    break;
                case "insertback":
                    _list.InsertBack(request.Data);
                    break;
                case "insertat":
                    _list.InsertAt(request.Data, request.Index);
                    break;
                default:
                    throw new ArgumentException("Invalid insert sub-operation");
            }
        }

        private void HandleDeleteOperation(OperationRequest request)
        {
            switch (request.SubOperation.ToLower())
            {
                case "remove":
                case "removefront":
                    _list.RemoveFront();
                    break;
                case "removeback":
                    _list.RemoveBack();
                    break;
                case "removeat":
                    _list.RemoveAt(request.Index);
                    break;
                default:
                    throw new ArgumentException("Invalid delete sub-operation");
            }
        }

        private void HandleUtilitiesOperation(OperationRequest request)
        {
            switch (request.SubOperation.ToLower())
            {
                case "updatedata":
                    _list.UpdateData(request.Index, request.Data);
                    break;
                case "clear":
                    _list.Clear();
                    break;
                case "search":
                    // Search is handled client-side for visualization
                    break;
                default:
                    throw new ArgumentException("Invalid utilities sub-operation");
            }
        }
    }

    public class OperationRequest
    {
        public string Operation { get; set; }
        public string SubOperation { get; set; }
        public string Data { get; set; }
        public int Index { get; set; }
    }
}