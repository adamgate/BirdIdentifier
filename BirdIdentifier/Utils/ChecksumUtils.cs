using System.Security.Cryptography;

namespace UploadImage_Api.Util;

public static class ChecksumUtils
{
    public static string GetChecksum(IFormFile file)
    { 
        using (var stream = file.OpenReadStream())
        {
            var sha = new SHA256Managed();
            var checksum = sha.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }
    }
}