using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Services
{
    public class Upload
    {
        public static async Task<bool> UploadFiles(IFormFile signatureFile, int userId, string imageCategory, BankDbContext context)
        {
            try
            {
                if (signatureFile == null || signatureFile.Length == 0)
                {
                    throw new ArgumentException("File is null or empty.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(signatureFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await signatureFile.CopyToAsync(stream);
                }

                filePath = Path.Combine("/uploads", fileName);

                bool isFileRecordExists = context.UserFiles.Any(uf => uf.user_id == userId);
                if (isFileRecordExists)
                {
                    var existingFileRecord = context.UserFiles.First(uf => uf.user_id == userId);
                    if (imageCategory == "Signature")
                    {
                        existingFileRecord.signature_path = filePath;
                    }
                    else if (imageCategory == "ProfilePicture")
                    {
                        existingFileRecord.profile_photo_path = filePath;
                    }
                    context.UserFiles.Update(existingFileRecord);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var newUserFile = new BankEntities.UserFiles
                    {
                        user_id = userId,
                        signature_path = imageCategory == "Signature" ? filePath : null,
                        profile_photo_path = imageCategory == "ProfilePicture" ? filePath : null
                    };
                    context.UserFiles.Add(newUserFile);
                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading files: {ex.Message}");
                return false;
            }
        }

        public static async Task<UserFiles> GetUserFile(int id, BankDbContext context)
        {
            try
            {
                UserFiles userFile = context.UserFiles.FirstOrDefault(e => e.user_id == id);
                return userFile;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
