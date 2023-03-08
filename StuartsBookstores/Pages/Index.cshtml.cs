using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Models;
using StuartsBookstores.Services;


namespace StuartsBookstores.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public SQLService SQLService;
        public IEnumerable<Bookstore> Bookstores { get; private set; }


        public IndexModel(ILogger<IndexModel> logger,
            SQLService sqlService)
        {
            _logger = logger;
            SQLService = sqlService;
        }

        public void OnGet()
        {
            if(HttpContext.Session.GetString("LoggedInUser") == null)
            {
                HttpContext.Session.SetString("LoggedInUser", "");
            }

            Bookstores = SQLService.GetBookstores();
            LoggedUser = HttpContext.Session.GetString("LoggedInUser");
        }

        public string LoggedUser { get; private set; }
    }
}