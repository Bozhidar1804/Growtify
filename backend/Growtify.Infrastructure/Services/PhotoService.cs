using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Growtify.Application.Common.Settings;
using Growtify.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Growtify.Infrastructure.Services
{
    // PhotoService is responsible for handling photo uploads and deletions using Cloudinary. It implements the IPhotoService interface defined in the Application layer, allowing for abstraction and separation of concerns. The service uses Cloudinary's SDK to interact with the Cloudinary API, enabling efficient management of photos in the application.
    // PhotoService is implemented in Infrastructure layer because it uses CloudinaryDotNet, which is not needed in the Application layer. This way, we keep the Application layer clean and focused on business logic, while the Infrastructure layer handles the implementation details of photo management.
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
                    Folder = "da-ang20"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }
    }
}
