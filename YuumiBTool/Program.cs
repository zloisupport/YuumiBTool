using System;
using System.IO;
using OpenMcdf;

namespace YuumiBTool
{
    class Program :IAction
    {

        static void Main(string[] args)
        {
            Console.WriteLine(@"
  __  __                _ ___ ______          __
 \ \/ /_ ____ ____ _  (_) _ )_  __/__  ___  / /
  \  / // / // /  ' \/ / _  |/ / / _ \/ _ \/ / 
  /_/\_,_/\_,_/_/_/_/_/____//_/  \___/\___/_/     CLI
 
                v 1.0.0.a
Author:zlo1supp0rt@gmail.com 
date: 07-11-21 
license:Mit");
            Console.WriteLine("Command:\n" +
                "u : -unpacking StringTable.xml\n" +
                "p : -pack Resource.db\n" +
                "c : -console Clear\n"+
                "q : -quit\n");
            while (true)
            {       
              
                Console.WriteLine("Enter command: u, p, c, q" );  
                
                var inputCommand = Console.ReadLine();
                Program program = new Program();
                switch (@inputCommand)
                {
                    case "u":
                       program.StringTableExtract();
                        break;
                    case "p":
                        program.StringTablePacking();
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
            byte[] stringTable = File.ReadAllBytes("./Edit_StringTable.xml");
            if (!File.Exists(resourceDb) || !File.Exists("./Edit_StringTable.xml"))
            {
                Console.WriteLine("Error resource.db or StringTable.xml not found");
                return;
            }
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
