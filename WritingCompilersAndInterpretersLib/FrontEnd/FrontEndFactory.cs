using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// A factory class that creates parsers for specific source languages.
/// </summary>
public class FrontEndFactory
{
    private const string _pascalLanguage = "Pascal";
    private const string _topDownType = "top-down";

    /// <summary>
    /// Create a <see cref="Parser"/>.
    /// </summary>
    /// <param name="language">The name of the source language.</param>
    /// <param name="type">The type of <see cref="Parser"/>.</param>
    /// <param name="source">The <see cref="Source"/> to use as input.</param>
    /// <returns>The appropriate <see cref="Parser"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Either the <paramref name="language"/> or the <paramref name="type"/> is invalid.
    /// </exception>
    public static Parser CreateParser(string language, string type, Source source)
    {
        if (language.Equals(_pascalLanguage, StringComparison.OrdinalIgnoreCase)
            && type.Equals(_topDownType, StringComparison.OrdinalIgnoreCase))
        {
            Scanner scanner = new PascalScanner(source);
            return new PascalParserTopDown(scanner);
        }
        else if (!language.Equals(_pascalLanguage, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Invalid language: '{language}'", nameof(language));
        }
        else
        {
            throw new ArgumentException($"Invalid type: '{type}'", nameof(type));
        }
    }
}
