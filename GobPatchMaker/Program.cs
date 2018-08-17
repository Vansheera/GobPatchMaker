using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GobPatchMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            SQL.ConnectToWorld();
            SQL.ConnectToWDBX();

            Worker.ReadListFile();
            Worker.M2Work();
            Worker.WMOWork();

            //SQL Working
            Worker.WDBXWork();
            Worker.TemplateWork();

            watch.Stop();
            Console.WriteLine("Gameobject Patch generated in {0} ms.", watch.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
