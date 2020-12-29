using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static System.Console;

namespace OOPMetrics
{
    class Program
    {
        static void Main()
        {
            string input;
            Metrics Metrics;

            try
            {
                var filename = "TestClassLib.dll";
                var assembly = Assembly.UnsafeLoadFrom("../../assemblies/" + filename);
                var waitForCommand = true;
                while (waitForCommand)
                {
                    Console.WriteLine("1. Get metrics for library.\n2. Get metrics for classes.\n0. Exit.\n");
                    input = ReadLine();

                    switch (Int32.Parse(input))
                    {
                        case 1:
                            Metrics = MetricsAnalizer.GetAssemblyMetrics(assembly);
                            PrintMetrics(filename, Metrics);
                            break;
                        case 2:
                            List<ClassMetrics> MetricsList = MetricsAnalizer.GetClassMetrics(assembly.GetTypes());
                            PrintMetrics(filename, MetricsList);
                            break;
                        case 0:
                            waitForCommand = false;
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
            
        }

        private static void PrintMetrics(string input, Metrics metrics)
        {

            WriteLine ("\nASSEMBLY: {0}\nMETRICS: \n{1}\n", Path.GetFileName(input), 
                        MetricsAnalizer.MetricsToString(metrics));
        }
        
        private static void PrintMetrics(string input, List<ClassMetrics> metrics)
        {

            WriteLine("\nASSEMBLY: {0}\nMETRICS: \n{1}\n\n", Path.GetFileName(input),
                        MetricsAnalizer.MetricsToString(metrics));
        }
    }
}
