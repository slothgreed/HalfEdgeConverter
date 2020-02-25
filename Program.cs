using System;
using System.IO;
using HalfEdgeConverter.HEStructure;
namespace HalfEdgeConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandArgs commandArgs = new CommandArgs();
            if (!commandArgs.Parse(args))
            {
                commandArgs.ShowHelp();
                return;
            }

            Console.WriteLine("Loading STL Data : " + commandArgs.inputFile);
            STLLoader stlModel = new STLLoader(commandArgs.inputFile);
            if (!stlModel.Load())
            {
                Console.WriteLine("Failed Load STL Data");
                return;
            }

            Console.WriteLine("Loaded STL Data");
            
            
            HalfEdge half = new HalfEdge();
            Console.WriteLine("Create Half Edge Data");
            if (!half.Create(stlModel.Vertex))
            {
                Console.WriteLine("Failed HalfEdge Structure");
                return;
            }
            Console.WriteLine("Created HalfEdge Data");

            Console.WriteLine("Output File : " + commandArgs.outputFile);
            half.WriteFile(commandArgs.outputFile, commandArgs.version, commandArgs.binary);
            Console.WriteLine("Success");
            return;
        }
    }
}
