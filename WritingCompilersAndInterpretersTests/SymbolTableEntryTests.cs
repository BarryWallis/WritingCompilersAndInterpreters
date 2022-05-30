using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class SymbolTableEntryTests
{
    [TestMethod]
    public void Ctor_ValidSymbolTableEntry_SymbolTableEntryCreated()
    {
        string name = "barry";
        SymbolTable symbolTable = new(0);

        SymbolTableEntry actual = new(name, symbolTable);

        Assert.AreEqual(name, actual.Name);
        Assert.AreEqual(symbolTable, actual.SymbolTable);
        Assert.AreEqual(0, actual.LineNumbers.Count);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [ExpectedException(typeof(ArgumentException))]
    public void Ctor_InvalidName_ThrowsArgumentException(string name)
    {
        SymbolTable symbolTable = new(0);

        _ = new SymbolTableEntry(name, symbolTable);
    }

    [TestMethod]
    public void AppendLineNumber_ValidLineNumber_LineNumberAddedToCollection()
    {
        string name = "barry";
        int lineNumber = 123;
        SymbolTable symbolTable = new(0);
        SymbolTableEntry symbolTableEntry = new(name, symbolTable);

        symbolTableEntry.AppendLineNumber(lineNumber);

        Assert.AreEqual(lineNumber, symbolTableEntry.LineNumbers.ElementAt(0));
        Assert.AreEqual(1, symbolTableEntry.LineNumbers.Count);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void AppendLineNumber_InvalidLineNumber_ThrowsArgumentOutOfRangeExceptione(int lineNumber)
    {
        string name = "barry";
        SymbolTable symbolTable = new(0);
        SymbolTableEntry symbolTableEntry = new(name, symbolTable);

        symbolTableEntry.AppendLineNumber(lineNumber);
    }

    [TestMethod]
    public void SetAttribute_UniqueAttribute_AttributeAdded()
    {
        string name = "barry";
        SymbolTableKey symbolTableKey = SymbolTableKey.RoutineSymbolTable;
        SymbolTable symbolTable = new(0);
        SymbolTableEntry symbolTableEntry = new(name, symbolTable);

        symbolTableEntry.SetAttribute(symbolTableKey, name);

        Assert.AreEqual(name, symbolTableEntry.GetAttribute(symbolTableKey) as string);
    }

    [TestMethod]
    public void SetAttribute_DuplicateAttribute_AttributeReplaced()
    {
        string name = "barry";
        int newAttribute = 123;
        SymbolTableKey symbolTableKey = SymbolTableKey.RoutineSymbolTable;
        SymbolTable symbolTable = new(0);
        SymbolTableEntry symbolTableEntry = new(name, symbolTable);
        symbolTableEntry.SetAttribute(symbolTableKey, name);

        symbolTableEntry.SetAttribute(symbolTableKey, newAttribute);

        Assert.IsNotNull(symbolTableEntry.GetAttribute(symbolTableKey));
        Assert.AreEqual(newAttribute, (int)symbolTableEntry.GetAttribute(symbolTableKey)!);
    }

    [TestMethod]
    public void GetAttribute_AttributeInList_ReturnsAttributeValue()
    {
        string name = "barry";
        SymbolTableKey symbolTableKey = SymbolTableKey.RoutineSymbolTable;
        SymbolTable symbolTable = new(0);
        SymbolTableEntry symbolTableEntry = new(name, symbolTable);
        symbolTableEntry.SetAttribute(symbolTableKey, name);

        string? actual = symbolTableEntry.GetAttribute(symbolTableKey) as string;

        Assert.IsNotNull(actual);
        Assert.AreEqual(name, actual);
    }

    [TestMethod]
    public void GetAttribute_AttributeNotInList_ReturnsNull()
    {
        string name = "barry";
        SymbolTableKey symbolTableKey = SymbolTableKey.RoutineSymbolTable;
        SymbolTable symbolTable = new(0);
        SymbolTableEntry symbolTableEntry = new(name, symbolTable);
        symbolTableEntry.SetAttribute(symbolTableKey, name);

        string? actual = symbolTableEntry.GetAttribute(SymbolTableKey.DataValue) as string;

        Assert.IsNull(actual);
    }
}
