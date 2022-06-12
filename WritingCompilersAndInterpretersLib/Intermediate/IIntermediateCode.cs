namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// The framework interface for intermediate code.
/// </summary>
public interface IIntermediateCode
{
    /// <value>The root node.</value>
    public IIntermediateCodeNode? Root { get; set; }
}
