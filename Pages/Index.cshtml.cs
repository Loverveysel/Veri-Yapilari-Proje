using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkedListCircular.Pages
{
    public class IndexModel : PageModel
    {
        private static List<Node> _nodes = new();

        public void OnGet()
        {
        }

        public IActionResult OnPostExecuteOperation([FromBody] OperationRequest request)
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
                        if (request.SubOperation.ToLower() != "search")
                        {
                            HandleUtilitiesOperation(request);
                        }
                        break;
                    default:
                        return BadRequest(new { success = false, message = "Invalid operation" });
                }

                return new JsonResult(new
                {
                    success = true,
                    message = "Operation completed",
                    list = _nodes.Select((n, i) => new { data = n.Data, index = i })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        private void HandleInsertOperation(OperationRequest request)
        {
            var newNode = new Node(request.Data ?? "New Node");

            switch (request.SubOperation.ToLower())
            {
                case "insertfront":
                    _nodes.Insert(0, newNode);
                    break;
                case "insertback":
                    _nodes.Add(newNode);
                    break;
                case "insertat":
                    if (request.Index < 0 || request.Index > _nodes.Count)
                        throw new ArgumentException("Index out of range");
                    _nodes.Insert(request.Index, newNode);
                    break;
            }
        }

        private void HandleDeleteOperation(OperationRequest request)
        {
            switch (request.SubOperation.ToLower())
            {
                case "removefront":
                    if (_nodes.Count > 0)
                        _nodes.RemoveAt(0);
                    break;
                case "removeback":
                    if (_nodes.Count > 0)
                        _nodes.RemoveAt(_nodes.Count - 1);
                    break;
                case "removeat":
                    if (request.Index < 0 || request.Index >= _nodes.Count)
                        throw new ArgumentException("Index out of range");
                    _nodes.RemoveAt(request.Index);
                    break;
                case "clear":
                    _nodes.Clear();
                    break;
            }
        }

        private void HandleUtilitiesOperation(OperationRequest request)
        {
            switch (request.SubOperation.ToLower())
            {
                case "updatedata":
                    if (request.Index < 0 || request.Index >= _nodes.Count)
                        throw new ArgumentException("Index out of range");
                    _nodes[request.Index].Data = request.Data ?? "Updated";
                    break;
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

    public class Node
    {
        public string Data { get; set; }

        public Node(string data)
        {
            Data = data;
        }
    }
}