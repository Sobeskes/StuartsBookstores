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

                BookstoreData bookstoreData = GetData();
                return bookstoreData.Bookstores;
            }
        }

        public void AddBookstore(
            string name, string city, string state, string country,
            string fileName, string address, string zip,
            string latitude, string longitude, 
            string website, string dateVisted
            )
        {
            IEnumerable<Bookstore> bookstores = GetBookstores();

            Bookstore newBookstore = new Bookstore();

            newBookstore.Id = GetNextID();

            newBookstore.Name = name;
            newBookstore.City = city;
            newBookstore.State = state;
            newBookstore.Country = country;
            if (fileName != null) newBookstore.Image = fileName;
            if (address != null) newBookstore.Address = address;
            if (zip != null) newBookstore.Zip = Convert.ToInt32(zip);
            if (latitude != null) newBookstore.Latitude = Convert.ToDouble(latitude);
            if (longitude != null) newBookstore.Longitude = Convert.ToDouble(latitude);
            if (website != null) newBookstore.Website = website;
            if (dateVisted != null) newBookstore.DateVisited = dateVisted;

            bookstores = bookstores.Concat(new[] { newBookstore});

            BookstoreData bookstoreData = GetData();
            bookstoreData.Bookstores = bookstores;
            bookstoreData.Meta.IDCounter = bookstoreData.Meta.IDCounter + 1;
            bookstoreData.Meta.BookstoreCount = bookstoreData.Meta.BookstoreCount + 1;

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<BookstoreData>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    bookstoreData
                );
            }
        }

        private BookstoreData GetData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {

                BookstoreData bookstoreData = JsonSerializer.Deserialize<BookstoreData>(jsonFileReader.ReadToEnd(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                return bookstoreData;
            }
        }

        public Bookstore GetBookstore(int id)
        {
            IEnumerable<Bookstore> bookstores = GetBookstores();
            return bookstores.First(x => x.Id == id);
        }

        public int GetNextID()
        {
            BookstoreData bookstoreData = GetData();
            return bookstoreData.Meta.IDCounter;
        }
    }
}