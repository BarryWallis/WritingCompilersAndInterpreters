namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

public record RuntimeErrorCode
{
    private readonly string _message;

    public static readonly RuntimeErrorCode UninitializedValue = new("Uninitialized value");
    public static readonly RuntimeErrorCode ValueRange = new("Value out of range");
    public static readonly RuntimeErrorCode InvalidCaseExpressionValue = new("Invalid CASE exprssion value");
    public static readonly RuntimeErrorCode DivisionByZero = new("Division by zero");
    public static readonly RuntimeErrorCode InvalidStandardFunctionArgument
        = new("Invalid standard function argument");
    public static readonly RuntimeErrorCode InvalidInput = new("Invalid input");
    public static readonly RuntimeErrorCode StackOverflow = new("Runtime stack overflow");
    public static readonly RuntimeErrorCode UnimplementedFeature = new("Unimplemented runtime feature");

    public RuntimeErrorCode(string message) => _message = message;

    public override string ToString() => _message;
}