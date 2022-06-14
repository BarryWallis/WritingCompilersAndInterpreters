using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.Intermediate;

/// <summary>
/// The interface for a node of the intermediate code.
/// </summary>
public interface IIntermediateCodeNode
{
    /// <value>The node type.</value>
    public IntermediateCodeNodeType NodeType { get; }

    /// <summary>
    /// The parent of this node.
    /// </summary>
    public IIntermediateCodeNode? Parent { get; protected set; }

    /// <value>The list of all the nodes children."</value>
    public IReadOnlyCollection<IIntermediateCodeNode> Children { get; }

    /// <summary>
    /// Add a child node.
    /// </summary>
    /// <param name="child">The child node.</param>
    /// <returns>The child node.</returns>
    public IIntermediateCodeNode? AddChild(IIntermediateCodeNode? child);

    /// <summary>
    /// Set a node attribute's value.
    /// <para>If the attribute's key is already set, replace it with the new value.</para>
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    public void SetAttribute(IntermediateCodeKey key, object value);

    /// <summary>
    /// Get a node attribute's value.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <returns>
    /// If the attribute exists, return the attribute value; otherwise return <see langword="null"/>.
    /// </returns>
    public object? GetAttribute(IntermediateCodeKey key);

    /// <summary>
    /// Make a copy of this mnode.
    /// </summary>
    /// <returns>The copy of this node.</returns>
    public IIntermediateCodeNode Copy();
}