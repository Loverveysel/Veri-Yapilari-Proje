using LinkedListCircular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkedListCircular.Pages
{
    public class ListeModel : PageModel
    {
        private static List<Node> _nodes = new();

        public void OnGet()
        {
        }

        public IActionResult OnPostExecuteOperation([FromBody] LinkedListOperationRequest request)
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
                        return BadRequest(new { success = false, message = "Geçersiz işlem" });
                }

                return new JsonResult(new
                {
                    success = true,
                    message = "İşlem tamamlandı",
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

        private void HandleInsertOperation(LinkedListOperationRequest request)
        {
            var newNode = new Node(request.Data ?? "Yeni Düğüm");

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
                        throw new ArgumentException("Pozisyon aralık dışında");
                    _nodes.Insert(request.Index, newNode);
                    break;
            }
        }

        private void HandleDeleteOperation(LinkedListOperationRequest request)
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
                        throw new ArgumentException("Pozisyon aralık dışında");
                    _nodes.RemoveAt(request.Index);
                    break;
                case "clear":
                    _nodes.Clear();
                    break;
            }
        }

        private void HandleUtilitiesOperation(LinkedListOperationRequest request)
        {
            switch (request.SubOperation.ToLower())
            {
                case "updatedata":
                    if (request.Index < 0 || request.Index >= _nodes.Count)
                        throw new ArgumentException("Pozisyon aralık dışında");
                    _nodes[request.Index].Data = request.Data ?? "Güncellendi";
                    break;
            }
        }
    }

    public class LinkedListOperationRequest
    {
        public string Operation { get; set; }
        public string SubOperation { get; set; }
        public string Data { get; set; }
        public int Index { get; set; }
    }
}