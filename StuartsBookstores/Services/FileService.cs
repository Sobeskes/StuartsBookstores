namespace StuartsBookstores.Services
{
    public class FileService
    {
        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _webHostEnvironment = hostingEnvironment;
        }

        public async Task<string?> UploadFile(IFormFile? File, string path)
        {
            string? GeneratedFileName = null;
            if(File != null && File.Length > 0)
            {
                GeneratedFileName = Guid.NewGuid().ToString() + "_" + File.FileName;
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, path, GeneratedFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(fileStream);
                }
            }

            return GeneratedFileName;
        }

        private readonly IWebHostEnvironment _webHostEnvironment;
    }
}
