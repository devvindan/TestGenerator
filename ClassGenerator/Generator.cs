using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace ClassGenerator
{
    public static class Generator
    {

        public static int maxInputCount { get; set; } = 1;
        public static int maxProcessingCount { get; set; } = 1;
        public static int maxOutputCount { get; set; } = 1;

        
        


        public static Task<string> Generate(string code)
        {
            return Task.Run(() => { return code + "generated_test_class"; });
        }

    }
}
