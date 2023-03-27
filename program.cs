using System;
using System.IO;
using System.Collections;

namespace P1_1
{
    class Program
    {
        public static byte[] StegFunc(byte[] InputBytes, byte[] bmpImgBytes)
        {
            BitArray InputBits = new BitArray(InputBytes);

            byte[] TempByteArray = new byte[bmpImgBytes.Length];

            for(int i = 0; i < 26; i++)  // iterate through the first 26 bytes of the image (header)
            {
                TempByteArray[i] = bmpImgBytes[i];
            }

            for(int i = 0; i < 96; i += 2)  // iterate through the rest of the image (data)
            {
                Byte Counter;
                int ImgZoneMax = (i / 8 + 1) * 8 - 1;  // divide by 8 to get the byte number, then multiply by 8 to get the max and min of the byte
                int ImgZoneMin = (i / 8) * 8;  // divide by 8 to get the byte number, then multiply by 8 to get the max and min of the byte
                int ImageZone = ImgZoneMax - (i - ImgZoneMin);  // get the bit number of the image
                if(InputBits[ImageZone] == true && InputBits[ImageZone - 1] == true)  // if the two bits are both 1, then the counter is 3
                    Counter = 3;
                else if(InputBits[ImageZone] == true && InputBits[ImageZone - 1] == false)  // if the two bits are 1 and 0, then the counter is 2
                    Counter = 2;
                else if(InputBits[ImageZone] == false && InputBits[ImageZone - 1] == true)  // if the two bits are 0 and 1, then the counter is 1
                    Counter = 1;
                else Counter = 0;  // if the two bits are both 0, then the counter is 0
                TempByteArray[i / 2 + 26] = BitConverter.GetBytes(bmpImgBytes[i / 2 + 26] ^ Counter)[0];  // 26 header bytes, so start at 26 + i / 2 (divided by 2 because we are only looking at every other byte bc of the 2 bits per byte)
            }

            return TempByteArray;
        }

        public static string getInputFromCommandLine(string[] args)
        {
            string input = "";
            if (args.Length == 1)
            {
                input = args[0]; // Gets the first string after the 'dotnet run' command
            }
            else
            {
                Console.WriteLine("Incorrect input! use 'dotnet run' followed by a string of array HEX code");
            }
            return input;
        }


        public static string P1_1(string[] args)
        {
            byte[] bmpImgBytes = new byte[]
            {
                0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
                0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,0x18,0x00,
                0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0xFF,
                0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,
                0xFF,0xFF,0xFF,0x00,0x00,0x00,0xFF,0x00,0x00,
                0xFF,0xFF,0xFF,0xFF,0x00,0x00,0xFF,0xFF,0xFF,
                0xFF,0xFF,0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,
                0x00,0x00,0x00};

            string Input = getInputFromCommandLine(args);


            string[] InputArray=Input.Split(' ');
            byte[] InputBytes = new byte[12];

            for(int i = 0; i < InputArray.Length; i++)
            {
                InputBytes[i] = Convert.ToByte(InputArray[i], 16);
            }

            byte[] Result = StegFunc(InputBytes, bmpImgBytes);

            string OutputResult = BitConverter.ToString(Result).Replace("-", " ");
            Console.WriteLine(OutputResult);

            return OutputResult;
        }

        static void Main(string[] args)
        {
            P1_1(args);
        }

    }
}
