using WritingCompilersAndInterpretersLib.FrontEnd;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

/// <summary>
/// Pascal scanner.
/// </summary>
public class PascalScanner : Scanner
{
    /// <summary>
    /// Create a new <see cref="PascalScanner"/>.
    /// </summary>
    /// <param name="source">The <see cref="Source"/> to get tokens from.</param>
    public PascalScanner(Source source) : base(source)
    {

    }

    /// <inheritdoc/>
    protected override Token ExtractToken() => new TokenFactory(Source).Create();
}
