using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

/// <summary>
/// The top-down Pascal parser.
/// </summary>
public class PascalParserTopDown : Parser
{
    /// <summary>
    /// Create a new Pascal top-down parser.
    /// </summary>
    /// <param name="scanner">The <see cref="Scanner"/> to be used for this parser.</param>
    public PascalParserTopDown(Scanner scanner) : base(scanner)
    {
    }

    public override int ErrorCount { get; protected set; } = 0;

    /// <summary>
    /// Parse a Pascal source program and generate the symbol table.
    /// </summary>
    public override void Parse()
    {
        Token token;
        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        while ((token = GetNextToken()) is not EndOfFileToken)
        {
            // This line intentionally left blank.
        }

        long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        float elapsedTime = (endTime - startTime) / 1000f;
        SendMessage(new ParserSummaryMessage(token.LineNumber, ErrorCount, elapsedTime));
    }
}
