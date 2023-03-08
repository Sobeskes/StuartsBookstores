using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Services;
using System.Data.SqlClient;
using System.Globalization;

namespace StuartsBookstores.Pages
{
    public class AddBookstoreModel : PageModel
    {

        public AddBookstoreModel(ILogger<IndexModel> logger,
            SQLService bookstoreService,
            FileService fileService)
        { 
            _logger = logger;
            DatabaseService = bookstoreService;
            FileUploadService = fileService;
        }

        public async Task<IActionResult> OnPost()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != "stuart")
            {
                return RedirectToPage("Index");
            }
            string? GeneratedFileName = await FileUploadService.UploadFile(File, "images");

            DatabaseService.AddBookstore(Name, City, State, Country, GeneratedFileName, Address,
                Zip, Latitude, Longitude, Website, DateVisited);

            return new EmptyResult();
        }

        public IActionResult OnGet()
        {
            if(HttpContext.Session.GetString("LoggedInUser") != "stuart")
            {
                return RedirectToPage("Index");
            }
            return new EmptyResult();

        }

        private readonly ILogger<IndexModel> _logger;
        public SQLService DatabaseService;
        public FileService FileUploadService;

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public IFormFile? File { get; set; }
        [BindProperty]
        public string? Address { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string State { get; set; }
        [BindProperty]
        public string? Zip { get; set; }
        [BindProperty]
        public string Country { get; set; }
        [BindProperty]
        public string? Latitude { get; set; }
        [BindProperty]
        public string? Longitude { get; set; }
        [BindProperty]
        public string? Website { get; set; }
        [BindProperty]
        public string? DateVisited { get; set; }
    }
}
