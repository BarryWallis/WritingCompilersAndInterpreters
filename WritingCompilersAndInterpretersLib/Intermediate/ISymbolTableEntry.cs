using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// The symbal table entry interface.
/// </summary>
public interface ISymbolTableEntry
{
    /// <value>The name of this symbol table entry.</value>
    public string Name { get; }


    /// <value>The symbol table that contains this entry.</value>
    public ISymbolTable SymbolTable { get; }

    /// <value>The collection of line numbers.</value>
    public IReadOnlyCollection<int> LineNumbers { get; }

    /// <summary>
    /// Get an attribute associated with this symbol table entry.
    /// </summary>
    /// <param name="symbolTableKey">The symbol table key of the attribute.</param>
    /// <returns>If the attribute exists it returns the value of the attribute; otherwise it returns null.</returns>
    public object? GetAttribute(SymbolTableKey symbolTableKey);

    /// <summary>
    /// Set an attribute associated with this symbol table entry.
    /// <para>If the <paramref name="symbolTableKey"/> is already there, the <paramref name="value"/> is overridden.</para>
    /// </summary>
    /// <param name="symbolTableKey">The symbol table key of the attribute.</param>
    /// <param name="value">The value of the attribute.</param>
    public void SetAttribute(SymbolTableKey symbolTableKey, object value);

    /// <summary>
    /// Append a source line number to the entry.
    /// </summary>
    /// <param name="lineNumber">The line number to append.</param>
    public void AppendLineNumber(int lineNumber);

}