using System.Security.Cryptography;

namespace BirdIdentifier.Utils;

/**
 * <summary>Collection of encoding-related helper functions</summary>
 */
public static class ChecksumUtils
{
    /**
     * <summary>Helper function to generate a sha256 checksum from a file</summary>
     * <param name="file">A file</param>
     * <returns>checksum</returns>
     */
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