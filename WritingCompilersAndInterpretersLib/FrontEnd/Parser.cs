﻿using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// A language-independent framework class. This abstract <see cref="Parser"/> class will be implemented by language
/// specific subclasses.
/// </summary>
public abstract class Parser : MessageHandler
{
    /// <value>The <see cref="Parser"/> symbol table.</value>
    public static ISymbolTable? SymbolTable { get; protected set; } = null;

    private static readonly ISymbolTableStack _symbolTableStack = SymbolTableFactory.CreateSymbolTableStack();
    /// <value>The symbol table stack.</value>
#pragma warning disable CA1822 // Mark members as static
    public ISymbolTableStack SymbolTableStack => _symbolTableStack;
#pragma warning restore CA1822 // Mark members as static

    /// <value>the message handler.</value>
    protected static MessageHandler MessageHandler { get; } = new();

    /// <value>The scanner to use with this <see cref="Parser"/>.</value>
    protected Scanner Scanner { get; init; }

    /// <value>The intrmediate code generated by this parser.</value>
    public IIntermediateCode? IntermediateCode { get; protected set; } = null;

    /// <value>The number of syntax errors returned by the <see cref="Parse"/>.</value>
    public abstract int ErrorCount { get; }

    /// <value>Return the scanner's current <see cref="Token"/>.</value>
    public Token? CurrentToken => Scanner.CurrentToken;

    /// <summary>
    /// Create a new <see cref="Parser"/> with the given <see cref="Scanner"/>.
    /// </summary>
    /// <param name="scanner">The <see cref="FrontEnd.Scanner"/> to use with this <see cref="Parser"/>.</param>
    protected Parser(Scanner scanner) => Scanner = scanner;

    /// <summary>
    /// Parse a source program and generate the intermediate code and the symbol table. 
    /// </summary>
    public abstract void Parse();

    /// <summary>
    /// Return the next <see cref="Token"/> from the scanner.
    /// </summary>
    /// <returns></returns>
    public Token GetNextToken() => Scanner.GetNextToken();
}
