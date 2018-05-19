using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Aes256
    {
        public string EncryptString(byte[] plainBytes, byte[] key, byte[] iv, byte[] day)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            encryptor.Padding = PaddingMode.None;

            // Set key and IV
            encryptor.Key = key;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array

            // Encrypt the input plaintext string
            //byte[] plainByte = Auto_padding(plainBytes, day);
            //cryptoStream.Write(plainByte, 0, plainByte.Length);

            byte[] plainByte = new byte[1024];
            string t = tinhpading(Encoding.ASCII.GetString(plainBytes));
            plainByte = Encoding.ASCII.GetBytes(t);
            cryptoStream.Write(plainByte, 0, plainByte.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText + ';' + Convert.ToString(plainByte.Length - plainBytes.Length);
        }
        public string DecryptString(string cipherText, byte[] key, byte[] iv, int count)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            encryptor.Padding = PaddingMode.None;

            // Set key and IV
            encryptor.Key = key;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();
                plainBytes = Dell_padding(plainBytes, count);

                // Convert the decrypted byte array to string
                plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length).TrimEnd('\0');
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;
        }
        //loai bo so bit da thoa thuan 
        public byte[] Dell_padding(byte[] mess_encrypt, int count)
        {
            int n = mess_encrypt.Length;
            byte[] tmp = new byte[n];
            for (int i = 0; i < n; i++)
            {
                if (n - i > count)
                {
                    tmp[i] = mess_encrypt[i];
                }
                else
                {
                    tmp[i] = 0;
                }
            }
            return tmp;
        }
        
        public string getdatime()
        {
            return DateTime.Now.ToString("h:mm:ss tt");
        }

        //Tính Padding
        public string tinhpading(string text) 
        {
            md5 hash = new md5();
            string ivc = getdatime();
            string chuoipading;

            int i = 0;
            int n = text.Length % 16;
            if (n != 0)
            {
                string thempading = hash.GetMD5(ivc).Substring(0, 16 - n);
                chuoipading = text + thempading;
            }
            else
            {
                chuoipading = text;
            }
            return chuoipading;

        }


    }
}
