using System.Security.Cryptography;

namespace BirdIdentifier.Utils;

/**
 * <summary>Collection of encoding-related helper functions</summary>
 */
public static class EncodingUtils
{
    /**
     * <summary>Helper function to generate a sha256 checksum from a file</summary>
     * <param name="file">A file</param>
     * <returns>checksum</returns>
     */
    public static string ToChecksum(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            var sha = SHA256.Create();
            var checksum = sha.ComputeHash(stream);
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }
    }

    /**
     * <summary>Helper function to generate a base64 string from a file</summary>
     * <param name="file">A file</param>
     * <returns>base64 string</returns>
     */
    public static string ToBase64(IFormFile file)
    {
        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            var base64ImageRepresentation = Convert.ToBase64String(stream.ToArray());
            return base64ImageRepresentation;
        }
    }

    // public static IFormFile FromBase64(String image)
    // {
    //     // var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
    // }
}
