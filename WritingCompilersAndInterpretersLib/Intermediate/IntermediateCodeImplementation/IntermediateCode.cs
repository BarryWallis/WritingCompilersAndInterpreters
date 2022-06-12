using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

/// <summary>
/// An implementation of the intermediate code as a parse tree.
/// </summary>
public class IntermediateCode : IIntermediateCode
{
    /// <inheritdoc/>
    public IIntermediateCodeNode? Root { get; set; }
}
