using System;
using System.IO;
using System.Security.Cryptography;


namespace P1_2
{
    class Program
    {
        public static Tuple<string,string> GetInputFromCommandLine(string[] args)
        {
            string Input1="", Input2="";
            if(args.Length == 2)
            {
                Input1 = args[0];
                Input2 = args[1];
            }else
            {
                Console.WriteLine("Incorrect input!");
            }
            return Tuple.Create(Input1, Input2);
        }

        private static double FindSeed(string PlainText, string CipherText)
        {
            DateTime First_Date = new DateTime(2020, 7, 3, 11, 0, 0);
            DateTime Second_Date = new DateTime(2020, 7, 4, 11, 0, 0);
            TimeSpan First_Time_Span = First_Date.Subtract(new DateTime(1970, 1, 1));
            TimeSpan Second_Time_Span = Second_Date.Subtract(new DateTime(1970, 1, 1));

            string SecretString = PlainText;
            int lowerBound = (int)First_Time_Span.TotalMinutes;
            int upperBound = (int)Second_Time_Span.TotalMinutes;

            int Result_Time;
            for(Result_Time = lowerBound; Result_Time <= upperBound; Result_Time++)
            {
                Random Random_Number_Gen = new Random(Result_Time);
                byte[] Key = BitConverter.GetBytes(Random_Number_Gen.NextDouble());
                if(CipherText == Encrypt(Key, SecretString))
                    return Result_Time;
            }

            return -1;
        }

        private static string Encrypt(byte[] Key, string SecretString)
        {
            DESCryptoServiceProvider Crypto_sp = new DESCryptoServiceProvider();
            MemoryStream Memory_s = new MemoryStream();
            CryptoStream Crypto_s = new CryptoStream(Memory_s, Crypto_sp.CreateEncryptor(Key, Key), CryptoStreamMode.Write);
            StreamWriter Stream_w = new StreamWriter(Crypto_s);
            Stream_w.Write(SecretString);
            Stream_w.Flush();
            Crypto_s.FlushFinalBlock();
            Stream_w.Flush();
            return Convert.ToBase64String(Memory_s.GetBuffer(), 0, (int)Memory_s.Length);
        }

        public static double P1_2(string[] args)
        {
            Tuple<string, string> commandlineInputs = GetInputFromCommandLine(args);
            string PlainText = commandlineInputs.Item1;
            string CipherText = commandlineInputs.Item2;


            double Result = FindSeed(PlainText, CipherText);
            Console.WriteLine(Result);

            return Result;

        }

        static void Main(string[] args)
        {
            P1_2(args);
        }
    }
}
