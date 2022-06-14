using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.Utility;

/// <summary>
/// Generate a cross-reference listing.
/// </summary>
public class CrossReferencer
{
    private const int _nameWidth = 16;
    private const string _numbersLabel = " Line numbers    ";
    private const string _numbersUnderline = " ------------    ";
    private static readonly int _labelWidth = _numbersLabel.Length;
    private static readonly int _indentWidth = _nameWidth + _labelWidth;
#pragma warning disable IDE0052 // Remove unread private members
    private static readonly string _indent = new(' ', _indentWidth);
#pragma warning restore IDE0052 // Remove unread private members

    /// <summary>
    /// Print the cross-reference table.
    /// </summary>
    /// <param name="symbolTableStack">The symbol table stack.</param>
#pragma warning disable CA1822 // Mark members as static
    public void Print(ISymbolTableStack symbolTableStack)
#pragma warning restore CA1822 // Mark members as static
    {
        Console.WriteLine($"\n==== CROSS-REFERENCE TABLE ====");
        PrintColumnHeadings();
        PrintSymbolTable(symbolTableStack.LocalSymbolTable);
    }

    /// <summary>
    /// Print the symbol table entries.
    /// </summary>
    /// <param name="symbolTable">The symbol table.</param>
    private static void PrintSymbolTable(ISymbolTable symbolTable)
    {
        IList<ISymbolTableEntry> sortedEntries = symbolTable.SortedEntries;
        foreach (ISymbolTableEntry symbolTableEntry in sortedEntries)
        {
            IReadOnlyCollection<int>? lineNumbers = symbolTableEntry.LineNumbers;
            Console.Write($"{symbolTableEntry.Name,-_nameWidth}");
            if (lineNumbers is not null)
            {
                foreach (int lineNumber in lineNumbers)
                {
                    Console.Write($" {lineNumber:D3}");
                }
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Print column headings.
    /// </summary>
    private static void PrintColumnHeadings()
    {
        Console.WriteLine();
        Console.WriteLine($"{"Identifier",-_nameWidth}{_numbersLabel}");
        Console.WriteLine($"{"----------",-_nameWidth}{_numbersUnderline}");
    }
}
