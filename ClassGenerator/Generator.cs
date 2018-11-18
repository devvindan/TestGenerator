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

        public static Task<List<GeneratedTestClass>> Generate(string code)
        {        

            return Task.Run(() => {

                List<GeneratedTestClass> generatedClasses = null;
                return generatedClasses;

            });
        }

    }
}
