using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ECC.NET;

namespace ECC.Encryption
{
    
    public static class Encryption
    {

        private static Cryptography.KeyPair keyPair;
        private static Point sharedSecret;
        //private static byte[] symmKey;
        
        
        // Convert key retrieved from IoT into actual byte[]
        public static byte[] HexStringToByteArray(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentException("Hex string is null or empty.");
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Hex string must have an even length.");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        
        // To make point from IoT's public key so shared secret can be generated
        public static Point HexStringToPoint(string hex, Curve curve)
        {
            if (hex.Length % 2 != 0 || hex.Length < 2 * ((curve.Length + 7) / 8))
                throw new ArgumentException("Hex string does not represent a valid point on the curve.");

            string xHex = hex.Substring(2, hex.Length / 2 - 1); 
            string yHex = hex.Substring(2 + hex.Length / 2 - 1);

            BigInteger x = BigInteger.Parse(xHex, NumberStyles.AllowHexSpecifier);
            BigInteger y = BigInteger.Parse(yHex, NumberStyles.AllowHexSpecifier);

            Point publicKey = new Point(x, y, curve);
            curve.CheckPoint(publicKey, new ArgumentException("The public key point is not on the curve."));
            return publicKey;
        }

        // Get the pu from iot convert it to point and generate the secred w/ ECC form PK PU and the secp256 curve
        public static void GenSharedSecret(string message)
        {
            string ioTPublicKeyHex = message.Substring("Connected:".Length).Trim();
            try
            {
                Curve curve = new Curve(Curve.CurveName.secp256r1);
                Point ioTPublicKeyPoint = HexStringToPoint(ioTPublicKeyHex, curve);
                Point shase = Cryptography.GetSharedSecret(keyPair.PrivateKey, ioTPublicKeyPoint);
                Console.WriteLine($"Shared secret computed: ({sharedSecret.X}, {sharedSecret.Y})");
                
                // Save shared secret for further use
                sharedSecret = shase;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error generating shared secret: {e.Message}");
            }
        }

        // Getting the symmetric key from the shared secret by hashing the X coord
        public static byte[] DeriveSymmetricKey()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(sharedSecret.X.ToByteArray());
                // symmKey = sha256.ComputeHash(sharedSecret.X.ToByteArray());

            }
        }

        // Encrypting message with the symmetric key obtained form shared secret using AES
        public static string EncryptMessage(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // aesAlg.Key = DeriveSymmetricKey();
                // aesAlg.Key = symmKey;
                byte[] key = Encoding.UTF8.GetBytes("RaT‰ëòçÇRQqBèºQ|{ŽnÎA");
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Decrypting message from IoT by using the symmetric key derived from shared secret since we have the same 
        // shared secret same shared secret thus the same symmetric key 
        public static string DecryptMessage(string cipherText)
        {
            
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                // aesAlg.Key = DeriveSymmetricKey();
                byte[] key = Encoding.UTF8.GetBytes("RaT‰ëòçÇRQqBèºQ|{ŽnÎA");
                aesAlg.Key = key;
                // aesAlg.Key = symmKey;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipher))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        // I think we need this since in hte handshake we make a keypair based on a curve and i suppose we 
        // want to use the same keypair throughout the connection
        public static void SaveKeyPair(Cryptography.KeyPair generatedKeyPair)
        {
            keyPair = generatedKeyPair;
        }

        public static Cryptography.KeyPair GetKeyPair()
        {
            return keyPair;
        }
        
    }
}
