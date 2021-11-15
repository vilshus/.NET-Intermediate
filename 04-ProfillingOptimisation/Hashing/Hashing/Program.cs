using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace Hashing
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            var salt = Enumerable.Repeat((byte)0x20, 16).ToArray();
            watch.Start();
            for (int i = 0; i < 1000; i++)
            {
                GeneratePasswordHashUsingSaltOriginal("password1", salt);
            }
            Console.WriteLine($"Original: {watch.ElapsedMilliseconds}");

            salt = Enumerable.Repeat((byte) 0x20, 16).ToArray();
            watch.Restart();
            for (int i = 0; i < 1000; i++)
            {
                GeneratePasswordHashUsingSaltModified("password1", salt);
            }
            Console.WriteLine($"Modified: {watch.ElapsedMilliseconds}");

            Console.WriteLine("Finished");
        }

        public static string GeneratePasswordHashUsingSaltModified(string passwordText, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, 10000);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(pbkdf2.GetBytes(20), 0, hashBytes, 8, 20);
            pbkdf2.Dispose();

            return Convert.ToBase64String(hashBytes);
        }

        public static string GeneratePasswordHashUsingSaltOriginal(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
    }
}
