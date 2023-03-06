using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Models;
using StuartsBookstores.Services;

namespace StuartsBookstores.Pages
{
    public class MapModel : PageModel
    {

        public MapModel(JsonFileBookstoreService bookstoreService)
        {
            BookstoreService = bookstoreService;
        }

        public JsonFileBookstoreService BookstoreService { get; private set; }
        public IEnumerable<Bookstore> Bookstores { get; private set; }

        public void OnGet()
        {
            Bookstores = BookstoreService.GetBookstores();
        }
    }
}
