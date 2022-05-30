namespace WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

/// <summary>
/// A symbol table.
/// </summary>
public class SymbolTable : ISymbolTable
{
    private readonly SortedDictionary<string, ISymbolTableEntry> _symbolTableEntries = new();

    /// <inheritdoc/>
    public int NestingLevel { get; }

    /// <inheritdoc/>
    public IList<ISymbolTableEntry> SortedEntries => _symbolTableEntries.Values.ToList();

    /// <summary>
    /// Create a new symbol table.
    /// </summary>
    /// <param name="nestingLevel">The nesting level of the symbol table.</param>
    /// <exception cref="ArgumentException">Must have a value.</exception>
    public SymbolTable(int nestingLevel) => NestingLevel = nestingLevel;

    /// <inheritdoc/>
    public ISymbolTableEntry Enter(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Must have a value", nameof(name));
        }

        ISymbolTableEntry symbolTableEntry = SymbolTableFactory.CreateSymbolTableEntry(name, this);
        if (_symbolTableEntries.ContainsKey(name))
        {
            _symbolTableEntries[name] = symbolTableEntry;
        }
        else
        {
            _symbolTableEntries.Add(name, symbolTableEntry);
        }

        return symbolTableEntry;
    }

    /// <inheritdoc/>
    public ISymbolTableEntry? Lookup(string name) => _symbolTableEntries.ContainsKey(name) ? _symbolTableEntries[name] : null;
}