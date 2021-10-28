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
            Console.WriteLine(binaryString);
        }

        /*
            *** FUNCTIONS ***
        */

        // ConvertToByteArray / ToBinary are used to convert some string into binary
        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }
    }
}
