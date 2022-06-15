using System.Diagnostics;

using WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;
using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

/// <summary>
/// The top-down Pascal parser.
/// </summary>
public class PascalParserTopDown : Parser
{
    protected readonly PascalErrorHandler errorHandler = new();

    public override int ErrorCount => PascalErrorHandler.ErrorCount;

    /// <summary>
    /// Create a new Pascal top-down parser.
    /// </summary>
    /// <param name="scanner">The <see cref="Scanner"/> to be used for this parser.</param>
    public PascalParserTopDown(Scanner scanner) : base(scanner)
    {
    }

    /// <summary>
    /// Constructor for subclasses.
    /// </summary>
    /// <param name="parent">The parent parser.</param>
    protected PascalParserTopDown(PascalParserTopDown parent) : base(parent.Scanner)
        => _observers = parent._observers;

    /// <summary>
    /// Parse a Pascal source program and generate the symbol table.
    /// </summary>
    public override void Parse()
    {
        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        IntermediateCode = IntermediateCodeFactory.CreateIntermediateCode();
        try
        {
            Token token = GetNextToken();
            IIntermediateCodeNode? rootNode = null;
            if (token.TokenType == PascalTokenType.Begin)
            {
                StatementParser statementParser = new(this);
                rootNode = statementParser.Parse(token);
                Debug.Assert(CurrentToken is not null);
                token = CurrentToken;
            }
            else
            {
                errorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
            }

            if (token.TokenType != PascalTokenType.Dot)
            {
                errorHandler.Flag(token, PascalErrorCode.MissingPeriod, this);
            }
            Debug.Assert(CurrentToken is not null);
            token = CurrentToken;

            if (rootNode is not null)
            {
                IntermediateCode.Root = rootNode;
            }

            long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            float elapsedTime = (endTime - startTime) / 1000f;
            SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, elapsedTime));
        }

        catch (IOException)
        {
            errorHandler.AbortTranslation(PascalErrorCode.IOError, this);
        }
    }

    /// <summary>
    /// Synchronize the parser.
    /// </summary>
    /// <param name="synchronizationsTokenTypes">The set of token types for syncronizing the parser.</param>
    /// <returns>The token where the parser has synchronized.</returns>
    public Token Synchronize(ISet<PascalTokenType> synchronizationsTokenTypes)
    {
        Debug.Assert(CurrentToken is not null);
        Token token = CurrentToken;
        Debug.Assert(token.TokenType is not null);
        if (!synchronizationsTokenTypes.Contains(token.TokenType))
        {
            errorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
            do
            {
                token = GetNextToken();
                Debug.Assert(token.TokenType is not null);
            } while ((token is not EndOfFileToken) && !synchronizationsTokenTypes.Contains(token.TokenType));
        }

        return token;
    }
}
