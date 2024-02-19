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
                "ru : -unpacking Resource.db\n" +
                "rp : -pack Resource.db\n" +
                "du : -unpacking default.db\n" +
                "dp : -pack default folder\n" +
                "c  : -console Clear\n" +
                "q  : -quit\n");
            while (true)
            {       
                Console.WriteLine("Enter command: ru, rp, du, dp, c , q");  
                
                var inputCommand = Console.ReadLine();
                var stringTableBuilder = new StringTableBuilder();
                var duiSkinBuilder = new DuiSkinBuilder();

                switch (@inputCommand.ToLower())
                {
                    case "ru":
                        stringTableBuilder.Extract();
                        break;
                    case "rp":
                        stringTableBuilder.Build();
                        break;
                    case "du":
                        duiSkinBuilder.Extract();
                        break;                  
                    case "dp":
                        duiSkinBuilder.Build();
                        break;
                    case "c":
                        Console.Clear();
                        break;
                    case "q":
                        Console.WriteLine("Good bye! =]");
                        return ;
                }
               
            }
        }


    }
}
