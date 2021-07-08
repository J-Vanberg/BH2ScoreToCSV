using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
                filePath = filePath.Trim('"');

                if (File.Exists(filePath))
                {
                    StreamReader file = new StreamReader(filePath);
                    DirectoryInfo dir = new DirectoryInfo(filePath);
                    StreamWriter csv = new StreamWriter(
                        string.Format(@"{0}\Score {1}.csv", dir.Parent, Guid.NewGuid().ToString()));
                    csv.WriteLine("Completion|Score/s|Score|Difficulty|Duration|Title & Artist");
                    int counter = 0;
                    string line;

                    // read line by line parsing any scores into the csv
                    // all of this will break if the format changes, meh
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Contains("Score/s"))
                        {
                            string completion, scorePerSec, score, difficulty, duration, title;
                            int startIndex, endIndex;

                            // completion %
                            startIndex = line.IndexOf(": ") + 2;
                            endIndex = line.IndexOf(' ', startIndex);
                            completion = line.Substring(startIndex, endIndex - startIndex);

                            // score per second
                            startIndex = line.IndexOf('=', endIndex) + 1;
                            endIndex = line.IndexOf(' ', startIndex);
                            scorePerSec = line.Substring(startIndex, endIndex - startIndex);

                            // score
                            startIndex = line.IndexOf('=', endIndex) + 1;
                            endIndex = line.IndexOf(' ', startIndex);
                            score = line.Substring(startIndex, endIndex - startIndex);

                            // difficulty
                            startIndex = line.IndexOf('=', endIndex) + 1;
                            endIndex = line.IndexOf(' ', startIndex);
                            difficulty = line.Substring(startIndex, endIndex - startIndex);

                            // duration
                            startIndex = line.IndexOf('=', endIndex) + 1;
                            endIndex = line.IndexOf(' ', startIndex);
                            duration = line.Substring(startIndex, endIndex - startIndex);

                            // title
                            startIndex = line.IndexOf('-', endIndex) + 2;
                            endIndex = Regex.Match(line, @"[C-Z][:]\\").Index;
                            if (endIndex == 0)
                            {
                                endIndex = line.LastIndexOf(" - ");
                            }
                            else
                            {
                                endIndex -= 3;
                            }
                            title = line.Substring(startIndex, endIndex - startIndex);

                            csv.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}", completion, scorePerSec, score, difficulty, duration, title);

                            counter++;
                        }
                    }

                    file.Close();
                    csv.Close();

                    Console.WriteLine("Successfully wrote {0} highscores to file.", counter);
                    invalidPath = false;
                }

                if (invalidPath)
                {
                    Console.WriteLine("Please enter a valid file path for your debug.txt.\n\n");
                }
            }

            Console.WriteLine("\n\nPress any key to exit the application...");
            Console.ReadKey();
        }
    }
}
