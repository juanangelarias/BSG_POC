using System.Security.Cryptography;
using System.Text;
using BSG.BackEnd.Common.Model;

namespace BSG.BackEnd.Services.Encryption;

public class EncryptionService(Keys keys): IEncryptionService
{
    private const int ITERATIONS = 350000;
    private const int KEY_LENGTH = 256;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;
    private const string VALID_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int MAIL_TOKEN_LENGTH = 64;
    private readonly byte[] _iv = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16];
    private const int START = 19;
    private const int LENGTH = 32;

    public async Task<byte[]> EncryptAsync(string clearText, byte[]? encryptionKey = null)
    {
        encryptionKey ??= Convert.FromBase64String(keys.Encryption!.Substring(START,LENGTH));

        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.IV = _iv;

        using var output = new MemoryStream();
        await using var cryptoStream = new CryptoStream( output, aes.CreateEncryptor(), CryptoStreamMode.Write );

        await cryptoStream.WriteAsync( Encoding.Unicode.GetBytes( clearText ) );
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }
    
    public async Task<string> DecryptAsync(byte[] cipherText, byte[]? encryptionKey = null)
    {
        encryptionKey ??= Convert.FromBase64String(keys.Encryption!.Substring(START,LENGTH));
        
        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.IV = _iv;

        using var input = new MemoryStream( cipherText );
        await using var cryptoStream = new CryptoStream( input, aes.CreateDecryptor(), CryptoStreamMode.Read );
        using var output = new MemoryStream();

        await cryptoStream.CopyToAsync( output );

        return Encoding.Unicode.GetString( output.ToArray() );
    }

    public byte[] OneWayEncrypt(string text, byte[]? key = null)
    {
        key ??= Encoding.Unicode.GetBytes(keys.Encryption!);
        var passwordBytes = Encoding.Unicode.GetBytes( text );

        var hash =  Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            key,
            ITERATIONS,
            HashAlgorithm,
            KEY_LENGTH);
        
        return hash;
    }

    public byte[] CreateSalt()
    {
        var passwordBytes = RandomNumberGenerator.GetBytes( KEY_LENGTH );
        
        var emptySalt = Array.Empty<byte>();
        var hash =  Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            emptySalt,
            ITERATIONS,
            HashAlgorithm,
            KEY_LENGTH);

        return hash;
    }

    public string CreateMailToken()
    {
        var len = VALID_CHARS.Length;
        var rnd = new Random(Guid.NewGuid().GetHashCode());

        var token = string.Empty;
        for (var i = 0; i < MAIL_TOKEN_LENGTH; i++)
        {
            var pos = rnd.Next(len);
            token += VALID_CHARS.Substring(pos, 1);
        }

        return token;
    }

    public bool VerifyHashed( string password, byte[] hash, byte[] salt )
    {
        var passwordBytes = Encoding.Unicode.GetBytes( password );
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2( passwordBytes, salt, ITERATIONS, HashAlgorithm, KEY_LENGTH );
        return CryptographicOperations.FixedTimeEquals( hashToCompare, hash );
    }
}