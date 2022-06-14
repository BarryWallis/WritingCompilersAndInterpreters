using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// Factory to create intermediate code obects.
/// </summary>
public static class IntermediateCodeFactory
{
    /// <summary>
    /// Create an intermediate code object.
    /// </summary>
    /// <returns>The intermediate code object.</returns>
    public static IIntermediateCode CreateIntermediateCode() => new IntermediateCode();

    /// <summary>
    /// Create an intermediate code node.
    /// </summary>
    /// <param name="nodeType">The type of intermediate code node.</param>
    /// <returns>The intermediate code node.</returns>
    public static IIntermediateCodeNode CreateIntermediateCodeNode(IntermediateCodeNodeType nodeType)
        => new IntermediateCodeNode(nodeType);
}
