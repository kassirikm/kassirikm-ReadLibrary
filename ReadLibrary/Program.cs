using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace ReadLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            // encryption key for encryption / decryption
            byte[] key = { 0x02, 0x03, 0x01, 0x03, 0x03, 0x07, 0x07, 0x08, 0x09, 0x09, 0x11, 0x11, 0x16, 0x17, 0x19, 0x16 };
            makeEncryptionFiles(key);

            Console.WriteLine("What is jouw toegangsniveau?");
            Console.WriteLine("1) Admin");
            Console.WriteLine("2) Andere");
            var role = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Wat wil je doen?");
            Console.WriteLine("1) Lees Text File.");
            Console.WriteLine("2) Lees XML File.");
            Console.WriteLine("3) Lees Encrypted text.");
            var keuze = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("");
            Console.WriteLine("Wat is de naam van je document?");
            var input = Console.ReadLine();

            switch (keuze)
            {
                case 1:
                    if (role == 1)
                    {
                        ReadText(input);//textFile
                    }
                    else
                    {
                        Console.WriteLine("Uw toegangsniveau geeft je geen toegang tot deze file.");
                    }
                    break;
                case 2:
                    ReadXML(input);//textFile
                    break;
                case 3:
                    ReadEncryptedText(input, key);//textEncrypted
                    break;
                default:
                    Console.WriteLine("Dit keuze bestaat niet.");
                    break;
            }
        }

        static void ReadText(string input)
        {

            try
            {
                // Open de text file met stream reader.
                using (var sr = new StreamReader(input + ".txt"))
                {
                    // Lees de stream als een string, en schrijf de string naar de console.
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("De file kon niet gelezen worden:");
                Console.WriteLine(e.Message);
            }
        }

        static void ReadXML(string input)
        {

            try
            {
                // Open de Xml file met XmlTextReader.
                using (XmlTextReader xr = new XmlTextReader(input + ".xml"))
                {
                    while (xr.Read())
                    {
                        switch (xr.NodeType)
                        {
                            /*case XmlNodeType.Element: // The node is an element.
                                Console.Write("<" + xr.Name);
                                Console.WriteLine(">");
                                break;*/

                            case XmlNodeType.Text: //Display the text in each element.
                                Console.WriteLine(xr.Value);
                                break;

                                /* case XmlNodeType.EndElement: //Display the end of the element.
                                     Console.Write("</" + xr.Name);
                                     Console.WriteLine(">");
                                     break;*/
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("De file kon niet gelezen worden:");
                Console.WriteLine(e.Message);
            }

        }

        static void EncryptText(string input, byte[] key)
        {

            // ENCRYPT DATA
            try
            {
                // create file stream
                using FileStream myStream = new FileStream(input, FileMode.OpenOrCreate);

                // configure encryption key.  
                using Aes aes = Aes.Create();
                aes.Key = key;

                // store IV
                byte[] iv = aes.IV;
                myStream.Write(iv, 0, iv.Length);

                // encrypt filestream  
                using CryptoStream cryptStream = new CryptoStream(
                    myStream,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write);

                // write to filestream
                using StreamWriter sWriter = new StreamWriter(cryptStream);
                string plainText = "Welkom in de Encrypted File van Kassiri!";
                sWriter.WriteLine(plainText);

                // done 
                // Console.WriteLine("---SUCCESSFUL ENCRYPTION---\n");

            }
            catch
            {
                // error  
                Console.WriteLine("---ENCRYPTION FAILED---");
                throw;

            }

            // SHOW ENCRYPTED DATA
            /*   try
               {
                   string text = File.ReadAllText(input);

                   // encrypted data
                   Console.WriteLine("Encrypted Text Data: {0}\n\n", text);

               }
               catch(IOException e )
               {
                   Console.WriteLine(e.Message);
               }*/
        }

        static void ReadEncryptedText(string input, byte[] key)
        {


            try
            {
                // create file stream
                using FileStream myStream = new FileStream(input, FileMode.Open);

                // create instance
                using Aes aes = Aes.Create();

                // reads IV value
                byte[] iv = new byte[aes.IV.Length];
                myStream.Read(iv, 0, iv.Length);

                // decrypt data
                using CryptoStream cryptStream = new CryptoStream(
                   myStream,
                   aes.CreateDecryptor(key, iv),
                   CryptoStreamMode.Read);

                // read stream
                using StreamReader sReader = new StreamReader(cryptStream);

                // display stream
                Console.WriteLine("\n---SUCCESSFUL DECRYPTION---\n");
                Console.WriteLine("Decrypted data: {0}", sReader.ReadToEnd());
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine("De file kon niet gelezen worden:");
                Console.WriteLine(e.Message);
            }
        }

        private static void makeEncryptionFiles(byte[] key)
        {
            //make encrypted text file
            if (!File.Exists("textEncrypted.txt"))
            {
                EncryptText("textEncrypted", key);
            }
            else
            {
                File.Delete("textEncrypted.txt");
                EncryptText("textEncrypted", key);
            }
        }
    }
}
