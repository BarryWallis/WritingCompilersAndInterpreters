namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// The framewrk sy,bol table interface.
/// </summary>
public interface ISymbolTable
{
    /// <value>The scope nesting level of this symbol table.</value>
    public int NestingLevel { get; }

    /// <value>The list of all the symbol table entries sorted in name order.</value>
    public IList<ISymbolTableEntry> SortedEntries { get; }

    /// <summary>
    /// Create and enter a new entry into the symbol table. 
    /// <para> If <paramref name="name"/> is already entered, it is replaced with a new symbol table entry.</para>
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The new entry.</returns>
    public ISymbolTableEntry Enter(string name);

    /// <summary>
    /// Look up an existing symbol table entry.
    /// </summary>
    /// <param name="name">THe entry name.</param>
    /// <returns>The new entry or <see langword="null"/> if it does not exist.</returns>
    public ISymbolTableEntry? Lookup(string name);
}