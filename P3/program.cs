using System;
using System.Security.Cryptography;
using System.Numerics;
using System.IO;

namespace P3
{
    class Program
    {
        static byte[] get_bytes_from_string(string Input)
        {
            var InputSplit = Input.Split(' ');
            byte[] InputBytes = new byte[InputSplit.Length];
            int i = 0;
            foreach (string Item in InputSplit)
            {
                InputBytes.SetValue(Convert.ToByte(Item, 16), i);
                i++;
            }
            return InputBytes;
        }


        static byte[] EncryptStringToBytes_AES(string PlainText, byte[] Key, byte[] IV)
        {
            // Check input arguments
            if (PlainText == null || PlainText.Length <= 0)
                throw new ArgumentNullException("PlainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] EncryptStr;


            // Create AESstr as an AES object with input Key and IV
            using (Aes AESstr = Aes.Create())
            {
                AESstr.Key = Key;
                AESstr.IV = IV;

		// Create an object (Encryptor) to perform stream transformation
                ICryptoTransform Encryptor = AESstr.CreateEncryptor(AESstr.Key, AESstr.IV);

		// Create a Memory Stream object (MSstr) for using in our  encryption
                using (MemoryStream MSstr = new MemoryStream())
                {
                    using (CryptoStream CSstr = new CryptoStream(MSstr, Encryptor, CryptoStreamMode.Write))
                    {
			// create a Stream Writer object (SWstr) to perform write operation
                        using (StreamWriter SWstr = new StreamWriter(CSstr))
                        {
                            // Write Encrypted data
                            SWstr.Write(PlainText);
                        }
			// Convert to byte string
                        EncryptStr = MSstr.ToArray();
                    }
                }
            }
            // Return value is a string of byte values
            return EncryptStr;
        }


        static string DecryptStringFromBytes_AES(byte[] CipherText, byte[] Key, byte[] IV)
        {
	    // Check input arguments
            if (CipherText == null || CipherText.Length <= 0)
                throw new ArgumentNullException("CipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

	    // Variable to hold dycrypted text
            string PlainText = null;

	    // Create AESstr as an AES object with input Key and IV and key size = 256
            using (Aes AESstr = Aes.Create())
            {
                AESstr.KeySize = 256;
                AESstr.Key = Key;
                AESstr.IV = IV;

                // Create an object (Decryptor) to perform stream transformation
                ICryptoTransform Decryptor = AESstr.CreateDecryptor(AESstr.Key, AESstr.IV);

		// Create a Memory Stream object (MSstr) for using in our  decryption
                using (MemoryStream MSstr = new MemoryStream(CipherText))
                {
                    using (CryptoStream CSstr = new CryptoStream(MSstr, Decryptor, CryptoStreamMode.Read))
                    {
			// create a Stream Read object (SRstr) to perform read operation
                        using (StreamReader SRstr = new StreamReader(CSstr))
                        {
                            // Read the decrypted bytes from the Stream Read Object (SRstr) and place it in a string.
                            PlainText = SRstr.ReadToEnd();
                        }
                    }
                }
            }

            return PlainText;
        }


        // Diffie-Hellman key is g^(xy) mod N. In the input you are given g_y which is g^y.
	// We must compute g_y^(x), we are using the BigInteger class to perform operation
        // And key = g_y^(x) mod N
        public static string P3(string[] args)
        {
            string IVstr = "";
            int n_e = 0, n_c = 0;
            BigInteger n = 0;
            BigInteger x = 0, g_y = 0;
            string CipherMsg = "";
            string PlainMsg = "";

	    int index = 1;
            foreach(var Item in args)
            {
                switch (index)
                {
                    case 1:
                        IVstr = Item;
                        break;
                    case 4:
                        n_e = int.Parse(Item);
                        break;
                    case 5:
                        n_c = int.Parse(Item);
                        break;
                    case 6:
                        x = BigInteger.Parse(Item);
                        break;
                    case 7:
                        g_y = BigInteger.Parse(Item);
                        break;
                    case 8:
                        CipherMsg = Item;
                        break;
                    case 9:
                        PlainMsg = Item;
                        break;
                    default:
                        break;
                }
                index++;
            }

	    byte[] IVbytes = get_bytes_from_string(IVstr);

            n = BigInteger.Subtract(BigInteger.Pow(2, n_e), n_c);
            BigInteger key = BigInteger.ModPow(g_y, x, n);
            byte[] KEY_bytes = key.ToByteArray();

            byte[] Encrypted = EncryptStringToBytes_AES(PlainMsg, KEY_bytes, IVbytes);

            byte[] CipherMsgbyte = get_bytes_from_string(CipherMsg);
            string Decrypted = DecryptStringFromBytes_AES(CipherMsgbyte, KEY_bytes, IVbytes);

            string Result = Decrypted + "," + BitConverter.ToString(Encrypted).Replace("-", " ");
            Console.WriteLine(Result);
            return Result;

        }
        static void Main(string[] args)
        {
            P3(args);
        }
    }
}