using System;
using System.Collections.Generic;
using System.IO;

namespace ToyMachine {
  class Program {
    static void Main(string[] args) {
      if (args.Length == 0) {
        Console.WriteLine("Please specify source files to run via command line arguments.");
      } else {
        Machine m = new Machine();
        int result = 0;
        try {
          foreach (string s in args) {
            Console.WriteLine("Source File " + s + "");
            List<string> source = new List<string>();
            Console.WriteLine("  Loading...");
            foreach (string line in File.ReadLines(s)) {
              source.Add(line.Trim());
            }
            m.LoadProgram(source);
            Console.WriteLine("  Running...");
            result = m.RunProgram();
          }
        } catch (IOException ioe) {
          Console.WriteLine("An error occured when loading source, please ensure source files exist.");
          Console.WriteLine();
          Console.WriteLine("Full Error Message:");
          Console.WriteLine(ioe.Message);
          return;
        }

        Console.WriteLine("Final Result is: " + result);
      }
      Console.WriteLine("Press Any Key to Exit...");
      Console.ReadKey();
    }
  }
}
