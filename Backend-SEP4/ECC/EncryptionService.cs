using System.Security.Cryptography;
using ECC.Interface;

namespace ECC;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] encryptionKey;
    private readonly byte[] iv;
    
    public EncryptionService(byte[] encryptionKey, byte[] iv)
    {
        this.encryptionKey = encryptionKey;
        this.iv = iv;
    }
    
    public byte[] Encrypt(string plaintext)
    {
        byte[] cyphertextBytes;
        using var aes = Aes.Create();
        var encryptor = aes.CreateEncryptor(encryptionKey, iv);
        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plaintext);
                }
            }
            cyphertextBytes = memoryStream.ToArray();
            return cyphertextBytes;

        }
    }

    public string Decrypt(byte[] cyphertext)
    {
        var cyphertextBytes = cyphertext;
        using var aes = Aes.Create();
        var decryptor = aes.CreateDecryptor(encryptionKey, iv);
        using (var memoryStream = new MemoryStream(cyphertextBytes))
        {
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}