using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class SymbolTableFactoryTests
{
    [TestMethod]
    public void CreateSymbolTableEntry_ValidName_ReturnsSymbolTableEntry()
    {
        string name = "Barry";
        SymbolTable symbolTable = new(0);

        ISymbolTableEntry symbolTableEntry = SymbolTableFactory.CreateSymbolTableEntry(name, symbolTable);

        Assert.AreEqual(name, symbolTableEntry.Name);
        Assert.AreEqual(symbolTable, symbolTableEntry.SymbolTable);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateSymbolTableEntry_InvalidName_ThrowsArgumentException()
    {
        string name = " ";
        SymbolTable symbolTable = new(0);

        _ = SymbolTableFactory.CreateSymbolTableEntry(name, symbolTable);
    }
}
