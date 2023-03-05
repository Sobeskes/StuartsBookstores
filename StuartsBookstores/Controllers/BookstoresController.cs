using Microsoft.AspNetCore.Mvc;
using StuartsBookstores.Models;
using StuartsBookstores.Services;

namespace StuartsBookstores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookstoresController : Controller
    {
        public BookstoresController(JsonFileBookstoreService bookstoreService)
        {
            this.BookstoreService = bookstoreService;
        }

        [HttpGet]
        public IEnumerable<Bookstore> Get()
        {
            return BookstoreService.GetBookstores();
        }

        public JsonFileBookstoreService BookstoreService { get; }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
