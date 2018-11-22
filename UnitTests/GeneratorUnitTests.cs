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

        private string classOneName;
        private string classTwoName;

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

            classOneName = "MyClassTests.cs";
            classTwoName = "GeneratedTestClassTests.cs";

            string souceClassPath = Path.Combine(sourceFilesDirectory, "MyTestClass.txt");

            sourceClassCode = File.ReadAllText(souceClassPath);

            generatedClasses = Generator.Generate(sourceClassCode).Result;

            foreach (var genClass in generatedClasses)
            {
                if (genClass.Name == classOneName)
                {
                    classOneCode = genClass.Code;
                }

                if (genClass.Name == classTwoName)
                {
                    classTwoCode = genClass.Code;
                }

            }

            classOneRoot = CSharpSyntaxTree.ParseText(classOneCode).GetCompilationUnitRoot();
            classTwoRoot = CSharpSyntaxTree.ParseText(classTwoCode).GetCompilationUnitRoot();

        }

        // Test that generator produces two separate files instead of one
        [TestMethod]
        public void TestNumberOfClasses()
        {
            Assert.AreEqual(generatedClasses.Count, 2);
        }

        // Test that code is produced by our generator.
        [TestMethod]
        public void TestProducedCode()
        {
            foreach (GeneratedTestClass gClass in generatedClasses)
            {
                if (gClass.Name == classOneName)
                {
                    Assert.IsTrue(gClass.Code == classOneCode);
                } else
                {
                    Assert.IsTrue(gClass.Code == classTwoCode);
                }
            }
        }

        // Test method count in First and Second class
        [TestMethod]
        public void TestMethodCount()
        {

            int methodsOneCount = classOneRoot.DescendantNodes().OfType<MethodDeclarationSyntax>().Count();
            Assert.AreEqual(methodsOneCount, 1);

            int methodsTwoCount = classTwoRoot.DescendantNodes().OfType<MethodDeclarationSyntax>().Count();
            Assert.AreEqual(methodsTwoCount, 2);

        }

        // Check number of class declarations inside generated classes
        [TestMethod]
        public void TestClassDeclarationCount()
        {
            int classesOneCount = classOneRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().Count();
            Assert.AreEqual(classesOneCount, 1);

            int classesTwoCount = classTwoRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().Count();
            Assert.AreEqual(classesTwoCount, 1);
        }

        // Check that namespaces count is correct
        [TestMethod]
        public void TestNamespacesCount()
        {
            int namespacesOneCount = classOneRoot.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Count();
            Assert.AreEqual(namespacesOneCount, 1);

            int namespacesTwoCount = classTwoRoot.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Count();
            Assert.AreEqual(namespacesTwoCount, 1);
        }

        // Check that class attributes are correct
        [TestMethod]
        public void TestClassAttributes()
        {
            Assert.AreEqual(1, classOneRoot.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where((classDeclaration) => classDeclaration.AttributeLists.Any((attributeList) => attributeList.Attributes
                .Any((attribute) => attribute.Name.ToString() == "TestClass"))).Count());

            Assert.AreEqual(1, classTwoRoot.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where((classDeclaration) => classDeclaration.AttributeLists.Any((attributeList) => attributeList.Attributes
                .Any((attribute) => attribute.Name.ToString() == "TestClass"))).Count());
        }

        // Check that method attributes are correct
        [TestMethod]
        public void TestMethodAttributes()
        {
            IEnumerable<MethodDeclarationSyntax> methodsOne = classOneRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();

            Assert.AreEqual(1, methodsOne.Where((methodDeclaration) => methodDeclaration.AttributeLists
                .Any((attributeList) => attributeList.Attributes.Any((attribute) => attribute.Name.ToString() == "TestMethod")))
                .Count());

            IEnumerable<MethodDeclarationSyntax> methodsTwo = classTwoRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();

            Assert.AreEqual(2, methodsTwo.Where((methodDeclaration) => methodDeclaration.AttributeLists
                .Any((attributeList) => attributeList.Attributes.Any((attribute) => attribute.Name.ToString() == "TestMethod")))
                .Count());

        }


        [TestMethod]
        public void TestMethodNames()
        {
            IEnumerable<MethodDeclarationSyntax> methodsOne = classOneRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();

            Assert.AreEqual(1, methodsOne.Where((method) => method.Identifier.ToString() == "MyMethodTest").Count());

            IEnumerable<MethodDeclarationSyntax> methodsTwo = classTwoRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();

            Assert.AreEqual(1, methodsTwo.Where((method) => method.Identifier.ToString() == "WritesomethingTest").Count());
            Assert.AreEqual(1, methodsTwo.Where((method) => method.Identifier.ToString() == "LolkekCheburekTest").Count());

        }

        [TestMethod]
        public void TestDefaultUsing()
        {
            var clsOneUsings = classOneRoot.Usings.Select(x => x.Name.ToString()).ToArray();
            var clsTwoUsings = classTwoRoot.Usings.Select(x => x.Name.ToString()).ToArray();

            var testoneUsings = new string[] { "Microsoft.VisualStudio.TestTools.UnitTesting", "System.Linq", "System", "System.Collections.Generic", "MyTestSpace" };
            var testtwoUsings = new string[] { "Microsoft.VisualStudio.TestTools.UnitTesting", "System.Linq", "System", "System.Collections.Generic", "ClassGenerator" };


            CollectionAssert.AreEquivalent(clsOneUsings, testoneUsings);
            CollectionAssert.AreEquivalent(clsTwoUsings, testtwoUsings);
        }



    }

}