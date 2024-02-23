using OpenMcdf;
using System;
using System.IO;


namespace YuumiBTool
{
    internal class StringTableBuilder
    {

        public void Extract()
        {
            StringTableExtract();
        }

        public void Build()
        {
            StringTablePacking();
        }

        private void StringTablePacking()
        {
            string resourceDb = "./resource.db";
            if (!File.Exists(resourceDb) || !File.Exists("StringTable.xml"))
            {
                Console.WriteLine("Error resource.db or StringTable.xml not found");
                return;
            }

            byte[] stringTable = File.ReadAllBytes("StringTable.xml");

            using (var fs = new FileStream(resourceDb,
               FileMode.Open,
               FileAccess.ReadWrite))
            {
                using (var cf = new CompoundFile(fs, CFSUpdateMode.Update, CFSConfiguration.SectorRecycle | CFSConfiguration.EraseFreeSectors))
                {
                    CFStorage foundStream = cf.RootStorage;
                    foundStream.Delete("StringTable.xml");
                    foundStream.AddStream("StringTable.xml").SetData(stringTable);
                    cf.Commit(true);
                }
            }
            Console.WriteLine("Compressing:  StringTable.xml -> resource.db ");
            Console.WriteLine("Success!");
        }

        private void StringTableExtract()
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
            ByteArrayToFile("StringTable.xml", stringTable);
        }

        private static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(byteArray, 0, byteArray.Length);
                Console.WriteLine("Extract: resource.db -> StringTable.xml");
                Console.WriteLine("Success!");
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
