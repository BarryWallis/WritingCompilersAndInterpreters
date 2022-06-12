using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

/// <summary>
/// An implementtaion of a node of the intermediaste code.
/// </summary>
public record IntermediateCodeNode : IIntermediateCodeNode
{
    private readonly Dictionary<IntermediateCodeKey, object> _attributes = new();

    private readonly List<IIntermediateCodeNode> _children = new();

    public IntermediateCodeNodeType NodeType { get; }

    public IIntermediateCodeNode? Parent { get; private set; } = null;

    public IReadOnlyCollection<IIntermediateCodeNode> Children => _children;

    public IReadOnlyDictionary<IntermediateCodeKey, object> Attributes => _attributes;

    /// <summary>
    /// Create a new intermediate code node.
    /// </summary>
    /// <param name="nodeType">The node tpe.</param>
    public IntermediateCodeNode(IntermediateCodeNodeType nodeType) => NodeType = nodeType;

    /// <inheritdoc/>
    public IIntermediateCodeNode? AddChild(IIntermediateCodeNode? child)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        _children.Add(child);  // Null items are allowed to be added to a List.
#pragma warning restore CS8604 // Possible null reference argument.
        return child;
    }

    /// <inheritdoc/>
    public IIntermediateCodeNode Copy()
    {
        IntermediateCodeNode copy 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(NodeType) as IntermediateCodeNode 
              ?? throw new InvalidOperationException($"Could not create {nameof(IntermediateCodeNode)}");
        foreach (KeyValuePair<IntermediateCodeKey, object> keyValuePair in _attributes)
        {
            copy._attributes.Add(keyValuePair.Key, keyValuePair.Value);
        }

        return copy;
    }

    /// <inheritdoc/>
    public object? GetAttribute(IntermediateCodeKey key) 
        => _attributes.ContainsKey(key) ? _attributes[key] : null;

    /// <inheritdoc/>
    public void SetAttribute(IntermediateCodeKey key, object value)
    {
        if (_attributes.ContainsKey(key))
        {
            _attributes[key] = value;
        }
        else
        {
            _attributes.Add(key, value);
        }
    }

    /// <inheritdoc/>
    public override string? ToString() => NodeType.ToString();
}
