using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace P2
{
    class Program
    {
        public static string GetInputFromCommandLine(string[] args)
        {
            string Input = "";
            if (args.Length == 1)
            {
                Input = args[0];
	        //Console.WriteLine(Input);
            }
            else
            {
                Input = "C5";
                // Console.WriteLine("Incorrect input! ");
            }
            return Input;
        }
        // This function gets out plaintext and salt, then compute MD5 hash value with salt and produce output as string
        public static string Compute_MD5_With_Salt(string PlainText, string Salt)
        {
            string Result;
            byte[] Data_Before_Salt = Encoding.UTF8.GetBytes(PlainText);  // Gets an encoding for the UTF-8 format.
            byte[] Data_After_Salt = new byte[Data_Before_Salt.Length + 1];
            for(int i = 0; i < Data_Before_Salt.Length; i++)
            {
                Data_After_Salt[i] = Data_Before_Salt[i];
            }
            Data_After_Salt[Data_Before_Salt.Length] = Convert.ToByte(Salt, 16);
            // Creates an instance of the default implementation of the MD5 hash algorithm.]
            MD5 md5 = MD5.Create();
            // Computes the hash value for the specified region of the specified byte array.
	    byte[] Data = md5.ComputeHash(Data_After_Salt);
            // Converts the numeric value of each element of a specified array of bytes to its equivalent hexadecimal string representation.
	    Result = BitConverter.ToString(Data).Replace("-", " ").Substring(0, 14);
            return Result;
        }

	// This function get alphanumeric charcters and desired length as input and and get
	// a random plaintext in output
        public static string Create_Random_PlainText(string Alphanumeric_Characters, int Len)
        {
            string Result = "";
            for(int i = 0; i < Len; i++)
            {
                Random Rand = new Random();
                int Random_Number = Rand.Next(0, 36);
                Result += Alphanumeric_Characters[Random_Number];
            }
            return Result;
        }

	// Driver function of our program.
	// First gets input parameter (salt) as a command line parameter
	// Create Random Plaintext and convert it to MD5 hash with salt value
	// Then using birthday attack find collisions
	// Concatinate founded collisions (Pass1,Pass2) with each other and get as output
        public static string P2(string[] args)
        {
            string Alphanumeric_Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string Salt = GetInputFromCommandLine(args);
	    //Console.WriteLine(Salt);
            //Console.WriteLine("Please wait ...");
            string Pass1 = "";
            string Pass2 = "";
            int Len = 10;
            bool Finish = false;
            Dictionary<string, string> MD5HashWithSalt_PlainText =
                new Dictionary<string, string>();

            while(!Finish)
            {
                string New_PlainText = Create_Random_PlainText(Alphanumeric_Characters, Len);
                string New_MD5 = Compute_MD5_With_Salt(New_PlainText, Salt);
                if(MD5HashWithSalt_PlainText.ContainsKey(New_MD5) == false)
                    MD5HashWithSalt_PlainText.Add(New_MD5, New_PlainText);
                else if(MD5HashWithSalt_PlainText[New_MD5] != New_PlainText)
                {
                    Pass1 = MD5HashWithSalt_PlainText[New_MD5];
                    Pass2 = New_PlainText;
                    break;
                }
            }


//            Console.WriteLine(Compute_MD5_With_Salt(Pass1, Salt));
//            Console.WriteLine(Compute_MD5_With_Salt(Pass2, Salt));

            string P2_Result = Pass1.ToUpper() + "," + Pass2.ToUpper();
            Console.WriteLine(P2_Result);

            return P2_Result;
        }

        static void Main(string[] args)
        {
            P2(args);
        }

    }
}
