using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace IOManager
{
    public class ConfigReader
    {
        public readonly string path_to_files;
        public readonly string output_path;
        public readonly int input_parallelism_degree;
        public readonly int output_parallelism_degree;
        public readonly int processing_parallelism_degree;

        public ConfigReader(bool loadParams = true,
            string input = " ", string output = " ", int ip = -5, int op = -5, int pp = -5)
        {
            if (loadParams)
            {
                try
                {
                    path_to_files = ConfigurationManager.AppSettings["PathToFiles"];
                    output_path = ConfigurationManager.AppSettings["OutputPath"];
                    input_parallelism_degree = Int32.Parse(ConfigurationManager.AppSettings["InputParallelismDegree"]);
                    output_parallelism_degree = Int32.Parse(ConfigurationManager.AppSettings["OutputParallelismDegree"]);
                    processing_parallelism_degree = Int32.Parse(ConfigurationManager.AppSettings["ProcessingParallelismDegree"]);
                }
                catch
                {
                    throw new ArgumentException("Error reading values from configuration file.");
                }
            } else
            {
                path_to_files = input;
                output_path = output;
                input_parallelism_degree = ip;
                output_parallelism_degree = op;
                processing_parallelism_degree = pp;
            }
 


            if (!Directory.Exists(path_to_files))
            {
                throw new ArgumentException("Input directory does not exist.");
            }

            if (!Directory.Exists(output_path))
            {
                try
                {
                    Directory.CreateDirectory(output_path);
                } catch
                {
                    throw new ArgumentException("Output directory does not exist and can't be created");
                }
            }

            if ((input_parallelism_degree <= 0) || 
                (output_parallelism_degree <= 0) ||
                (processing_parallelism_degree <= 0))
            {
                throw new ArgumentException("Parallelism degree must be >0");
            }

        }

    }
}
