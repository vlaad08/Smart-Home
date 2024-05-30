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
        aes.Key = encryptionKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        var encryptor = aes.CreateEncryptor(encryptionKey, null);
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
        {
            Console.WriteLine("Starting Decryption");

            using var aes = Aes.Create();
            aes.Key = encryptionKey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;
            var decryptor = aes.CreateDecryptor(aes.Key, null);

            using var memoryStream = new MemoryStream(cyphertext);
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                using (var streamReader = new StreamReader(cryptoStream))
                {

                    string plaintext = streamReader.ReadToEnd();
                    return plaintext;
                }
            }
        }
    }
}