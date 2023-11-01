﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ArcaneSurvivorsClient {
    public static class Encrypt {
        private static Random random = new();

        //해쉬생성
        public static string GenerateHash(int length) {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789_";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //단방향 암호화
        public static class SimplexHash {
            public static string ComputeMD4(string text) {
                byte[] hash = ComputeMD4Binary(Encoding.UTF8.GetBytes(text));
                StringBuilder sb = new();
                for (int i = 0; i < hash.Length; ++i) { sb.Append(hash[i].ToString("x2")); }

                return sb.ToString();
            }

            public static byte[] ComputeMD4Binary(byte[] data) {
                using (HashAlgorithm cryptor = MD4.Create()) { return cryptor.ComputeHash(data); }
            }

            public static string ComputeMD5(string text) {
                byte[] hash = ComputeMD5Binary(Encoding.UTF8.GetBytes(text));
                StringBuilder sb = new();
                for (int i = 0; i < hash.Length; ++i) { sb.Append(hash[i].ToString("x2")); }

                return sb.ToString();
            }

            public static byte[] ComputeMD5Binary(byte[] data) {
                using (MD5 cryptor = MD5.Create()) { return cryptor.ComputeHash(data); }
            }

            public static string SHA256(string text) {
                StringBuilder sb = new();
                byte[] hash = SHA256Binary(Encoding.UTF8.GetBytes(text));
                for (int i = 0; i < hash.Length; ++i) { sb.Append(hash[i].ToString("x2")); }

                return sb.ToString();
            }

            public static byte[] SHA256Binary(byte[] data) {
                using (SHA256 cryptor = System.Security.Cryptography.SHA256.Create()) {
                    return cryptor.ComputeHash(data);
                }
            }
        }

        //양방향 암호화
        public static class DuplexHash {
            public static string AES256Encrypt(string text, string password) {
                return Convert.ToBase64String(AES256EncryptBinary(Encoding.UTF8.GetBytes(text), password));
            }

            public static byte[] AES256EncryptBinary(byte[] data, string password) {
                Aes rijndaelCipher = Aes.Create();
                byte[] salt = Encoding.UTF8.GetBytes(password.Length.ToString());
                PasswordDeriveBytes secretKey = new(password, salt);
                ICryptoTransform encryptor =
                    rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                byte[] encryptedData;
                using (MemoryStream memoryStream = new()) {
                    using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write)) {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        encryptedData = memoryStream.ToArray();
                    }
                }

                return encryptedData;
            }

            public static string AES256Decrypt(string encryptedText, string password) {
                byte[] encryptedData = Convert.FromBase64String(encryptedText);
                byte[] originData = AES256DecryptBinary(encryptedData, password);
                return Encoding.UTF8.GetString(originData);
            }

            public static byte[] AES256DecryptBinary(byte[] encryptedData, string password) {
                Aes aes = Aes.Create();
                byte[] salt = Encoding.UTF8.GetBytes(password.Length.ToString());
                PasswordDeriveBytes secretKey = new(password, salt);
                ICryptoTransform decryptor = aes.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                byte[] originData;
                int decryptedCount;
                using (MemoryStream memoryStream = new(encryptedData)) {
                    using (CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read)) {
                        originData = new byte[encryptedData.Length];
                        decryptedCount = cryptoStream.Read(originData, 0, originData.Length);
                    }
                }

                return originData.Take(decryptedCount).ToArray();
            }
        }
    }
}