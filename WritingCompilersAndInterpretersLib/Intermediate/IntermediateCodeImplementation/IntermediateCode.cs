namespace WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

/// <summary>
/// An implementation of the intermediate code as a parse tree.
/// </summary>
public class IntermediateCode : IIntermediateCode
{
    /// <inheritdoc/>
    public IIntermediateCodeNode? Root { get; set; }
}
