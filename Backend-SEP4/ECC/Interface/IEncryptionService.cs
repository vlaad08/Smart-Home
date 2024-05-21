namespace ECC.Interface;

public interface IEncryptionService
{
    string Encrypt(string plaintext);
    string Decrypt(string cyphertext);
}