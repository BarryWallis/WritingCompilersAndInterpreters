using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.BackEnd;
using WritingCompilersAndInterpretersLib.BackEnd.Compiler;
using WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class BackEndFactoryTests
{
    [TestMethod]
    [DataRow("compile")]
    [DataRow("COMPILE")]
    public void CreateBackEnd_CompileOperation_ReturnsCodeGenerator(string operation)
    {
        BackEnd actual = BackEndFactory.CreateBackEnd(operation);

        Assert.IsInstanceOfType(actual, typeof(CodeGenerator));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("EXECUTE")]
    public void CreateBackEnd_ExecuteOperation_ReturnsExecutor(string operation)
    {
        BackEnd actual = BackEndFactory.CreateBackEnd(operation);

        Assert.IsInstanceOfType(actual, typeof(Executor));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CreateBackEnd_InvalidOperation_ThrowsArgumentOutOfRangeException() => _ = BackEndFactory.CreateBackEnd("x");
}
