using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.BackEnd.Compiler;
using WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

namespace WritingCompilersAndInterpretersLib.BackEnd;

/// <summary>
/// A factory class that creates compiler and interpreter components.
/// </summary>
public static class BackEndFactory
{
    /// <summary>
    /// Create the appropriate back end component.
    /// </summary>
    /// <param name="operation">The operation of the component.</param>
    /// <returns>The appropriate back end.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid operation given.</exception>
    public static BackEnd CreateBackEnd(string operation) 
        => operation.Equals("compile", StringComparison.OrdinalIgnoreCase)
           ? new CodeGenerator()
           : operation.Equals("execute", StringComparison.OrdinalIgnoreCase)
             ? (BackEnd)new Executor()
             : throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid operation '{operation}'");
}
