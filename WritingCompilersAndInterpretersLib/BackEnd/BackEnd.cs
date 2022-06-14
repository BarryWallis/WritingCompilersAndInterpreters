using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd;

/// <summary>
/// The framework class that represents the back end component.
/// </summary>
public abstract class BackEnd : MessageHandler
{
    protected ISymbolTableStack? SymbolTableStack { get; set; }
    protected IIntermediateCode? IntermediateCode { get; set; }

    /// <summary>
    /// Process the <paramref name="intermediateCode"/> and <paramref name="symbolTable"/>.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code to process.</param>
    /// <param name="symbolTable">The symbol table to process.</param>
    public abstract void Process(IIntermediateCode intermediateCode, ISymbolTableStack symbolTableStack);
}
