using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd;

/// <summary>
/// The framework class that represents the back end component.
/// </summary>
public abstract class BackEnd : MessageHandler
{
    protected ISymbolTable? SymbolTable { get; set; } = null;
    protected IIntermediateCode? IntermediateCode { get; set; } = null;

    /// <summary>
    /// Process the <paramref name="intermediateCode"/> and <paramref name="symbolTable"/>.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to process.</param>
    /// <param name="symbolTable">The symbol table to process.</param>
    public abstract void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable);
}
