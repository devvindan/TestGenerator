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
using System.Configuration;

namespace IOManager
{
    class Program
    {
        static void Main(string[] args)
        {

            // Getting Configuration
            ConfigReader config;

            try
            {
                config = new ConfigReader();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured reading a configuration file");
                Console.ReadKey();
                return;
            }


            // Defining Dataflow Pipeline (4 blocks)

            // set up dataflow parameters
            ExecutionDataflowBlockOptions inputOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = config.input_parallelism_degree
            };

            ExecutionDataflowBlockOptions processingOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = config.processing_parallelism_degree
            };

            ExecutionDataflowBlockOptions outputOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = config.output_parallelism_degree
            };



            // 1. Get file names from user input directory
            var getFileNames = new TransformManyBlock<string, string>(path =>
            {
               return Directory.GetFiles(path);
            });

            // 2. Asynchroneously loads each file into memory
            var loadFile = new TransformBlock<string, string>(async filename =>
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    return await reader.ReadToEndAsync();
                }
            }, inputOptions);

            // 3. Generates test classes code for each file (1 to 1 or many)
            var generateTestClass = new TransformManyBlock<string, GeneratedTestClass>(async classText =>
            {
                return await Generator.Generate(classText);
            }, processingOptions); 

            // 4. Writes each file into memory.
            var writeGeneratedFile = new ActionBlock<GeneratedTestClass>(async testClass =>
            {
                string fullpath = Path.Combine(config.output_path, testClass.Name);
                using (StreamWriter writer = new StreamWriter(fullpath))
                {
                    await writer.WriteAsync(testClass.Code);
                }
            }, outputOptions);

            getFileNames.LinkTo(loadFile);
            loadFile.LinkTo(generateTestClass);
            generateTestClass.LinkTo(writeGeneratedFile);

            getFileNames.Post(config.path_to_files);
 
            Console.ReadKey();
        }
    }
}
