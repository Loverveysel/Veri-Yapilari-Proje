using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Veri_Yapilari_Proje.Models;

namespace Veri_Yapıları_Proje.Pages.LinkedList
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string SelectedListType { get; set; } = "singly";

        [BindProperty]
        public string ActionType { get; set; } = "add";

        [BindProperty]
        public int NodeValue { get; set; }

        public string ListJson { get; set; } = "{}";

        private static ListManager listManager = new ListManager();

        public void OnGet()
        {
            ListJson = listManager.GetListAsJson(SelectedListType);
        }

        public IActionResult OnPost()
        {
            listManager.ProcessCommand(SelectedListType, ActionType, NodeValue);
            ListJson = listManager.GetListAsJson(SelectedListType);
            return Page();
        }
    }
}
