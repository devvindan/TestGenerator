using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks.Dataflow;
using System.IO;
using ClassGenerator;

namespace IOManager
{
    class Program
    {
        static void Main(string[] args)
        {

            // Getting user input

            string classes_path, output_path;

            do
            {
                Console.WriteLine("Enter the path to the folder containing classes: ");
                classes_path = Console.ReadLine();
            } while (!Directory.Exists(classes_path));

            do
            {

                Console.WriteLine("Enter the path to the output folder: ");
                output_path = Console.ReadLine();
            } while (!Directory.Exists(output_path));


            // Defining Dataflow Pipeline (4 blocks)
            
            // Get file names from user input directory
            var getFileNames = new TransformManyBlock<string, string>(path =>
            {
               return Directory.GetFiles(path);
            });

            // Asynchroneously loads each file into memory
            var loadFile = new TransformBlock<string, string>(async filename =>
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    return await reader.ReadToEndAsync();
                }
            });

            // Generates test class code for each file
            var generateTestClass = new TransformBlock<string, string>(async classText =>
            {
                return await Generator.Generate(classText);
            }); 

            // Writes each file into memory.
            var printFileName = new ActionBlock<string>(name =>
            {
                Console.WriteLine(name);
            });

            getFileNames.LinkTo(loadFile);
            loadFile.LinkTo(generateTestClass);
            generateTestClass.LinkTo(printFileName);


            getFileNames.Post(classes_path);

 
            Console.ReadKey();
        }
    }
}
