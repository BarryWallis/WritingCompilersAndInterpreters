namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

public record PascalErrorCode
{
    private static int _errorCodeCount = 0;

    public static readonly PascalErrorCode AlreadyForwarded = new("Already Specified In Forward");
    public static readonly PascalErrorCode CaseConstantReused = new("Case Constant Reused");
    public static readonly PascalErrorCode IdentifierRedefined = new("Redefined Identifier");
    public static readonly PascalErrorCode IdentifierUndefined = new("Undefined Identifier");
    public static readonly PascalErrorCode IncompatibleAssignment = new("Incompatible Assignment");
    public static readonly PascalErrorCode IncompatibleTypes = new("Incompatible Types");
    public static readonly PascalErrorCode InvalidAssignment = new("Invalid Assignment Statement");
    public static readonly PascalErrorCode InvalidCharacter = new("Invalid Character");
    public static readonly PascalErrorCode InvalidConstant = new("Invalid Constant");
    public static readonly PascalErrorCode InvalidExponent = new("Invalid Exponent");
    public static readonly PascalErrorCode InvalidExpression = new("Invalid Expression");
    public static readonly PascalErrorCode InvalidField = new("Invalid Field");
    public static readonly PascalErrorCode InvalidFraction = new("Invalid Fraction");
    public static readonly PascalErrorCode InvalidIdentifierUsage = new("Invalid Identifier Usage");
    public static readonly PascalErrorCode InvalidIndexType = new("Invalid Index Type");
    public static readonly PascalErrorCode InvalidNumber = new("Invalid Number");
    public static readonly PascalErrorCode InvalidStatement = new("Invalid Statement");
    public static readonly PascalErrorCode InvalidSubrangeType = new("Invalid Subrange Type");
    public static readonly PascalErrorCode InvalidTarget = new("Invalid Assignment Target");
    public static readonly PascalErrorCode InvalidType = new("Invalid Type");
    public static readonly PascalErrorCode InvalidVarParm = new("Invalid Var Parameter");
    public static readonly PascalErrorCode MinGtMax = new("Min Limit Greater Than Max Limit");
    public static readonly PascalErrorCode MissingBegin = new("Missing Begin");
    public static readonly PascalErrorCode MissingColon = new("Missing :");
    public static readonly PascalErrorCode MissingColonEquals = new("Missing :=");
    public static readonly PascalErrorCode MissingComma = new("Missing , ");
    public static readonly PascalErrorCode MissingConstant = new("Missing Constant");
    public static readonly PascalErrorCode MissingDo = new("Missing Do");
    public static readonly PascalErrorCode MissingDotDot = new("Missing ..");
    public static readonly PascalErrorCode MissingEnd = new("Missing End");
    public static readonly PascalErrorCode MissingEquals = new("Missing =");
    public static readonly PascalErrorCode MissingForControl = new("Invalid For Control Variable");
    public static readonly PascalErrorCode MissingIdentifier = new("Missing Identifier");
    public static readonly PascalErrorCode MissingLeftBracket = new("Missing [");
    public static readonly PascalErrorCode MissingOf = new("Missing Of");
    public static readonly PascalErrorCode MissingPeriod = new("Missing .");
    public static readonly PascalErrorCode MissingProgram = new("Missing Program");
    public static readonly PascalErrorCode MissingRightBracket = new("Missing ]");
    public static readonly PascalErrorCode MissingRightParen = new("Missing )");
    public static readonly PascalErrorCode MissingSemicolon = new("Missing ;");
    public static readonly PascalErrorCode MissingThen = new("Missing Then");
    public static readonly PascalErrorCode MissingToDownto = new("Missing To Or Downto");
    public static readonly PascalErrorCode MissingUntil = new("Missing Until");
    public static readonly PascalErrorCode MissingVariable = new("Missing Variable");
    public static readonly PascalErrorCode NotConstantIdentifier = new("Not A Constant Identifier");
    public static readonly PascalErrorCode NotRecordVariable = new("Not A Record Variable");
    public static readonly PascalErrorCode NotTypeIdentifier = new("Not A Type Identifier");
    public static readonly PascalErrorCode RangeInteger = new("Integer Literal Out Of Range");
    public static readonly PascalErrorCode RangeReal = new("Real Literal Out Of Range");
    public static readonly PascalErrorCode StackOverflow = new("Stack Overflow");
    public static readonly PascalErrorCode TooManyLevels = new("Nesting Level Too Deep");
    public static readonly PascalErrorCode TooManySubscripts = new("Too Many Subscripts");
    public static readonly PascalErrorCode UnexpectedEof = new("Unexpected End Of File");
    public static readonly PascalErrorCode UnexpectedToken = new("Unexpected Token");
    public static readonly PascalErrorCode Unimplemented = new("Unimplemented Feature");
    public static readonly PascalErrorCode Unrecognizable = new("Unrecognizable Input");
    public static readonly PascalErrorCode WrongNumberOfParms = new("Wrong Number Of Actual Parameters");

    public static readonly PascalErrorCode IOError = new("Object I/O error", -101);
    public static readonly PascalErrorCode TooManyErrors = new("Too many syntax errors", -102);

    /// <value>The error message.</value>
    public string Message { get; init; }

    /// <value>The exit status code.</value>
    public int Status { get; init; }

    /// <summary>
    /// Create a new <see cref="PascalErrorCode"/>.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="status">The optional error code.
    /// <para> If left out a positive status code will be assigned; otherwise the given status code must be negative.
    /// </para>
    /// </param>
    /// <exception cref="ArgumentException">The <paramref name="message"/> is empty
    /// <para>or</para>
    /// <para>The <paramref name="status"/> was not negative.</para>
    /// </exception>
    public PascalErrorCode(string message, int? status = null)
    {
        Message = string.IsNullOrWhiteSpace(message) ? throw new ArgumentException("Must have a value", nameof(message))
                                                     : message;
        Status = status is null ? _errorCodeCount++
                                : status.Value < 0 ? status.Value
                                                   : throw new ArgumentException("Must be negative", nameof(status));
    }

    public override string? ToString() => Message;
}