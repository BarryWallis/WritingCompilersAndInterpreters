namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens
{
    public record PascalSpecialSymbolToken : PascalToken
    {
        /// <summary>
        /// Create a Pascal special symbol token from the <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The sourxe.</param>
        public PascalSpecialSymbolToken(Source source)
        {
            if (!PascalTokenType.LookupSpecialSymbols.ContainsKey(source.GetCurrentCharacter().ToString()))
            {
                throw new InvalidOperationException($"Expected special symbol, found: '{source.GetCurrentCharacter()}'");
            }

            char currentCharacter = source.GetCurrentCharacter();
            LineNumber = source.LineNumber;
            Position = source.Position;
            Text = currentCharacter.ToString();
            TokenType = null;

            switch (currentCharacter)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case ',':
                case ';':
                case '=':
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '^':
                    _ = source.GetNextCharacter();
                    break;
                case ':':
                    currentCharacter = source.GetNextCharacter();
                    if (currentCharacter == '=')
                    {
                        Text += currentCharacter;
                        _ = source.GetNextCharacter();
                    }
                    break;
                case '<':
                    currentCharacter = source.GetNextCharacter();
                    if (currentCharacter == '=')
                    {
                        Text += currentCharacter;
                        _ = source.GetNextCharacter();
                    }
                    else if (currentCharacter == '>')
                    {
                        Text += currentCharacter;
                        _ = source.GetNextCharacter();
                    }
                    break;
                case '>':
                    currentCharacter = source.GetNextCharacter();
                    if (currentCharacter == '=')
                    {
                        Text += currentCharacter;
                        _ = source.GetNextCharacter();
                    }
                    break;
                case '.':
                    currentCharacter = source.GetNextCharacter();
                    if (currentCharacter == '.')
                    {
                        Text += currentCharacter;
                        _ = source.GetNextCharacter();
                    }
                    break;
                default:
                    throw new InvalidOperationException("Impossible case!");
                    //_ = source.GetNextCharacter();
                    //TokenType = PascalTokenType.Error;
                    //Value = PascalErrorCode.InvalidCharacter;
                    //break;
            }

            if (TokenType is null)
            {
                TokenType = PascalTokenType.LookupSpecialSymbols[Text];
            }
        }
    }
}