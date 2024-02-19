using OpenMcdf;
using System.IO;
using System;
using SevenZipExtractor;

namespace YuumiBTool
{
    internal class DuiSkinBuilder
    {
        private string dir = Path.Combine(Directory.GetCurrentDirectory(), "default");
        public void Extract()
        {
            ExtractDefaultArchive();
        }

        private void ExtractDefaultArchive()
        {
            var defaultArchive = Path.Combine(Directory.GetCurrentDirectory(), "default.db");
            if (Directory.Exists(defaultArchive))
            {
                Console.WriteLine($"Missing file:{defaultArchive}");
                return;
            }

            Directory.CreateDirectory(dir);

            using (FileStream fs = new FileStream(defaultArchive, FileMode.Open))
            {
                using (ArchiveFile archiveFile = new ArchiveFile(fs, SevenZipFormat.Compound))
                {
                    archiveFile.Extract(dir, true);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The default.db file has been successfully extracted!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"Locate: {dir}");
                    Console.ResetColor();
                }
            }

        }

        public void Build()
        {

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
