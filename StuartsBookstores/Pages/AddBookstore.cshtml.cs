using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Services;

namespace StuartsBookstores.Pages
{
    public class AddBookstoreModel : PageModel
    {

        public AddBookstoreModel(ILogger<IndexModel> logger,
            JsonFileBookstoreService bookstoreService,
            FileService fileService)
        { 
            _logger = logger;
            BookstoreService = bookstoreService;
            FileUploadService = fileService;
        }

        public async void OnPost()
        {
            string? GeneratedFileName = await FileUploadService.UploadFile(File, "images");
            
            BookstoreService.AddBookstore(Name, City, State, Country, GeneratedFileName, Address, Zip, Latitude, Longitude, Website, DateVisited);
        }

        public void OnGet()
        {

        }

        private bool ValidInput()
        {
            if (Name == null || City == null || State == null || Country == null)
            {
                return false;
            }
            return true;
        }

        private readonly ILogger<IndexModel> _logger;
        public JsonFileBookstoreService BookstoreService;
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
