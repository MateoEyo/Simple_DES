using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace simple_DES
{
    class Program
    {

        const uint LEFT_KEY_BIT_MASK = 0xF00;
        const uint CENTER_KEY_BIT_MASK = 0x0F0;
        const uint RIGHT_KEY_BIT_MASK = 0x00F;
        const uint LEFT_DATA_BIT_MASK = 0xF0;
        const uint RIGHT_DATA_BIT_MASK = 0x0F;
        private static Random rand = new Random();

        static void Main(string[] args)
        {
            string title = 
                "\n-----------------------------\n" +
                "- Simple DES Implementation -\n" +
                "-----------------------------" ;

            Console.WriteLine($"{title}");

            // Get initial Key (Class example: (A,B,C))
            uint key = GetKey();

            // Get Key Rounds (Class example: (1,7,6))
            List<uint> keyRounds = GetKeyRounds(key);

            // Specify file path
            Console.WriteLine("Enter a path to a binary file:");
            string path = Console.ReadLine();
            while(File.Exists(path) == false) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Path does not exist, please enter path to existing file:");
                path = Console.ReadLine();
            }
            Console.WriteLine();
            Console.ResetColor();

            // Read in all bytes of file given earlier
            IEnumerable<uint> data = File.ReadAllBytes(path).Select(x => (uint)x);

            string plainText = Encoding.Default.GetString(
                    data.Select(x => (byte)x).ToArray()
                );

            // Encrypt plaintext using Key Scheduler
            List<uint> encryptedData = data
                .Select(x => ApplyAlgorithm(x, keyRounds))
                .ToList();

            // Reverse Keys in Key Scheduler for decryption
            keyRounds.Reverse();

            // Decrypt ciphertext using reversed key scheduler
            List<uint> decryptedData = encryptedData
                .Select(x => ApplyAlgorithm(x, keyRounds))
                .ToList();
            
            // Print out Original, Encrypted, and Decrypted data
            Console.WriteLine(String.Format("{0,-15} {1,-1}","Plaintext: ",$"{plainText}"));
            Console.Write("Original Data:  ");
            foreach (byte b in data) Console.Write($"{b:X2}");
            Console.Write("\nEncrypted Data: ");
            foreach (uint d in encryptedData) Console.Write($"{d:X2}");
            Console.Write("\nDecrypted Data: ");
            foreach (uint d in decryptedData) Console.Write($"{d:X2}");
            Console.WriteLine();

            // Convert uint list into plaintext (should be original plaintext)
            string decryptedText = Encoding.Default.GetString(
                    decryptedData.Select(x => (byte)x).ToArray()
                );
            
            // Print out human readable decrypted text (plaintext)
            Console.WriteLine($"Decrypted Text: {decryptedText}\n");

            // Write encrypted and decrypted texts to files
            try{
                File.WriteAllBytes(@"outputEncrypted.txt", encryptedData.Select(x => (byte)x).ToArray());
                File.WriteAllBytes(@"outputDecrypted.txt", decryptedData.Select(x => (byte)x).ToArray());
            } catch (Exception e) {
                Console.WriteLine("\nSomething went wrong writing out the encrypted and decrypted data.");
                Console.WriteLine($"The error is printed below\n\n{e}");
            }

            // Pause program
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // This applies the encryption/decryption of SD_DES using the appropriate version of the key scheduler
        private static uint ApplyAlgorithm(uint plainText, List<uint> keyRounds)
        {
            uint previousRound = plainText;

            foreach (uint round in keyRounds) 
                previousRound = ApplyRound(previousRound, round);

            uint encryptedData = ApplyFinalRound(previousRound);
            return encryptedData;
        }

        // 'ApplyFinalRound' is a misnomer, as it's not necessarily an additional round to the three rounds
        // but it's combining the 'left' and 'right' of the third round to produce the final output
        private static uint ApplyFinalRound(uint previousData)
        {
            uint leftPrevious = (previousData & LEFT_DATA_BIT_MASK) >> 4;
            uint rightPrevious = previousData & RIGHT_DATA_BIT_MASK;

            return (rightPrevious << 4) | leftPrevious;
        }


        // Perform bit shifting against the data given (e.g. the 8 bit data block and the 4 bit key in a given round)
        private static uint ApplyRound(uint previousData, uint keyRound)
        {
            uint leftPrevious = (previousData & LEFT_DATA_BIT_MASK) >> 4;
            uint rightPrevious = previousData & RIGHT_DATA_BIT_MASK;

            uint newLeft = rightPrevious;
            uint newRight = (rightPrevious ^ keyRound) ^ leftPrevious;

            uint newData = (newLeft << 4) | newRight;

            return newData;
        }

        // Perform bit shifting on keys and XOR operations to create key scheduler
        private static List<uint> GetKeyRounds(uint key)
        {
            uint left = (key & LEFT_KEY_BIT_MASK) >> 8;
            uint center = (key & CENTER_KEY_BIT_MASK) >> 4;
            uint right = key & RIGHT_KEY_BIT_MASK;

            return new List<uint>() {left ^ center, center ^ right, right ^ left };
        }


        // Returns an unsigned into between 0 - 4095 since all possible 12 bit keys are in that range
        public static uint GetKey() => (uint) rand.Next(0, 4095);
    }
}
