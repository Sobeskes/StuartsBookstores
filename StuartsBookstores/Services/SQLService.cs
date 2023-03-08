using Newtonsoft.Json;
using StuartsBookstores.Models;
using StuartsBookstores.Services;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;

namespace StuartsBookstores.Services
{
    public class SQLService
    {
        public SQLService(CryptographyService cryptographyService)
        {
            ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" 
                + AppDomain.CurrentDomain.GetData("DataDirectory").ToString() 
                + "\\Bookstores.mdf;Integrated Security=True";

            BuiltConnection = new SqlConnectionStringBuilder(ConnectionString);

            CryptoService = cryptographyService;
        }

        public IEnumerable<Bookstore> GetBookstores()
        {
            List<Bookstore> bookstores = new List<Bookstore>();

            using (SqlConnection connection = new SqlConnection(BuiltConnection.ConnectionString))
            {

                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM bookstores", connection);

                using (var reader = command.ExecuteReader())
                {
                    var serializer = new Newtonsoft.Json.JsonSerializer();
                    while (reader.Read())
                    {
                        var bookstore = new Bookstore();

                        bookstore.Id = reader.GetInt32(0);
                        bookstore.Name = reader.GetString(1);
                        bookstore.Image = (string)null;
                        bookstore.Address = (string)null;
                        bookstore.City = reader.GetString(4);
                        bookstore.State = reader.GetString(5);
                        bookstore.Zip = null;
                        bookstore.Country = reader.GetString(7);
                        bookstore.Latitude = null;
                        bookstore.Longitude = null;
                        bookstore.Website = (string)null;
                        bookstore.DateVisited = (string)null;

                        if (!reader.IsDBNull(2)) bookstore.Image = reader.GetString(2);
                        if (!reader.IsDBNull(3)) bookstore.Address = reader.GetString(3);
                        if(!reader.IsDBNull(6)) bookstore.Zip = reader.GetInt32(6);
                        if(!reader.IsDBNull(8)) bookstore.Latitude = (double)reader.GetSqlSingle(8);
                        if(!reader.IsDBNull(9)) bookstore.Longitude = (double)reader.GetSqlSingle(9);
                        if(!reader.IsDBNull(10)) bookstore.Website = reader.GetString(10);
                        if (!reader.IsDBNull(11)) bookstore.DateVisited = reader.GetDateTime(11).ToString("MM/dd/yyyy").Replace("-", "/");

                        bookstores.Add(bookstore);
                    }
                }
            }

            return bookstores;
        }

        public void AddBookstore(
            string name, string city, string state, string country,
            string fileName, string address, string zip,
            string latitude, string longitude, string website,
            string dateVisited
            )
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\Bookstores.mdf;Integrated Security=True";

            var builder = new SqlConnectionStringBuilder(connectionString);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Bookstores " +
                    "(name, image, address, city, state, zip, country, latitude, longitude, website, dateVisited) " +
                    "VALUES (@name, @image, @address, @city, @state, @zip, @country, @latitude, @longitude, @website, @dateVisited)",
                    connection);

                cmd.Parameters.AddWithValue("@name", name);
                addWithValueCheckNull(ref cmd, "image", fileName);
                addWithValueCheckNull(ref cmd, "address", address);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                addWithValueCheckNull(ref cmd, "zip", zip);
                cmd.Parameters.AddWithValue("@country", country);
                addWithValueCheckNull(ref cmd, "latitude", latitude);
                addWithValueCheckNull(ref cmd, "longitude", longitude);
                addWithValueCheckNull(ref cmd, "website", website);
                addWithValueCheckNull(ref cmd, "dateVisited", dateVisited);


                cmd.ExecuteNonQuery();

            }
        }

        public Bookstore GetBookstore(int id)
        {
            //TODO: Not very memory efficient
            IEnumerable<Bookstore> bookstores = GetBookstores();
            return bookstores.First(x => x.Id == id);
        }

        private void addWithValueCheckNull(ref SqlCommand cmd, string param, string? value)
        {
            if (value == null)
            {
                cmd.Parameters.AddWithValue("@" + param, DBNull.Value);
                return;
            }
            if (param == "zip")
            {
                cmd.Parameters.AddWithValue("@" + param, int.Parse(value));
                return;
            }
            if (param == "longitude" || param == "latitude")
            {
                cmd.Parameters.AddWithValue("@" + param, float.Parse(value));
                return;
            }
            if (param == "dateVisited")
            {
                cmd.Parameters.AddWithValue("@" + param, DateTime.ParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                return;
            }
            cmd.Parameters.AddWithValue("@" + param, value);
        }
        
        public List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object> paramters)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\Bookstores.mdf;Integrated Security=True";

            var builder = new SqlConnectionStringBuilder(connectionString);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);

                foreach (KeyValuePair<string, object> entry in paramters)
                {
                    cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object value = reader.GetValue(i);

                        if(value != DBNull.Value)
                        {
                            row[columnName] = value;
                        }
                    }

                    results.Add(row);
                }
            }

            return results;
        }

        ////Null??
        //private SqlDataReader ExecuteQuery(string query, Dictionary<string, object> paramters)
        //{
        //    string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\Bookstores.mdf;Integrated Security=True";

        //    var builder = new SqlConnectionStringBuilder(connectionString);

        //    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        //    {
        //        connection.Open();

        //        SqlCommand cmd = new SqlCommand(query, connection);

        //        foreach(KeyValuePair<string, object> entry in paramters)
        //        {
        //            cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
        //        }

        //        return cmd.ExecuteReader();
        //    }
        //}

        public void addUser(string name, string password)
        {

            name = name.ToLower();

            Tuple<byte[], byte[]> HashTuple = CryptoService.HashPassword(password);

            byte[] hashedPassword = HashTuple.Item1;
            byte[] salt = HashTuple.Item2;
            
            string query = "INSERT INTO Users (UserName, PasswordHash, Salt, FileName) VALUES (@UserName, @PasswordHash, @Salt, @FileName)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("UserName", name);
            parameters.Add("PasswordHash", hashedPassword);
            parameters.Add("Salt", salt);
            parameters.Add("FileName", DBNull.Value);

            ExecuteQuery(query, parameters);
        }

        //TODO, name not found
        public bool attemptLogin(string name, string password)
        {
            string query = "SELECT * FROM USERS WHERE UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("name", name);

            List<Dictionary<string, object>> result = ExecuteQuery(query, parameters);

            if (result.Count == 0) return false;

            byte[] trueHash = (byte[])result[0]["PasswordHash"];
            byte[] salt = (byte[])result[0]["Salt"];

            return CryptoService.CheckHashValid(password, salt, trueHash);
        }

        private string ConnectionString { get; }
        private SqlConnectionStringBuilder BuiltConnection { get; }
        private CryptographyService CryptoService;
    }
}
