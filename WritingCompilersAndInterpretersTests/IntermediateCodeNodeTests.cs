using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class IntermediateCodeNodeTests
{
    [TestMethod]
    public void AddChild_AddAChildNode_ChildNodeAddedToChildrenAndReturnChild()
    {
        IIntermediateCodeNode root 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);
        IIntermediateCodeNode child
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        IIntermediateCodeNode actual = root.AddChild(child);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        Assert.AreEqual(child, actual);
        Assert.AreEqual(1, root.Children.Count);
        Assert.AreEqual(child, root.Children.First());
    }

    [TestMethod]
    public void SetAttribute_AddUniqueAttribute_AttributeAdded()
    {
        const IntermediateCodeKey attribute = IntermediateCodeKey.Line;
        const int value = 1;
        IIntermediateCodeNode root
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);

        root.SetAttribute(attribute, value);

        Assert.IsNotNull(root.GetAttribute(attribute));
        Assert.AreEqual(value, (int)root.GetAttribute(attribute)!);
    }

    [TestMethod]
    public void SetAttribute_AddDuplicateAttribute_AttributeUpdated()
    {
        const IntermediateCodeKey attribute = IntermediateCodeKey.Line;
        const int value = 1;
        const int newValue = 2;
        IIntermediateCodeNode root
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);
        root.SetAttribute(attribute, value);

        root.SetAttribute(attribute, newValue);

        Assert.IsNotNull(root.GetAttribute(attribute));
        Assert.AreEqual(newValue, (int)root.GetAttribute(attribute)!);
    }

    [TestMethod]
    public void GetAttribute_AttributeExists_ReturnAttributeValue()
    {
        const IntermediateCodeKey attribute = IntermediateCodeKey.Line;
        const int value = 1;
        IIntermediateCodeNode root
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);
        root.SetAttribute(attribute, value);

        Assert.IsNotNull(root.GetAttribute(attribute));
        int actual = (int)root.GetAttribute(attribute)!;

        Assert.AreEqual(value, actual);
    }

    [TestMethod]
    public void GetAttribute_AttributeDoesNotExist_ReturnsNull()
    {
        const IntermediateCodeKey attribute = IntermediateCodeKey.Line;
        IIntermediateCodeNode root
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);

        object? actual = root.GetAttribute(attribute);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToString_AnyValidIntermediateCodeNode_ReturnsStringOfNodeType()
    {
        IIntermediateCodeNode root
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Subtract);

        string? actual = root.ToString();

        Assert.IsNotNull(actual);
        Assert.AreEqual("Subtract", actual);
    }
}
