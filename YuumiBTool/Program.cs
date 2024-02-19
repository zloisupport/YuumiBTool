using System;
using System.IO;
using OpenMcdf;

namespace YuumiBTool
{
    class Program :IAction
    {

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"
  __  __                _ ___ ______          __
 \ \/ /_ ____ ____ _  (_) _ )_  __/__  ___  / /
  \  / // / // /  ' \/ / _  |/ / / _ \/ _ \/ / 
  /_/\_,_/\_,_/_/_/_/_/____//_/  \___/\___/_/     CLI
 
                v 1.0.1.a
Author:zlo1supp0rt@gmail.com 
date: 07-11-24 
license:MIT");
            Console.ResetColor();
            Console.WriteLine("Command:\n" +
                "ru : -unpacking StringTable.xml\n" +
                "rp : -pack Resource.db\n" +
                "du : -unpacking \n" +
                "dp : -pack default folder\n" +
                "c  : -console Clear\n" +
                "q  : -quit\n");
            while (true)
            {       
                Console.WriteLine("Enter command: ru, rp, du, dp, c , q");  
                
                var inputCommand = Console.ReadLine();
                Program program = new Program();
                var duiSkinBuilder = new DuiSkinBuilder();

                switch (@inputCommand.ToLower())
                {
                    case "ru":
                       program.StringTableExtract();
                        break;
                    case "rp":
                        program.StringTablePacking();
                        break;
                    case "du":
                        
                        break;                  
                    case "dp":
                        duiSkinBuilder.Build();
                        break;
                    case "c":
                        Console.Clear();
                        break;
                    case "q":
                        Console.WriteLine("Good bye! :😉");
                        return ;
                }
               
            }
        }

        public void StringTablePacking()
        {
            string resourceDb = "./resource.db";
            if (!File.Exists(resourceDb) || !File.Exists("./Edit_StringTable.xml"))
            {
                Console.WriteLine("Error resource.db or StringTable.xml not found");
                return;
            }
          
            byte[] stringTable = File.ReadAllBytes("./Edit_StringTable.xml");

            using (var fs = new FileStream(resourceDb,
               FileMode.Open,
               FileAccess.ReadWrite)) {
                using (var cf = new CompoundFile(fs,   CFSUpdateMode.Update,CFSConfiguration.SectorRecycle | CFSConfiguration.EraseFreeSectors))
                {
                    CFStorage foundStream = cf.RootStorage;
                    foundStream.Delete("StringTable.xml");
                    foundStream.AddStream("StringTable.xml").SetData(stringTable);
                    cf.Commit(true);
                }
             }
            Console.WriteLine("Packing:  Edit_StringTable.xml -> resource.db ");
            Console.WriteLine("Success!");
        }

        public void StringTableExtract()
        {
            string resourceDb = "resource.db";
            if (!File.Exists(resourceDb))
            {
                Console.WriteLine("Error resource.db not found");
                return;
            }

            CompoundFile cf = new CompoundFile(resourceDb, CFSUpdateMode.Update,
                CFSConfiguration.EraseFreeSectors | CFSConfiguration.SectorRecycle);
            CFStream foundStream = cf.RootStorage.GetStream("StringTable.xml");
            byte[] stringTable = foundStream.GetData();
            cf.Close();
            ByteArrayToFile("Unpacked_StringTable.xml", stringTable);
            File.Copy("./Unpacked_StringTable.xml", "./Edit_StringTable.xml", true);
        }

        private static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(byteArray, 0, byteArray.Length);
                Console.WriteLine("StringTable.xml unpacked! -> Unpacking_StringTable.xml->Edit_StringTable.xml");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

    }
}
