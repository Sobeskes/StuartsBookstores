using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Models;
using StuartsBookstores.Services;


namespace StuartsBookstores.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public JsonFileBookstoreService BookstoreService;
        public IEnumerable<Bookstore> Bookstores { get; private set; }

        public IndexModel(ILogger<IndexModel> logger,
            JsonFileBookstoreService bookstoreService)
        {
            _logger = logger;
            BookstoreService = bookstoreService;
        }

        public void OnGet()
        {
            Bookstores = BookstoreService.GetBookstores();
        }
    }
}