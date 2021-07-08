using System;
using System.IO;
using System.Collections.Generic;

namespace BH2ScoreToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BH2ScoreToCSV - Written by Justin Vandenberg");
            Console.WriteLine("===========================================");
            Console.WriteLine("Please enter the file path for your debug.txt.");
            Console.WriteLine(@"The debug file can be found in: C:\Users\<username>\Documents\ColdBeamGames\BeatHazard2\");
            Console.WriteLine("\n");

            bool invalidPath = true;

            while (invalidPath)
            {
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    try
                    {
                        StreamReader file = new StreamReader(filePath);
                        List<string[]> csv = new List<string[]> {
                            new string[] {  "Completion", "Score/s", "Score", "Difficulty", "Duration", "Title", "File Path" }
                        };
                        int counter = 0;
                        string line;

                        // read line by line parsing any scores into the csv
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Contains("Score/s"))
                            {
                                string[] data = new string[7];

                                counter++;
                            }
                        }

                        file.Close();
                    }
                    catch
                    {

                    }
                }

                if (invalidPath)
                {
                    Console.WriteLine("Please enter the file path for your debug.txt.\n\n");
                }
            }
        }
    }
}
