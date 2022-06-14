namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// The symbol table stack interface.
/// </summary>
public interface ISymbolTableStack
{
    /// <value>The current nesting level.</value>
    public int CurrentNestingLevel { get; }

    /// <value>The local symbol table at the top of the stack.</value>
    public ISymbolTable LocalSymbolTable { get; }

    /// <summary>
    /// Create and enter a new ebtry in the local symbol table.
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The new entry.</returns>
    public ISymbolTableEntry? EnterLocal(string name);

    /// <summary>
    /// Look up an existing symbol table entry in the local symbol table. 
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The symbol table entry or <see langword="null"/> if it does not exist.</returns>
    public ISymbolTableEntry? LookupLocal(string name);

    /// <summary>
    /// Look up an existing symbol table entry throughout the entire stack. 
    /// </summary>
    /// <param name="name">The name of the entry.</param>
    /// <returns>The symbol table entry or <see langword="null"/> if it does not exist.</returns>
    public ISymbolTableEntry? Lookup(string name);
}
