namespace ECC.Interface;

public interface IEncryptionService
{
    byte[] Encrypt(string plaintext);
    string Decrypt(byte[] cyphertext);
}