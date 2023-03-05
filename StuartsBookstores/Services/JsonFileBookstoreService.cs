using Microsoft.AspNetCore.Hosting;
using StuartsBookstores.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace StuartsBookstores.Services
{
    public class JsonFileBookstoreService
    {
        public JsonFileBookstoreService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "bookstores.json"); }
        }

        public IEnumerable<Bookstore> GetBookstores()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
#pragma warning disable CS8603
                {
                    return JsonSerializer.Deserialize<Bookstore[]>(jsonFileReader.ReadToEnd(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }); ;
                }
            }
        }

        public Bookstore GetBookstore(int id)
        {
            IEnumerable<Bookstore> bookstores = GetBookstores();
            return bookstores.First(x => x.Id == id);
        }
    }
}