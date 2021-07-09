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
            Console.WriteLine("BH2ScoreToCSV");
            Console.WriteLine("===========================================");
            Console.WriteLine("To dump your high scores: press the 'd' key while viewing the settings screen in Beat Hazard 2.");
            Console.WriteLine("Your scores will then be dumped into the debug.txt file for the game.\n");

            Console.WriteLine(@"The debug file can be found in: C:\Users\<username>\Documents\ColdBeamGames\BeatHazard2\");

            Console.WriteLine("\nDrag and drop your debug.txt file onto this window then press enter.");
            Console.WriteLine("\n");

            bool invalidPath = true;

            while (invalidPath)
            {
                bool caught = false;
                string filePath = Console.ReadLine();
                Console.WriteLine("\n");
                filePath = filePath.Trim('"');

                if (File.Exists(filePath))
                {
                    try
                    {
                        using (StreamReader file = new StreamReader(filePath))
                        {
                            DirectoryInfo dir = new DirectoryInfo(filePath);
                            string csvPath = string.Format(@"{0}\High Score Dump.csv", dir.Parent);
                            using (StreamWriter csv = new StreamWriter(csvPath))
                            {
                                csv.WriteLine("Completion|Score/s|Score|Difficulty|Duration|Title & Artist|Open Mic");
                                int counter = 0;
                                int numDumps = 0;
                                string line;

                                // read line by line parsing any scores into the csv
                                // all of this will break if the format changes, meh
                                while ((line = file.ReadLine()) != null)
                                {
                                    if (line.Contains("Num High Scores:"))
                                    {
                                        numDumps++;
                                    }

                                    if (line.Contains("Score/s"))
                                    {
                                        string completion, scorePerSec, score, difficulty, duration, title, openMic;
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
                                            openMic = "Y";
                                        }
                                        else
                                        {
                                            endIndex -= 3;
                                            openMic = "";
                                        }
                                        title = line.Substring(startIndex, endIndex - startIndex);

                                        if (title.EndsWith(" by "))
                                        {
                                            title = title.Substring(0, title.Length - 4);
                                        }

                                        csv.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}", completion, scorePerSec, score, difficulty, duration, title, openMic);

                                        counter++;
                                    }
                                }

                                if (numDumps > 0)
                                {
                                    Console.WriteLine("Successfully wrote {0} high scores to High Score Dump.csv.", counter);
                                    if (numDumps > 1)
                                    {
                                        Console.WriteLine("{0} high score dumps were found, output may contain duplicates.", numDumps);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No high score dumps were found.", counter);
                                }
                                invalidPath = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to parse debug.txt: {0}", e.Message);
                        caught = true;
                    }
                }

                if (invalidPath && !caught)
                {
                    Console.WriteLine("Please enter a valid file path for your debug.txt.\n\n");
                }
            }

            Console.WriteLine("\n\nPress any key to exit the application...");
            Console.ReadKey();
        }
    }
}
