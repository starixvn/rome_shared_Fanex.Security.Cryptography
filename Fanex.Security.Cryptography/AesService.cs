using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fanex.Cryptography.Aes
{
    /// <summary>
    /// AES Service to encrypt and decrypt.
    /// </summary>
    public interface IAesService
    {
        /// <summary>
        /// Gets vector key.
        /// </summary>
        string VectorKey { get; }

        /// <summary>
        /// Encrypts plain text.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="secretKey">A value must be 4 characters.</param>
        /// <returns>Encrypted text.</returns>
        string Encrypt(string plainText, string secretKey);

        /// <summary>
        /// Decrypts an encrypted text to plain text.
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="secretKey">A value must be 4 characters.</param>
        /// <returns>Plain text.</returns>
        string Decrypt(string cipherText, string secretKey);
    }

    /// <summary>
    /// AES Service to encrypt and decrypt.
    /// </summary>
    public class AesService : IAesService
    {
        private static IAesService aesService;

        /// <summary>
        /// Default vector key to encrypt and decrypt.
        /// </summary>
        public const string DefaultVectorKey = "a5s8d2e9c172";

        /// <summary>
        /// Default current instance of IAesService.
        /// </summary>
        public static IAesService Default => aesService = aesService ?? new AesService(DefaultVectorKey);

        /// <summary>
        /// Gets vector key.
        /// </summary>
        public string VectorKey { get; }

        /// <summary>
        /// Initializes an instance of <see cref="AesService" /> class.
        /// </summary>
        /// <param name="defaultVectorKey">A vector key with 12 characters.</param>
        public AesService(string defaultVectorKey = null)
        {
            if (!string.IsNullOrWhiteSpace(defaultVectorKey) && defaultVectorKey.Length != 12)
            {
                throw new ArgumentOutOfRangeException($"{nameof(defaultVectorKey)}: must be 12 characters");
            }

            VectorKey = string.IsNullOrWhiteSpace(defaultVectorKey) ? DefaultVectorKey : defaultVectorKey;
        }

        /// <summary>
        /// Decrypts an encrypted text to plain text.
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="secretKey">A value must be 4 characters.</param>
        /// <returns>Plain text.</returns>
        public string Decrypt(string cipherText, string secretKey)
        {

            if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length != 4)
            {
                throw new ArgumentOutOfRangeException($"{nameof(secretKey)}: must be 4 characters");
            }

            try
            {
                var key = Encoding.UTF8.GetBytes(VectorKey + secretKey);
                var iv = key;

                var encrypted = Convert.FromBase64String(cipherText.Replace(" ", "+"));
                var decrypted = DecryptStringFromBytes(encrypted, key, iv);

                return decrypted;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Encrypts plain text.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="secretKey">A value must be 4 characters.</param>
        /// <returns>Encrypted text.</returns>
        public string Encrypt(string plainText, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length != 4)
            {
                throw new ArgumentOutOfRangeException($"{nameof(secretKey)}: must be 4 characters");
            }

            try
            {
                var key = Encoding.UTF8.GetBytes(VectorKey + secretKey);
                var iv = key;
                var decrypted = EncryptStringToBytes(plainText, key, iv);

                return Convert.ToBase64String(decrypted);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            string plaintext;
            using (var rijAlg = new AesManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }

            return plaintext;
        }

        private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentNullException(nameof(plainText));
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            byte[] encrypted;
            using (var rijAlg = new AesManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }

            return encrypted;
        }
    }
}