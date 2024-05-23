using System;
using System.Security.Cryptography;
using System.Text;
using ECC;
using ECC.Interface;

public class KeyGenerator
{
    public static void Main()
    {
        string encodedString = "RaT‰ëòçÇRQqBèºQ|{ŽnÎA";

        // Attempt to handle it as Base64, although the string does not look like standard Base64
        try
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            Console.WriteLine("Decoded string: " + decodedString);
        }
        catch (FormatException)
        {
            Console.WriteLine("The string is not a valid Base64 encoded string.");
        }
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();

            byte[] key = aes.Key;
            byte[] iv = aes.IV;

            Console.WriteLine(key);
            Console.WriteLine(aes.KeySize);
            Console.WriteLine("Shared Key: " + Convert.ToBase64String(key));
            Console.WriteLine("Initialization Vector (IV): " + Convert.ToBase64String(iv));

            string p = "This is the plaintext";

            IEncryptionService enc = new EncryptionService(Convert.FromBase64String("qKBL+IAOLbn+jLnFJEYp8KAmlAe4iVQVfa2K4d9huA4=\n"), iv);
            byte[] c = enc.Encrypt(p);
            Console.WriteLine(c);
            string m = enc.Decrypt(c);
            Console.WriteLine(m);
        }
    }
}