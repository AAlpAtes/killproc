using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace killproc
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: Just run the executable with the filters seperated by space. You can use \"*\" or \"?\" as wildcard. It is case insensitive. Do not forget to \"Run as Administrator\"");
                Console.WriteLine("");
                Console.WriteLine("killproc filter1 [filter2] [filter3]...");
                Console.WriteLine("");
                Console.WriteLine("Ex: killproc notepad* *paint ");
                return;
            }

            try
            {
                List<Process> processes2Kill = new List<Process>();
                foreach (Process p in Process.GetProcesses())
                {
                    foreach (string s in args)
                    {
                        if (Like(p.ProcessName.ToLowerInvariant(), s.ToLowerInvariant().Replace("*", "%").Replace(".exe", "")))
                        {
                            Console.WriteLine(p.Id + " - " + p.ProcessName);
                            processes2Kill.Add(p);
                        }
                    }
                }

                Console.WriteLine("Are you sure to kill these processes? y/n?");
                if (Console.ReadKey().KeyChar.ToString().ToLowerInvariant() == "y")
                {
                    foreach (Process p in processes2Kill)
                    {
                        p.Kill();
                    }
                    Console.WriteLine("");
                    Console.WriteLine(processes2Kill.Count.ToString() + " number of processes killed.");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Cancelled.");
                }

            }
            catch (Exception E) {
                Console.WriteLine("Error: " + E.ToString());
            }

        }

        public static bool Like(string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }
    }

}
