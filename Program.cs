using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace simple_DES
{
    class Program
    {

        static void Main(string[] args)
        {
            // Specify file path
            string path = @"bin\file.txt";

            // Read all from path into byte array
            string fileText = File.ReadAllText(path);
            string binaryString = ToBinary(ConvertToByteArray(fileText, Encoding.ASCII));

            int numOfBytes = binaryString.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for(int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binaryString.Substring(8 * i, 8), 2);
            }

            Console.WriteLine($"fileText: {fileText}");
            Console.WriteLine($"binaryString: {binaryString}");
            Console.WriteLine($"bytes: {Encoding.ASCII.GetString(bytes)}\n");

            foreach (byte item in bytes)
            {
                byte leftHalf = GetLeftHalfByte(item);
                byte rightHalf = GetRightHalfByte(item);

                Console.WriteLine($"starting: {item}");
                Console.WriteLine($"left: {leftHalf}");
                Console.WriteLine($"right: {rightHalf}");
                byte recombined = RecombineByte(leftHalf, rightHalf);
                Console.WriteLine($"recombined: {recombined}\n");
            }
        }

        /*
            *** FUNCTIONS ***
        */

        // * Utility Functions *
        // ConvertToByteArray / ToBinary are used to convert some string into binary
        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        // Handle Byte shifting
        static byte GetLeftHalfByte(byte input)
        {
            return (byte)(input >> 4);
        }

        static byte GetRightHalfByte(byte input)
        {
            return (byte)(input << 28 >> 28);
        }

        static byte RecombineByte(byte left, byte right)
        {
            return (byte)((left << 4) ^ (right));
        }

        // * Encryption Functions *

        // * Decryption Functions *
    }
}
