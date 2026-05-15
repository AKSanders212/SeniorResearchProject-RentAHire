/*
 * Aaron Keith Sanders
 * G00225605
 * UA Grantham
 * IS498: Senior Research Project
 * Robert Chubbuck
 * Senior Capstone Project: Rent-A-Hire Hybrid Mobile App
 * File: PasswordHelper.cs
 * Desc: A class used to hash passwords for the app and to verify their match for login
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace SandersCapstoneProject
{
    public static class PasswordHelper
    {
        // Class method used to hash passwords using a salt (randomly generated string applied before hash algorithm)
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16); // Applies a random string as an array of random bytes

            // Hash the password with an applied salt, number of iterations and the SHA256 Hash Algorithm
            // Password base key derivation function 2 - pbkdf2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Sum the salt and the hash to the password
            byte[] hashBytes = new byte[48];
            // Copies the range of elements in the salt array at index 0 into hashBytes to the destination index 0
            // with a length of 16, the hash is copied at index 0 into hashBytes at index 16 with a length of 32
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        // Class method used to verify the password for login based on its stored hashed password
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Convert the storedHash password from its base64 string into a byte[] array
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16]; // Create the salt and then store into the salt the parts of the converted hashBytes
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Create the password base key derivation function 
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // For loop to compare the hashBytes of the hash password, where the hash should begin at index 16,
            // to determine if theres a match of the hash, if not, return false, otherwise, return true
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
