namespace WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

/// <summary>
/// An implementation of the symbol table stack.
/// </summary>
public class SymbolTableStack : ISymbolTableStack
{
    private readonly IList<ISymbolTable> _symbolTableStack = new List<ISymbolTable>();

    /// <inheritdoc/>
    public int CurrentNestingLevel { get; }

    /// <inheritdoc/>
    public ISymbolTable LocalSymbolTable => _symbolTableStack[CurrentNestingLevel];

    /// <value>The size of the symbol table stack.</value> 
    public int SymbolTableStackSize => _symbolTableStack.Count;

    /// <summary>
    /// Create a symbol table stack.
    /// </summary>
    public SymbolTableStack()
    {
        CurrentNestingLevel = 0;
        _symbolTableStack.Add(SymbolTableFactory.CreateSymbolTable(CurrentNestingLevel));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Must have a name.</exception>
    public ISymbolTableEntry EnterLocal(string name)
        => string.IsNullOrWhiteSpace(name)
                 ? throw new ArgumentException("Must have a value", nameof(name))
                 : _symbolTableStack[CurrentNestingLevel].Enter(name);

    /// <inheritdoc/>
    public ISymbolTableEntry? Lookup(string name) => LookupLocal(name);

    /// <inheritdoc/>
    public ISymbolTableEntry? LookupLocal(string name) => _symbolTableStack[CurrentNestingLevel].Lookup(name);
}
