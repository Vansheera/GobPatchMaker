using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace GobPatchMaker
{
    class SQL
    {
        public static MySqlConnection WorldConnection;
        public static MySqlConnection WDBXConnection;

        public static void ConnectToWorld()
        {
            Console.WriteLine("Attempt to connect MySQL server: World Database.");

            string WorldString = $@"server={Config.SQLHost};userid={Config.SQLUser};password={Config.SQLPassword};port={Config.SQLPort};database={Config.WorldDatabase};SslMode=none";
            WorldConnection = new MySqlConnection(WorldString);

            try
            {
                WorldConnection.Open();
                Console.WriteLine("World database connected successfully.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ConnectToWDBX()
        {
            Console.WriteLine("Attempt to connect MySQL server: WDBX Database.");

            string WDBXString = $@"server={Config.SQLHost};userid={Config.SQLUser};password={Config.SQLPassword};port={Config.SQLPort};database={Config.WDBXDatabase};SslMode=none";
            WDBXConnection = new MySqlConnection(WDBXString);

            try
            {
                WDBXConnection.Open();
                Console.WriteLine("WDBX database connected successfully.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void InsertIntoWDBX(int fileDataId, int entry)
        {
            string InsertInto = $@"INSERT INTO {Config.WDBXGobDisplay} VALUES({entry}, {fileDataId}, -0, -0, -0, -0, -0, -0, 0, 0, 0)";

            try
            {
                MySqlCommand command = WDBXConnection.CreateCommand();
                command.CommandText = InsertInto;
                command.ExecuteNonQuery();
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void InsertIntoGameObjectWorld(int entryId, string name, int type)
        {
            /* Working SQL
             * INSERT INTO gameobject_template VALUES (30000000, 0, 0, "Test Jok Request", "", "", "", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", 1)
            */

            string InsertInto = $@"INSERT INTO {Config.WorldGobTemplate}
                                VALUES ({entryId}, {type}, {entryId}, '{Config.WorldGobTag} {name}', '', '', '', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', 1)";

            try
            {
                MySqlCommand command = WorldConnection.CreateCommand();
                command.CommandText = InsertInto;
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
