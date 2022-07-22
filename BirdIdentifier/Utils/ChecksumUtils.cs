using System.Security.Cryptography;

namespace BirdIdentifier.Utils;

public static class ChecksumUtils
{
    public static string GetChecksum(IFormFile file)
    { 
        using (var stream = file.OpenReadStream())
        {
            var sha = SHA256.Create();
            var checksum = sha.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }
    }
}