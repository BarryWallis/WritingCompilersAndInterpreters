using System.Diagnostics;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

/// <summary>
/// Pascal token types.
/// </summary>
public record PascalTokenType : TokenType
{
    public static readonly IList<PascalTokenType> TokenTypes = new List<PascalTokenType>();
    public static readonly IDictionary<PascalTokenType, string> SpecialSymbols = new Dictionary<PascalTokenType, string>();
    public static readonly IDictionary<string, PascalTokenType> LookupSpecialSymbols
        = new Dictionary<string, PascalTokenType>();
    public static readonly ISet<string> ReservedWords = new HashSet<string>();

    //Reserved words
    public static readonly PascalTokenType And = new("And");
    public static readonly PascalTokenType Array = new("Array");
    public static readonly PascalTokenType Begin = new("Begin");
    public static readonly PascalTokenType Case = new("Case");
    public static readonly PascalTokenType Const = new("Const");
    public static readonly PascalTokenType Div = new("Div");
    public static readonly PascalTokenType Do = new("Do");
    public static readonly PascalTokenType Downto = new("Downto");
    public static readonly PascalTokenType Else = new("Else");
    public static readonly PascalTokenType End = new("End");
    public static readonly PascalTokenType File = new("File");
    public static readonly PascalTokenType For = new("For");
    public static readonly PascalTokenType Function = new("Function");
    public static readonly PascalTokenType Goto = new("Goto");
    public static readonly PascalTokenType If = new("If");
    public static readonly PascalTokenType In = new("In");
    public static readonly PascalTokenType Label = new("Label");
    public static readonly PascalTokenType Mod = new("Mod");
    public static readonly PascalTokenType Nil = new("Nil");
    public static readonly PascalTokenType Not = new("Not");
    public static readonly PascalTokenType Of = new("Of");
    public static readonly PascalTokenType Or = new("Or");
    public static readonly PascalTokenType Packed = new("Packed");
    public static readonly PascalTokenType Procedure = new("Procedure");
    public static readonly PascalTokenType Program = new("Program");
    public static readonly PascalTokenType Record = new("Record");
    public static readonly PascalTokenType Repeat = new("Repeat");
    public static readonly PascalTokenType Set = new("Set");
    public static readonly PascalTokenType Then = new("Then");
    public static readonly PascalTokenType To = new("To");
    public static readonly PascalTokenType Type = new("Type");
    public static readonly PascalTokenType Until = new("Until");
    public static readonly PascalTokenType Var = new("Var");
    public static readonly PascalTokenType While = new("While");
    public static readonly PascalTokenType With = new("With");

    // Special symbols
    public static readonly PascalTokenType Plus = new("Plus");
    public static readonly PascalTokenType Minus = new("Minus");
    public static readonly PascalTokenType Star = new("Star");
    public static readonly PascalTokenType Slash = new("Slash");
    public static readonly PascalTokenType ColonEquals = new("ColonEquals");
    public static readonly PascalTokenType Dot = new("Dot");
    public static readonly PascalTokenType Comma = new("Comma");
    public static readonly PascalTokenType Semicolon = new("Semicolon");
    public static readonly PascalTokenType Colon = new("Colon");
    public static readonly PascalTokenType Quote = new("Quote");
    public static readonly PascalTokenType Equal = new("Equals");
    public static readonly PascalTokenType NotEqual = new("NotEquals");
    public static readonly PascalTokenType LessThan = new("LessThan");
    public static readonly PascalTokenType LessEquals = new("LessEquals");
    public static readonly PascalTokenType GreaterEquals = new("GreaterEquals");
    public static readonly PascalTokenType GreaterThan = new("GreaterThan");
    public static readonly PascalTokenType LeftParen = new("LeftParen");
    public static readonly PascalTokenType RightParen = new("RightParen");
    public static readonly PascalTokenType LeftBracket = new("LeftBracket");
    public static readonly PascalTokenType RightBracket = new("RightBracket");
    public static readonly PascalTokenType LeftBrace = new("LeftBrace");
    public static readonly PascalTokenType RightBrace = new("RightBrace");
    public static readonly PascalTokenType UpArrow = new("UpArrow");
    public static readonly PascalTokenType DotDot = new("DotDot");

    // Primitives
    public static readonly PascalTokenType Identifier = new("Identifier");
    public static readonly PascalTokenType Integer = new("Integer");
    public static readonly PascalTokenType Real = new("Real");
    public static readonly PascalTokenType String = new("String");
    public static readonly PascalTokenType Error = new("Error");
    public static readonly PascalTokenType EndOfFile = new("EndOfFile");

    private static readonly int _firstReservedIndex;
    private static readonly int _lastReservedIndex;

    private static readonly int _firstSpecialSymbolIndex;
    private static readonly int _lastSpecialSymbolIndex;

    public string Text { get; init; }

    static PascalTokenType()
    {
        _firstReservedIndex = TokenTypes.IndexOf(And);
        Debug.Assert(_firstReservedIndex >= 0);
        _lastReservedIndex = TokenTypes.IndexOf(With);
        Debug.Assert(_lastReservedIndex >= 0);
        _firstSpecialSymbolIndex = TokenTypes.IndexOf(Plus);
        Debug.Assert(_firstSpecialSymbolIndex >= 0);
        _lastSpecialSymbolIndex = TokenTypes.IndexOf(DotDot);
        Debug.Assert(_lastSpecialSymbolIndex >= 0);

        SpecialSymbols[Plus] = "+";
        SpecialSymbols[Minus] = "-";
        SpecialSymbols[Star] = "*";
        SpecialSymbols[Slash] = "/";
        SpecialSymbols[ColonEquals] = ":=";
        SpecialSymbols[Dot] = ".";
        SpecialSymbols[Comma] = ",";
        SpecialSymbols[Semicolon] = ";";
        SpecialSymbols[Colon] = ":";
        SpecialSymbols[Quote] = "\"";
        SpecialSymbols[Equal] = "=";
        SpecialSymbols[NotEqual] = "<>";
        SpecialSymbols[LessThan] = "<";
        SpecialSymbols[LessEquals] = "<=";
        SpecialSymbols[GreaterEquals] = ">=";
        SpecialSymbols[GreaterThan] = ">";
        SpecialSymbols[LeftParen] = "(";
        SpecialSymbols[RightParen] = ")";
        SpecialSymbols[LeftBracket] = "[";
        SpecialSymbols[RightBracket] = "]";
        SpecialSymbols[LeftBrace] = "{";
        SpecialSymbols[RightBrace] = "}";
        SpecialSymbols[UpArrow] = "^";
        SpecialSymbols[DotDot] = "..";

        for (int i = _firstReservedIndex; i <= _lastReservedIndex; i++)
        {
            bool success = ReservedWords.Add(TokenTypes[i].Text);
            Debug.Assert(success);
        }

        for (int i = _firstSpecialSymbolIndex; i <= _lastSpecialSymbolIndex; i++)
        {
            LookupSpecialSymbols[SpecialSymbols[TokenTypes[i]]] = TokenTypes[i];
        }
    }

    public PascalTokenType(string text)
    {
        Text = string.IsNullOrWhiteSpace(text) ? throw new ArgumentException("Must not be empty", nameof(text))
                                               : text.ToLowerInvariant();
        TokenTypes.Add(this);
    }
}
