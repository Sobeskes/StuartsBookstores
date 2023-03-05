using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Services;
using StuartsBookstores.Models;

namespace StuartsBookstores.Pages
{
    public class BookstoreModel : PageModel
    {
        public BookstoreModel(ILogger<BookstoreModel> logger,
            JsonFileBookstoreService bookstoreService)
        {
            _logger = logger;
            BookstoreService = bookstoreService;
        }

        public void OnGet(int id)
        {
            Bookstore = BookstoreService.GetBookstore(id);
        }

        private readonly ILogger<BookstoreModel> _logger;

        public JsonFileBookstoreService BookstoreService;
        public Bookstore Bookstore { get; private set; }
    }
}