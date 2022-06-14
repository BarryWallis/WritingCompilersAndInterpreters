using System.Diagnostics;
using System.Text;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.Utility;

/// <summary>
/// Print a parse tree
/// </summary>
public class ParseTreePrinter
{
    private const int _indentWidth = 4;
    private const int _lineWidth = 80;

    private readonly TextWriter _textWriter;
    private int _length = 0;
    private readonly string _indent = new(' ', _indentWidth);
    private string _indentation = string.Empty;
    private readonly StringBuilder _lineBuffer = new();

    /// <summary>
    /// Create a parse tree writer to write to the given text writer.
    /// </summary>
    /// <param name="textWriter">The text writer to write the parse tree output to.</param>
    public ParseTreePrinter(TextWriter textWriter) => _textWriter = textWriter;

    /// <summary>
    /// Print the intermediate code as a parse tree.
    /// </summary>
    /// <param name="intermediateCode">The intermediate code.</param>
    public void Print(IIntermediateCode intermediateCode)
    {
        _textWriter.WriteLine("\n===== INTERMEDIATE CODE =====\n");
        Debug.Assert(intermediateCode.Root is not null);
        PrintNode(intermediateCode.Root);
        PrintLine();
    }

    /// <summary>
    /// Print an output line.
    /// </summary>
    private void PrintLine()
    {
        if (_length > 0)
        {
            _textWriter.WriteLine(_lineBuffer);
            _ = _lineBuffer.Clear();
            _length = 0;
        }
    }

    /// <summary>
    /// Print a parse tree node.
    /// </summary>
    /// <param name="node">The parse tree node.</param>
    private void PrintNode(IIntermediateCodeNode node)
    {
        Append(_indentation);
        Append($"<{node}");
        Debug.Assert(node is IntermediateCodeNode);
        PrintAttributes((node as IntermediateCodeNode)!);
        PrintTypeSpecification(node);
        IReadOnlyCollection<IIntermediateCodeNode>? children = node.Children;
        if (children.Count > 0)
        {
            Append(">");
            PrintLine();
            PrintChildren(children);
            Append(_indentation);
            Append($"</{node}>");
        }
        else
        {
            Append(" ");
            Append("/>");
        }

        PrintLine();
    }

    /// <summary>
    /// Parse a tree node's child nodes.
    /// </summary>
    /// <param name="children">The list of child nodes.</param>
    private void PrintChildren(IReadOnlyCollection<IIntermediateCodeNode> children)
    {
        string saveIndentation = _indentation;
        _indentation += _indent;
        children.ToList().ForEach(n => PrintNode(n));
        _indentation = saveIndentation;
    }

    /// <summary>
    /// Print a parse tree node's type specification.
    /// </summary>
    /// <param name="node"></param>
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1822 // Mark members as static
    private void PrintTypeSpecification(IIntermediateCodeNode node)
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0060 // Remove unused parameter
    {

    }

    /// <summary>
    /// Print a parse tree node's attributes.
    /// </summary>
    /// <param name="node">The parse tree node.</param>
    private void PrintAttributes(IntermediateCodeNode node)
    {
        string saveIndentation = _indentation;
        _indentation += _indent;
        node.Attributes
            .ToList()
            .ForEach(kvp => PrintAttribute(kvp.Key.ToString(), kvp.Value));
        _indentation = saveIndentation;
    }

    /// <summary>
    /// Print a node attribute as key="value".
    /// </summary>
    /// <param name="keyString">The key string.</param>
    /// <param name="value">The value.</param>
    private void PrintAttribute(string keyString, object value)
    {
        string valueString = value is ISymbolTableEntry ? (value as ISymbolTableEntry)!.Name : value.ToString()!;
        Append(" ");
        Append($"{keyString.ToLowerInvariant()}=\"{valueString}\"");
        if (value is ISymbolTableEntry)
        {
            int level = (value as ISymbolTableEntry)!.SymbolTable.NestingLevel;
            PrintAttribute("LEVEL", level);
        }
    }

    /// <summary>
    /// Append text to the outpout line.
    /// </summary>
    /// <param name="text">The text.</param>
    private void Append(string text)
    {
        int textLength = text.Length;
        bool lineBreak = false;
        if (_length + textLength > _lineWidth)
        {
            PrintLine();
            _ = _lineBuffer.Append(_indentation);
            _length = _indentation.Length;
            lineBreak = true;
        }

        if (!(lineBreak && text == " "))
        {
            _ = _lineBuffer.Append(text);
            _length += textLength;
        }
    }
}
