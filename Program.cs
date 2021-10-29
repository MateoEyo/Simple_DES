using System;
using System.IO;
using System.Linq;
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

            // Convert all text from file into binary
            string binaryString = ToBinary(ConvertToByteArray(fileText, Encoding.ASCII));

            string key = GetKey();

            // Convert key to binary string
            string binaryKey = String.Join(String.Empty,
                key.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            int numOfBytes = binaryString.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for(int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binaryString.Substring(8 * i, 8), 2);
            }

            Console.WriteLine($"fileText: {fileText}");
            Console.WriteLine($"binaryString: {binaryString}");
            Console.WriteLine($"bytes: {Encoding.ASCII.GetString(bytes)}");
            Console.WriteLine($"key: {key}");
            Console.WriteLine($"binary key: {binaryKey}\n");
            
            string[] feistelKeys = GetKeyXORS(binaryKey);

            string ciphertext = EncryptPlainText(binaryString, feistelKeys);
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

        // * Key related functions
        // Returns a hex string of 12
        static string GetKey()
        {
            System.Random random = new System.Random();
            int num = random.Next(256, 4095);
            return num.ToString("X");
        }

        // Returns a 3 element array of 4 bit keys used for SD_DES
        static string[] GetKeyXORS(string key)
        {
            const int SIZE = 3;
            string[] preFeistelRounds = new string[SIZE];
            string[] postFeistelRounds = new string[SIZE];
            
            preFeistelRounds[0] = key.Substring(0, 4);
            preFeistelRounds[1] = key.Substring(4, 4);
            preFeistelRounds[2] = key.Substring(8, 4);

            postFeistelRounds[0] = GetXORFromElements(preFeistelRounds[0], preFeistelRounds[1]);
            postFeistelRounds[1] = GetXORFromElements(preFeistelRounds[1], preFeistelRounds[2]);
            postFeistelRounds[2] = GetXORFromElements(preFeistelRounds[2], preFeistelRounds[0]);

            return postFeistelRounds;
        }

        // Return 4 bit XOR element
        static string GetXORFromElements(string first, string second)
        {
            int i = 0;
            string xorToReturn = String.Empty;

            foreach (char c in first)
            {
                if (c == second[i])
                {
                    xorToReturn += '0';
                } else {
                    xorToReturn += '1';
                }
                i++;
            }

            return xorToReturn;
        }

        // * Encryption Functions *
        static string EncryptPlainText(string plaintext, string[] keys)
        {
            

            return null;
        }

        // * Decryption Functions *
    }
}
