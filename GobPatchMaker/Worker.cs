using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GobPatchMaker
{
    public class M2
    {
        public int fileDataID;
        public string filePath;
        public int flag = 5;
    }

    public class WMO
    {
        public int fileDataID;
        public string filePath;
        public int flag = 38;
    }

    class Worker
    {
        public static string[] readedLines;
        public static List<string> matchedLines = new List<string>();
        public static List<string> ignoredLines = new List<string>();

        public static List<M2> m2Found = new List<M2>();
        public static List<WMO> wmoFound = new List<WMO>();

        public static void ReadListFile()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            readedLines = File.ReadAllLines(@"listfile_convert.txt"); //change for other users

            Console.WriteLine("Parsing your listfile.");

            foreach (string lines in readedLines)
            {
                var lineSplitted = lines.Split(null);

                if (lineSplitted[2].Contains(".m2") || lineSplitted[2].Contains(".wmo")) //if its a M2 or WMO
                {
                    if (lineSplitted[2].StartsWith("cameras/") || lineSplitted[2].StartsWith("item/") || lineSplitted[2].StartsWith("creature/") || lineSplitted[2].StartsWith("character/") && Config.ignoreUseless)
                    {
                        var showSplitted = $"{lineSplitted[0]} - {lineSplitted[2]}";
                        //Console.WriteLine("Ignoring Cameras / Items / Creatures / Characters, skip."); //because we don't want it

                        ignoredLines.Add(showSplitted);
                    }
                    else
                    {
                        var showSplitted = $"{lineSplitted[0]} - {lineSplitted[2]}"; //format for output and console write
                        //Console.WriteLine(showSplitted);

                        matchedLines.Add(showSplitted); //add found in list
                    }
                }
            }

            watch.Stop();
            Console.WriteLine($"{matchedLines.Count} gameobjects found ({watch.ElapsedMilliseconds} ms).");
        }

        public static void M2Work()
        {
            Console.WriteLine("Parsing M2.");

            foreach (string line in matchedLines)
            {
                if (line.EndsWith(".m2"))
                {
                    var lineSplitted = line.Split(" - ");

                    Regex reg = new Regex(@"\d+");
                    Match match = reg.Match(lineSplitted[0]);

                    if (match.Success)
                    {
                        M2 founded = new M2();
                        founded.fileDataID = int.Parse(match.Value);

                        if (Config.onlyFileName)
                        {
                            founded.filePath = Path.GetFileName(lineSplitted[1]);
                        }
                        else
                        {
                            founded.filePath = lineSplitted[1];
                        }

                        m2Found.Add(founded);
                    }
                }
            }
        }

        public static void WMOWork()
        {
            Console.WriteLine("Parsing WMO.");

            foreach (string line in matchedLines)
            {
                if (line.EndsWith(".wmo"))
                {
                    var lineSplitted = line.Split(" - ");

                    Regex reg = new Regex(@"\d+");
                    Match match = reg.Match(lineSplitted[0]);

                    if (match.Success)
                    {
                        WMO founded = new WMO();
                        founded.fileDataID = int.Parse(match.Value);

                        Regex reg2 = new Regex(@"\d+\.wmo");
                        Match match2 = reg2.Match(lineSplitted[1]);

                        if (!match2.Success)
                        {
                            if (Config.onlyFileName)
                            {
                                founded.filePath = Path.GetFileName(lineSplitted[1]);                         
                                wmoFound.Add(founded);
                            }
                            else
                            {
                                founded.filePath = lineSplitted[1];
                                wmoFound.Add(founded);
                            }
                        }
                    }
                }
            }
        }

        public static void WDBXWork()
        {
            Console.WriteLine($"Attempt to insert gameobject into {Config.WDBXGobDisplay}.");

            try
            {
                foreach (M2 m2 in m2Found)
                {
                    SQL.InsertIntoWDBX(m2.fileDataID, Config.db2Start);
                    Config.db2Start++;
                }

                foreach (WMO wmo in wmoFound)
                {
                    SQL.InsertIntoWDBX(wmo.fileDataID, Config.db2Start);
                    Config.db2Start++;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Gameobjects added, you can do a WDBX SQL Import in your gameobject_displayinfo.db2.");
        }

        public static void TemplateWork()
        {
            Console.WriteLine($"Attempt to insert gameobject into {Config.WorldGobTemplate}.");

            try
            {
                foreach (M2 m2 in m2Found)
                {
                    SQL.InsertIntoGameObjectWorld(Config.entryStart, m2.filePath, m2.flag);
                    Config.entryStart++;
                }

                foreach (WMO wmo in wmoFound)
                {
                    SQL.InsertIntoGameObjectWorld(Config.entryStart, wmo.filePath, wmo.flag);
                    Config.entryStart++;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine($"Gameobjects added into your {Config.WorldGobTemplate}.");
        }
    }
}
