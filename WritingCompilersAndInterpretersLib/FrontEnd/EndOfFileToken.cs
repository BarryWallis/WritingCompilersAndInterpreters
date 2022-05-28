﻿namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// The generic end of file token.
/// </summary>
public record EndOfFileToken : Token
{
    /// <summary>
    /// Create a new <see cref="EndOfFileToken"/>.
    /// </summary>
    public EndOfFileToken() : base()
    {
    }
}
