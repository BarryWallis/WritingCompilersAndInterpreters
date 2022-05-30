using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

/// <summary>
/// The top-down Pascal parser.
/// </summary>
public class PascalParserTopDown : Parser
{
    /// <summary>
    /// Handle any parser errors.
    /// </summary>
    protected static PascalErrorHandler PascalErrorHandler { get; } = new();

    public override int ErrorCount => PascalErrorHandler.ErrorCount;

    /// <summary>
    /// Create a new Pascal top-down parser.
    /// </summary>
    /// <param name="scanner">The <see cref="Scanner"/> to be used for this parser.</param>
    public PascalParserTopDown(Scanner scanner) : base(scanner)
    {
    }

    /// <summary>
    /// Parse a Pascal source program and generate the symbol table.
    /// </summary>
    public override void Parse()
    {
        Token token;
        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        try
        {
            while ((token = GetNextToken()) is not EndOfFileToken)
            {
                Debug.Assert(token.TokenType is not null);
                if (token.TokenType == PascalTokenType.Identifier)
                {
                    Debug.Assert(token.Text is not null);
                    string name = token.Text.ToLowerInvariant();
                    ISymbolTableEntry? symbolTableEntry = SymbolTableStack.Lookup(name);
                    symbolTableEntry ??= SymbolTableStack.EnterLocal(name);
                    Debug.Assert(symbolTableEntry is not null);
                    symbolTableEntry.AppendLineNumber(token.LineNumber);
                }
                else if (token.TokenType == PascalTokenType.Error)
                {
                    Debug.Assert(token.Value is PascalErrorCode);
                    PascalErrorHandler.Flag(token, (token.Value as PascalErrorCode)!, this);
                }
            }

            long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            float elapsedTime = (endTime - startTime) / 1000f;
            SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, elapsedTime));
        }
        catch (IOException)
        {
            PascalErrorHandler.AbortTranslation(PascalErrorCode.IOError, this);
        }

    }
}
