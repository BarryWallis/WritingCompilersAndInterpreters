using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class SymbolTableTests
{
    [TestMethod]
    public void Enter_ValidSymbolTableEntry_EntryAddedToSymbolTable()
    {
        string name = "Barry";
        SymbolTable symbolTable = new(0);

        ISymbolTableEntry symbolTableEntry = symbolTable.Enter(name);

        Assert.AreEqual(name, symbolTableEntry.Name);
        Assert.AreEqual(symbolTable, symbolTableEntry.SymbolTable);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [ExpectedException(typeof(ArgumentException))]
    public void Enter_InvalidSymbolTableEntry_ThrowsArgumentException(string name)
    {
        SymbolTable symbolTable = new(0);

        _ = symbolTable.Enter(name);
    }

    [TestMethod]
    public void Enter_DuplicateEntry_ReplacesPreviousEntry()
    {
        string name = "Barry";
        SymbolTable symbolTable = new(0);
        ISymbolTableEntry symbolTableEntry = symbolTable.Enter(name);
        symbolTableEntry.AppendLineNumber(1);

        ISymbolTableEntry newSymbolTableEntry = symbolTable.Enter(name);

        Assert.AreEqual(name, symbolTableEntry.Name);
        Assert.AreEqual(name, newSymbolTableEntry.Name);
        Assert.AreEqual(0, newSymbolTableEntry.LineNumbers.Count);
    }

    [TestMethod]
    public void Lookup_EnteredSymbolTableEntry_ReturnsTheEntry()
    {
        string name = "Barry";
        SymbolTable symbolTable = new(0);
        _ = symbolTable.Enter(name);

        ISymbolTableEntry? symbolTableEntry = symbolTable.Lookup(name);

        Assert.IsNotNull(symbolTableEntry);
        Assert.AreEqual(name, symbolTableEntry.Name);
        Assert.AreEqual(symbolTable, symbolTableEntry.SymbolTable);
    }

    [TestMethod]
    public void Lookup_NotEnteredSymbolTableEntry_ReturnsNull()
    {
        string name = "Barry";
        SymbolTable symbolTable = new(0);

        ISymbolTableEntry? symbolTableEntry = symbolTable.Lookup(name);

        Assert.IsNull(symbolTableEntry);
    }

    [TestMethod]
    public void SortedEntries_EntriesAddedUnsorted_ReturnsSortedEntries()
    {
        int count = 5;
        SymbolTable symbolTable = new(0);
        List<int> values = new();
        List<string> expected = Enumerable.Range(1, count)
                                         .Select(i => i.ToString())
                                         .ToList();
        foreach (string name in Enumerable.Range(1, count).Select(i => i.ToString()).Reverse())
        {
            _ = symbolTable.Enter(name);
        }

        List<string> actual = symbolTable.SortedEntries.Select(ste => ste.Name)
                                                    .ToList();
        CollectionAssert.AreEqual(expected, actual);
    }
}
