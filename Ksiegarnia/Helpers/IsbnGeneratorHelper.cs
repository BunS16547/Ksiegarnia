using System.Security.Cryptography;
using System.Text;

namespace Ksiegarnia.Helpers;

public static class IsbnGeneratorHelper {
    // znaki z których generowany jest losowy ISBN
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";

    public static string Generate()
    {
        var result = new StringBuilder(13);

        for (int i = 0; i < 13; i++)
        {
            var index = RandomNumberGenerator.GetInt32(Chars.Length);
            result.Append(Chars[index]);
        }

        return result.ToString();
    }
}