using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// Factory for creating objects that implement the symbo table.
/// </summary>
public static class SymbolTableFactory
{
    /// <summary>
    /// Create a symbol table stack.
    /// </summary>
    /// <returns>The new symbol table stack.</returns>
    public static ISymbolTableStack CreateSymbolTableStack() => new SymbolTableStack();

    /// <summary>
    /// Create a symbol table.
    /// </summary>
    /// <param name="nestingLevel">The nesting level.</param>
    /// <returns>The new symbol table.</returns>
    public static ISymbolTable CreateSymbolTable(int nestingLevel) => new SymbolTable(nestingLevel);

    /// <summary>
    /// Create a symbol table entry.
    /// </summary>
    /// <param name="name">The name of the symbol table entry.</param>
    /// <param name="symbolTable">The symbol table that contains the symbol table entry.</param>
    /// <returns>the new symbol table.</returns>
    /// <exception cref="ArgumentException">Must have a value.</exception>
    public static ISymbolTableEntry CreateSymbolTableEntry(string name, ISymbolTable symbolTable) 
        => string.IsNullOrWhiteSpace(name)
           ? throw new ArgumentException("Must have a value", nameof(name))
           : new SymbolTableEntry(name, symbolTable);
}
