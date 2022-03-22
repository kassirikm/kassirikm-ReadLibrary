using System;
using System.IO;

namespace ReadLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wat wil je doen?");
            Console.WriteLine("1) Lees Text File.");
            var keuze = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("");
            Console.WriteLine("Wat is de naam van je document?");
            var input = Console.ReadLine();

            switch (keuze)
            {
                case 1:
                    ReadText(input);//textFile
                    Console.WriteLine("Uw toegangsniveau geeft je geen toegang tot deze file.");
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
    }
}
