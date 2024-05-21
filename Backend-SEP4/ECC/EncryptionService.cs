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
        try
        {
            Console.WriteLine("Starting Decryption");

            using var aes = Aes.Create();
            aes.Key = encryptionKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;

            Console.WriteLine(Convert.ToBase64String(encryptionKey));
            Console.WriteLine(Convert.ToBase64String(iv));
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(cyphertext);
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    Console.WriteLine("Attempting to read decrypted data...");
                    string plaintext = streamReader.ReadToEnd();
                    Console.WriteLine("Decryption successful.");
                    Console.WriteLine(plaintext);
                    return plaintext;
                }
            }
        }
        catch (CryptographicException e)
        {
            Console.WriteLine($"CryptographicException: {e.Message}");
            throw; // Re-throwing to handle it at a higher level if needed
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw; // Re-throwing to handle it at a higher level if needed
        }
    }

}