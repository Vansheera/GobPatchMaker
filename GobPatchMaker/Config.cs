using System;
using System.Collections.Generic;
using System.Text;

namespace GobPatchMaker
{
    public class Config
    {
        public static bool ignoreUseless = true; //Ignore "character/", "creature/", "item/", "cameras/"
        public static bool onlyFileName = false;

        public static int entryStart = 30000000;
        public static int db2Start = 30000000;

        public static string SQLHost = "127.0.0.1";
        public static string SQLUser = "root";
        public static string SQLPassword = "";
        public static int SQLPort = 3307;

        public static string WorldDatabase = "world";
        public static string WorldGobTemplate = "gameobject_template";
        public static string WorldGobTag = "[Tag for Gob]";

        public static string WDBXDatabase = "wdbx";
        public static string WDBXGobDisplay = "db_gameobjectdisplayinfo_26972";
    }
}
