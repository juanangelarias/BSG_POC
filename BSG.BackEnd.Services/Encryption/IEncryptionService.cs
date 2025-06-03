namespace BSG.BackEnd.Services.Encryption;

public interface IEncryptionService
{
    Task<byte[]> EncryptAsync(string text, byte[]? salt = null);
    Task<string> DecryptAsync(byte[] hashed, byte[]? salt = null);
    byte[] OneWayEncrypt(string text, byte[]? salt = null);
    byte[] CreateSalt();
    string CreateMailToken();
    bool VerifyHashed( string password, byte[] hash, byte[] salt );
}