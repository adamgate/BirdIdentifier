using System.Security.Cryptography;

namespace BirdIdentifierBackend.Utils
{
    public class EncodingUtils
    {
        public static string ToChecksum(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var sha = SHA256.Create();
            var checksum = sha.ComputeHash(stream);
            return Convert.ToHexString(checksum);
        }
    }
}
