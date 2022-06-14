namespace WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

/// <summary>
/// A symbol table entry.
/// </summary>
public class SymbolTableEntry : ISymbolTableEntry
{
    private readonly IDictionary<SymbolTableKey, object> _attributes = new Dictionary<SymbolTableKey, object>();

    private readonly ICollection<int> _lineNumbers = new List<int>();
    /// <inheritdoc/>
    public IReadOnlyCollection<int> LineNumbers => _lineNumbers.ToList();

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public ISymbolTable SymbolTable { get; }

    /// <summary>
    /// Create a symbmol table entry.
    /// </summary>
    /// <param name="name">The entry name.</param>
    /// <param name="symbolTable">The symbol table that contains th entry.</param>
    /// <exception cref="ArgumentException">Must have a value.</exception>
    public SymbolTableEntry(string name, ISymbolTable symbolTable)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Must have a value", nameof(name)) : name;
        SymbolTable = symbolTable;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="lineNumber"/> must be positive.</exception>
    public void AppendLineNumber(int lineNumber)
    {
        if (lineNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "Must be positive");
        }

        _lineNumbers.Add(lineNumber);
    }

    /// <inheritdoc/>
    public object? GetAttribute(SymbolTableKey symbolTableKey)
        => _attributes.ContainsKey(symbolTableKey) ? _attributes[symbolTableKey] : null;

    /// <inheritdoc/>
    public void SetAttribute(SymbolTableKey symbolTableKey, object value)
    {
        if (_attributes.ContainsKey(symbolTableKey))
        {
            _attributes[symbolTableKey] = value;
        }
        else
        {
            _attributes.Add(symbolTableKey, value);
        }
    }
}