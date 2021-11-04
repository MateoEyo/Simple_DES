# Simple_DES
## Project Description
SD_DES is a binary cipher system which is a simplified design for the Data Encryption Standard (DES). Here are the specifications of this system:
- Block Size b = 8 bits
- Key Size K = 12 bits
- Number of Rounds Nr = 3

SD_DES uses scaled down 3 Fiestel rounds and a key scheduler. Both were presented in Lecture 22, in addition to a numerical example to encrypt and decrypt a single byte.  Write an application to implement SD_DES with the given specifications to do both encryption and decryption of any binary file. 

Write a report to document your SD_DES application with a discussion concerning statistical changes as the result of encryption as well as the security of this system.

## How to run
Pre-requisites are to have netcore 3.1 installed at a minimum.
You can compile using 'dotnet run' in the parent folder.
If seeking executable, executable can be found in 'C:\Users\mmate\Repos\simple_DES\bin\Debug\netcoreapp3.1'
