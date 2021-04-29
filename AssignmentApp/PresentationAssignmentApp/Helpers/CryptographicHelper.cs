using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace PresentationAssignmentApp.Helpers
{
    public static class CryptographicHelper
    {

        //Might want to store these in the database
        private static readonly Tuple<byte[], byte[]> _keyIVPair;

        private const string _pass = "Pa$$w0rd";
        private static readonly byte[] _salt;


        static CryptographicHelper()
        {
            _salt = GenerateSalt();
            _keyIVPair = GenerateKeys();
        }

        public static byte[] Hash(byte[] message, byte[] salt = null)
        {
            byte[] messageWithSalt;

            if(salt == null)
            {
                messageWithSalt = message;
            }
            else
            {
                List<byte> temp = new List<byte>(message.Length + salt.Length);
                temp.AddRange(message);
                temp.AddRange(salt);

                messageWithSalt = temp.ToArray();
            }

            SHA512 sha = SHA512.Create();
            byte[] digest = sha.ComputeHash(messageWithSalt);

            return digest;
        }

        public static string Hash(string message)
        {
            byte[] encodedMessage = Encoding.UTF32.GetBytes(message);

            byte[] digest = Hash(encodedMessage);

            return Convert.ToBase64String(digest);
        }

        public static byte[] GenerateSalt(int size = 64)
        {
            byte[] salt = new byte[size];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            rng.GetBytes(salt);

            return salt;
        }

        public static string SymmetricEncrypt(string plainTextMessage)
        {

            byte[] messageAsBytes = Encoding.UTF32.GetBytes(plainTextMessage);

            byte[] cipherAsBytes = SymmetricEncrypt(messageAsBytes);

            return Convert.ToBase64String(cipherAsBytes);
        }


        //Used for query strings
        //As part of hybrid encryption

        //Be sure to URI.Encode before and URI.decode after you use it as a query string
        public static byte[] SymmetricEncrypt(byte[] plainTextMessage)
        {
            
            Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            using (MemoryStream msIn = new MemoryStream(plainTextMessage))
            {
              
                msIn.Position = 0;

                using (MemoryStream msOut = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(
                        msOut,
                        aes.CreateEncryptor(_keyIVPair.Item1, _keyIVPair.Item2),
                        CryptoStreamMode.Write))
                    {
                        msIn.CopyTo(cs);
                        cs.FlushFinalBlock();
                    }

                    return msOut.ToArray();
                }
            }
        }

        public static string SymmetricDecrypt(string cipherText)
        {
            byte[] cipherTextAsBytes = Convert.FromBase64String(cipherText);

            byte[] plainTextAsBytes = SymmetricDecrypt(cipherTextAsBytes);

            string decryptedCipher = Encoding.UTF32.GetString(plainTextAsBytes);

            return decryptedCipher;
        }

        public static byte[] SymmetricDecrypt(byte[] encryptedMessage)
        {
            Aes aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            using (MemoryStream msIn = new MemoryStream(encryptedMessage))
            {
                msIn.Position = 0;
                using (MemoryStream msOut = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(
                                msOut,
                                aes.CreateDecryptor(_keyIVPair.Item1, _keyIVPair.Item2),
                                CryptoStreamMode.Write))
                    {
                        msIn.CopyTo(cs);
                        cs.FlushFinalBlock();
                    }

                    return msOut.ToArray();
                }
            }
        }

        public static Tuple<byte[], byte[]> GenerateKeys()
        {
            Aes aes = Aes.Create();

            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(_pass, _salt);

            byte[] key = keyGenerator.GetBytes(aes.KeySize / 8);
            byte[] iv = keyGenerator.GetBytes(aes.BlockSize / 8);

            return new Tuple<byte[], byte[]>(key, iv);
        }

        public static Tuple<string, string> GenerateAsymmetricKeys()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

            string publicKey = provider.ToXmlString(false);

            string privateKey = provider.ToXmlString(true);

            return new Tuple<string, string>(publicKey, privateKey);
        }

        public static byte[] AsymmetricEncrypt(byte[] data, string publicKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

            provider.FromXmlString(publicKey);

            byte[] cipher = provider.Encrypt(data, RSAEncryptionPadding.Pkcs1);

            return cipher;
        }

        public static byte[] AsymmetricDecrypt(byte[] data, string privateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

            provider.FromXmlString(privateKey);

            byte[] cipher = provider.Decrypt(data, RSAEncryptionPadding.Pkcs1);

            return cipher;
        }

        public static byte[] GenerateSignature(byte[] objectToSign, string privateKey)
        { 
            RSA rsa = RSA.Create();
            rsa.FromXmlString(privateKey);

            RSAPKCS1SignatureFormatter signatureFormatter = new RSAPKCS1SignatureFormatter(rsa);

            signatureFormatter.SetHashAlgorithm("SHA1");

            byte[] signedHashValue = signatureFormatter.CreateSignature(objectToSign);

            return signedHashValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signedHashValue"></param>
        /// <param name="objectToVerify">
        /// Same item that was signed earlier.
        /// 
        /// </param>
        /// <returns></returns>
        public static bool VerifySignature(byte[] signedHashValue, byte[] objectToVerify, string publicKey)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(publicKey);

            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA1");

            bool isValid = rsaDeformatter.VerifySignature(objectToVerify, signedHashValue);

            return isValid;
        }


        public static void Hybrid()
        {
            RSAPKCS1KeyExchangeFormatter rSAPKCS1KeyExchangeFormatter = new RSAPKCS1KeyExchangeFormatter();
          //  rSAPKCS1KeyExchangeFormatter.
        }

    }
}
