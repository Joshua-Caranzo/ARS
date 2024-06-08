using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Helper
{
    public class AES
    {
        private static readonly byte[] Key = { 158, 217, 19, 56, 24, 26, 85, 45, 114, 34, 27, 162, 37, 112, 222, 209, 179, 24, 175, 144, 173, 53, 43, 29, 24, 87, 17, 218, 134, 236, 53, 90 };
        private static readonly byte[] Vector = { 188, 64, 211, 111, 23, 3, 217, 119, 54, 121, 221, 32, 79, 98, 44, 156 };
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;
        private readonly UTF8Encoding _encoder;

        public AES()
        {
            var rm = new RijndaelManaged();
            _encryptor = rm.CreateEncryptor(Key, Vector);
            _decryptor = rm.CreateDecryptor(Key, Vector);
            _encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(_encoder.GetBytes(unencrypted))).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public string Decrypt(string encrypted)
        {
            try
            {
                string incoming = encrypted.Replace('_', '/').Replace('-', '+');
                switch (encrypted.Length % 4)
                {
                    case 2:
                        incoming += "==";
                        break;
                    case 3:
                        incoming += "=";
                        break;
                }
                return string.IsNullOrEmpty(incoming) ? null : _encoder.GetString(Decrypt(Convert.FromBase64String(incoming)));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, _encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, _decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
