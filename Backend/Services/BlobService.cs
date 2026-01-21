using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SupermarketAPI.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IConfiguration configuration, ILogger<BlobService> logger)
        {
            _logger = logger;
            var connectionString = configuration["Blob:ConnectionString"];
            var containerName = configuration["Blob:ContainerName"] ?? "product-images";

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogWarning("Blob:ConnectionString no configurada. La subida de imágenes no funcionará.");
                throw new InvalidOperationException("Blob:ConnectionString no está configurada en las variables de entorno.");
            }

            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            
            // Crear el contenedor si no existe con acceso público para lectura
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("El archivo está vacío o es nulo.");
            }

            // Validar tipo de archivo
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"Tipo de archivo no permitido. Solo se permiten: {string.Join(", ", allowedExtensions)}");
            }

            // Validar tamaño (máximo 5MB)
            const long maxSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxSize)
            {
                throw new ArgumentException("El archivo es demasiado grande. Tamaño máximo: 5MB");
            }

            // Generar nombre único para el archivo
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            try
            {
                // Subir el archivo
                var blobClient = _containerClient.GetBlobClient(fileName);
                
                using (var stream = file.OpenReadStream())
                {
                    var uploadOptions = new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = file.ContentType
                        }
                    };
                    await blobClient.UploadAsync(stream, uploadOptions);
                }

                // Retornar la URL pública del blob
                var imageUrl = blobClient.Uri.ToString();
                _logger.LogInformation("Imagen subida exitosamente: {ImageUrl}", imageUrl);
                
                return imageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir imagen a Blob Storage");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return false;
            }

            try
            {
                // Extraer el nombre del archivo de la URL
                var uri = new Uri(imageUrl);
                var fileName = Path.GetFileName(uri.LocalPath);

                var blobClient = _containerClient.GetBlobClient(fileName);
                var result = await blobClient.DeleteIfExistsAsync();

                _logger.LogInformation("Imagen eliminada: {FileName}", fileName);
                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen de Blob Storage: {ImageUrl}", imageUrl);
                return false;
            }
        }
    }
}
