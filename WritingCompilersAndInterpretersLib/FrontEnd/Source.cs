using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// The framework class that represents a source program.
/// </summary>
public class Source : MessageHandler
{
    private const int _noPosition = -2;
    private const int _startOfLine = -1;

    public const char EndOfLine = '\n';
    public const char EndOfFile = (char)0;

    private readonly TextReader _reader;
    private string? _line = null;

    /// <value>The current line number.</value>
    public int LineNumber { get; private set; } = 0;

    /// <value>The current position.</value>
    public int Position { get; private set; } = _noPosition;

    public char GetCurrentCharacter()
    {
        if (Position == _noPosition)
        {
            ReadLine();
            return GetNextCharacter();
        }
        else if (_line is null)
        {
            return EndOfFile;
        }
        else if (Position == _startOfLine || Position == _line.Length)
        {
            return EndOfLine;
        }
        else if (Position > _line.Length)
        {
            ReadLine();
            return GetNextCharacter();
        }
        else
        {
            return _line[Position];
        }
    }

    /// <summary>
    /// Create a new <see cref="Source"/>.
    /// </summary>
    /// <param name="reader">The <see cref="TextReader"/> for the source program.</param>
    public Source(TextReader reader) => _reader = reader;

    /// <summary>
    /// Consume the current source character and return the next character.
    /// </summary>
    /// <remarks>There must already be a character to consume.</remarks>
    /// <returns>The next character.</returns>
    public char GetNextCharacter()
    {
        Position += 1;
        return GetCurrentCharacter();
    }

    // Read the next source line.
    private void ReadLine()
    {
        _line = _reader.ReadLine();
        Position = _startOfLine;
        if (_line is not null)
        {
            LineNumber += 1;
        }

        if (_line is not null)
        {
            SendMessage(new SourceLineMessage(LineNumber, _line));
        }
    }

    /// <summary>
    /// Close the source file.
    /// </summary>
    public void Close() => _reader.Close();

    /// <summary>
    /// Peek at the next character without advancing the <see cref="CurrentCharacter"/>.
    /// </summary>
    /// <returns>The next character that will be read. </returns>
    /// <remarks>   
    /// <see cref="PeekNextCharacter"/> will not read past the end of a line. It will return return the 
    /// <see cref="EndOfLine"/> character at the end of the line.
    /// </remarks>
    public char PeekNextCharacter()
    {
        _ = GetCurrentCharacter();
        if (_line is null)
        {
            return EndOfFile;
        }

        int nextPosition = Position + 1;
        return nextPosition < _line.Length ? _line[nextPosition] : EndOfLine;
    }
}
