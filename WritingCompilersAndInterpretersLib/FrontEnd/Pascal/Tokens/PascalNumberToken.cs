using System.Text;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

public record PascalNumberToken : PascalToken
{
    /// <summary>
    /// Create a Pascal number token from the <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The source.</param>
    public PascalNumberToken(Source source)
    {
        LineNumber = source.LineNumber;
        Position = source.Position;
        StringBuilder textBuffer = new();
        ExtractNumber(source, textBuffer);
        Text = textBuffer.ToString();
    }

    /// <summary>
    /// Extract a Pascal number from the <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The program source.</param>
    /// <param name="textBuffer">The buffer to append the token's characters to.</param>
    protected void ExtractNumber(Source source, StringBuilder textBuffer)
    {
        string? fractionDigits = null;
        string? exponentDigits = null;
        char exponentSign = '+';
        bool sawDotDot = false;
        char currentCharacter;

        TokenType = PascalTokenType.Integer;
        string wholeDigits = UnsignedIntegerDigits(source, textBuffer);
        if (TokenType == PascalTokenType.Error)
        {
            return;
        }

        currentCharacter = source.GetCurrentCharacter();
        if (currentCharacter == '.')
        {
            if (source.PeekNextCharacter() == '.')
            {
                sawDotDot = true;
            }
            else
            {
                TokenType = PascalTokenType.Real;
                _ = textBuffer.Append(currentCharacter);
                _ = source.GetNextCharacter();
                fractionDigits = UnsignedIntegerDigits(source, textBuffer);
                if (TokenType == PascalTokenType.Error)
                {
                    return;
                }
            }
        }

        currentCharacter = source.GetCurrentCharacter();
        if (!sawDotDot && (currentCharacter is 'E' or 'e'))
        {
            exponentDigits = ProcessExponent(source, textBuffer, ref exponentSign, ref currentCharacter);
        }

        ProcessIntegerOrReal(fractionDigits, exponentDigits, exponentSign, wholeDigits);

    }

    private void ProcessIntegerOrReal(string? fractionDigits, string? exponentDigits, char exponentSign, string wholeDigits)
    {
        if (TokenType == PascalTokenType.Integer)
        {
            int integerValue = ComputeIntegerValue(wholeDigits);
            if (TokenType != PascalTokenType.Error)
            {
                Value = integerValue;
            }
        }
        else if (TokenType == PascalTokenType.Real)
        {
            float floatValue = ComputeFloatValue(wholeDigits, fractionDigits, exponentDigits, exponentSign);
            if (TokenType != PascalTokenType.Error)
            {
                Value = floatValue;
            }
        }
    }

    private string? ProcessExponent(Source source, StringBuilder textBuffer, ref char exponentSign, ref char currentCharacter)
    {
        string? exponentDigits;
        TokenType = PascalTokenType.Real;
        _ = textBuffer.Append(currentCharacter);
        currentCharacter = source.GetNextCharacter();

        if (currentCharacter is '+' or '-')
        {
            _ = textBuffer.Append(currentCharacter);
            exponentSign = currentCharacter;
            _ = source.GetNextCharacter();
        }
        exponentDigits = UnsignedIntegerDigits(source, textBuffer);
        return exponentDigits;
    }

    private float ComputeFloatValue(string wholeDigits, string? fractionDigits, string? exponentDigits, char? exponentSign)
    {
        const int MaxExponent = 37;

        double floatValue = 0.0f;
        int exponentValue = ComputeIntegerValue(exponentDigits);
        string digits = wholeDigits;
        if (exponentSign == '-')
        {
            exponentValue = -exponentValue;
        }

        if (fractionDigits != null)
        {
            exponentValue -= fractionDigits.Length;
            digits += fractionDigits;
        }

        if (Math.Abs(exponentValue + wholeDigits.Length) > MaxExponent)
        {
            TokenType = PascalTokenType.Error;
            Value = PascalErrorCode.RangeReal;
            return 0.0f;
        }

        int index = 0;
        while (index < digits.Length)
        {
            floatValue = (10 * floatValue) + char.GetNumericValue(digits[index++]);
        }

        if (exponentValue != 0.0f)
        {
            floatValue *= Math.Pow(10, exponentValue);
        }

        return (float)floatValue;
    }

    /// <summary>
    /// Compute and return the integer value of a string of digits.
    /// </summary>
    /// <param name="digits">The string of digits.</param>
    /// <returns>The integer value.</returns>
    private int ComputeIntegerValue(string? digits)
    {
        if (digits is null)
        {
            return 0;
        }

        if (int.TryParse(digits, out int result))
        {
            return result;
        }
        else
        {
            TokenType = PascalTokenType.Error;
            Value = PascalErrorCode.RangeInteger;
            return 0;
        }
    }

    /// <summary>
    /// Extract and return the digits of an unsigned number.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="textBuffer">The string of digits.</param>
    /// <returns>The string of digits.</returns>
    private string UnsignedIntegerDigits(Source source, StringBuilder textBuffer)
    {
        char currentCharacter = source.GetCurrentCharacter();
        if (!char.IsDigit(currentCharacter))
        {
            TokenType = PascalTokenType.Error;
            Value = PascalErrorCode.InvalidNumber;
            return string.Empty;
        }

        StringBuilder digits = new();
        while (char.IsDigit(currentCharacter))
        {
            _ = textBuffer.Append(currentCharacter);
            _ = digits.Append(currentCharacter);
            currentCharacter = source.GetNextCharacter();
        }

        return digits.ToString();
    }
}