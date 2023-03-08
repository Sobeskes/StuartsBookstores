using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StuartsBookstores.Services;

namespace StuartsBookstores.Pages
{
    public class LoginModel : PageModel
    {

        public LoginModel(SQLService databaseService)
        {
            DatabaseService = databaseService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostLogin()
        {
            bool loggedIn = DatabaseService.attemptLogin(Username, Password);
            if (loggedIn)
            {
                HttpContext.Session.SetString("LoggedInUser", Username.ToLower());
                return RedirectToPage("Index");
            }
            else
            {
                return new EmptyResult();
            }
        }

        public IActionResult OnPostLogout()
        {

            HttpContext.Session.SetString("LoggedInUser", "");
            return RedirectToPage("Index");
        }

        public SQLService DatabaseService;

        [BindProperty] public string Username { get; set; }

        [BindProperty] public string Password { get; set; }
    }
}
