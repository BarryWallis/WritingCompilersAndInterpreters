using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class SymbolTableStackTests
{
    [TestMethod]
    public void EnterLocal_ValidName_EnteredIntoLocalSymbolTable()
    {
        string name = "Barry";
        SymbolTableStack symbolTableStack = new();

        ISymbolTableEntry actual = symbolTableStack.EnterLocal(name);

        Assert.AreEqual(name, actual.Name);
        Assert.AreEqual(name, symbolTableStack.LocalSymbolTable.SortedEntries[0].Name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EnterLocal_InvalidName_ThrowsArgumentException()
    {
        string name = " ";
        SymbolTableStack symbolTableStack = new();

        _ = symbolTableStack.EnterLocal(name);
    }

    [TestMethod]
    public void LookupLocal_NameHasBeenEntered_ReturnsSymbolTableEntry()
    {
        string name = "Barry";
        SymbolTableStack symbolTableStack = new();
        ISymbolTableEntry? symbolTableEntry = symbolTableStack.EnterLocal(name);

        ISymbolTableEntry? actual = symbolTableStack.LookupLocal(name);

        Assert.IsNotNull(actual);
        Assert.AreEqual(symbolTableEntry.Name, actual.Name);
    }

    [TestMethod]
    public void LookupLocal_NameHasNotBeenEntered_ReturnsNull()
    {
        string name = "Barry";
        SymbolTableStack symbolTableStack = new();

        ISymbolTableEntry? actual = symbolTableStack.LookupLocal(name);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void Lookup_NameHasBeenEntered_ReturnsSymbolTableEntry()
    {
        string name = "Barry";
        SymbolTableStack symbolTableStack = new();
        ISymbolTableEntry? symbolTableEntry = symbolTableStack.EnterLocal(name);

        ISymbolTableEntry? actual = symbolTableStack.Lookup(name);

        Assert.IsNotNull(actual);
        Assert.AreEqual(symbolTableEntry.Name, actual.Name);
    }

    [TestMethod]
    public void Lookup_NameHasNotBeenEntered_ReturnsNull()
    {
        string name = "Barry";
        SymbolTableStack symbolTableStack = new();

        ISymbolTableEntry? actual = symbolTableStack.Lookup(name);

        Assert.IsNull(actual);
    }
}
