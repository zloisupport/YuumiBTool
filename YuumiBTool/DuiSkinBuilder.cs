using OpenMcdf;
using System.IO;
using System;

namespace YuumiBTool
{
    internal class DuiSkinBuilder
    {   
        

        public void Build()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(),"default");
            if (!Directory.Exists(dir))
            {

                Console.WriteLine($"Missing directory:{dir}");
                Console.WriteLine("First extract `default.db` into `default` folder");
                return;
            }

            using (var cdfFile = new CompoundFile())
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                var root = cdfFile.RootStorage;
                AddFilesAndFolder(root, directoryInfo);
                cdfFile.Save("default.db");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The default.db file has been successfully created!");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"Locate: {dir}.db");
                Console.ResetColor();
            }
        }

        private void AddFilesAndFolder(CFStorage root, DirectoryInfo directoryInfo)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                AddFile(root, file);
            }

            foreach (var subdir in directoryInfo.GetDirectories())
            {
                 AddSubStorage(root, subdir);
            }

        }

        private void AddFile(CFStorage root, FileInfo file)
        {
            byte[] data = File.ReadAllBytes(file.FullName);
            root.AddStream(file.Name).SetData(data);
        }

        private void AddSubStorage(CFStorage parentStorage, DirectoryInfo subdirectoryInfo)
        {
            var subStorage = parentStorage.AddStorage(subdirectoryInfo.Name);

            foreach (FileInfo file in subdirectoryInfo.GetFiles())
            {
                AddFile(subStorage, file);
            }
            foreach (var subdir in subdirectoryInfo.GetDirectories())
            {
                AddSubStorage(subStorage, subdir);
            }
        }


    }
}
