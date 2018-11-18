using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using IOManager;
using static ClassGenerator.Generator;
using ClassGenerator;

namespace UnitTests
{
    [TestClass]
    public class GeneratorUnitTests
    {

        private string classOneCode;
        private string classTwoCode;
        private string sourceClassCode;

        private List<GeneratedTestClass> generatedClasses = null;

        private CompilationUnitSyntax classOneRoot;
        private CompilationUnitSyntax classTwoRoot;

        [TestInitialize]
        public void TestInit()
        {
            ConfigReader config = new ConfigReader();

            string sourceFilesDirectory = config.path_to_files;
            string generatedFilesDirectory = config.output_path;

            string pathClassOne = Path.Combine(generatedFilesDirectory, "MyClassTests.cs");
            string pathClassTwo = Path.Combine(generatedFilesDirectory, "GeneratedTestClassTests.cs");

            string souceClassPath = Path.Combine(sourceFilesDirectory, "MyTestClass.txt");


            classOneCode = File.ReadAllText(pathClassOne);
            classTwoCode = File.ReadAllText(pathClassTwo);
            sourceClassCode = File.ReadAllText(souceClassPath);

            classOneRoot = CSharpSyntaxTree.ParseText(classOneCode).GetCompilationUnitRoot();
            classTwoRoot = CSharpSyntaxTree.ParseText(classTwoCode).GetCompilationUnitRoot();

            generatedClasses = Generator.Generate(sourceClassCode).Result;



        }

        [TestMethod]
        // Test that generator produces two separate files instead of one
        public void TestNumberOfClasses()
        {
            Assert.AreEqual(generatedClasses.Count, 2);
        }

        

    }
}