using System;
using BCrypt.Net;

class Program
{
    static void Main(string[] args)
    {
        string password = "1234";

        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine($"Generated hash: {hash}");

        bool isMatch = BCrypt.Net.BCrypt.Verify(password, hash);
        Console.WriteLine($"Matches? {isMatch}");
    }
}

