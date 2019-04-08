using System;
using System.Security.Cryptography;
using System.Text;

namespace JiraTimeBot.Core.Encryption
{
    public class PasswordEncryptionClass
    {
        private byte[] GetEntropy(string entropyString)
        {

            MD5 md5 = MD5.Create();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(entropyString));
        }

        public string Encrypt(string username, string password, string address)
        {
            byte[] entropy = GetEntropy(address + username);
            byte[] pass = Encoding.UTF8.GetBytes(password);
            byte[] encrypted = ProtectedData.Protect(pass, entropy, DataProtectionScope.LocalMachine);
            var encryptedBase64 = Convert.ToBase64String(encrypted);

            return encryptedBase64;
        }

        public string Decrypt(string username, string password, string address)
        {
            try
            {
                var pass = Convert.FromBase64String(password);
                byte[] entropy = GetEntropy(address + username);
                pass = ProtectedData.Unprotect(pass, entropy, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(pass);
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}