using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class PascalStringTokenTests
{
    [TestMethod]
    public void Ctor_SimpleString_ValidToken()
    {
        string expected = "'Barry'";
        StringReader stringReader = new(expected);
        Source source = new(stringReader);

        PascalStringToken actual = new(source);

        Assert.AreEqual(expected, actual.Text);
        Assert.AreEqual(expected[1..^1], actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    public void Ctor_StringWithTwoSingleQuotes_TokenWithASingleQuote()
    {
        string expected = "''''";
        StringReader stringReader = new(expected);
        Source source = new(stringReader);

        PascalStringToken actual = new(source);

        Assert.AreEqual(expected, actual.Text);
        Assert.AreEqual("'", actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    public void Ctor_SimpleStringWithTwoSingleQuotesInIt_TokenWithASinglequote()
    {

        string expected = "'Barry''Wallis'";
        StringReader stringReader = new(expected);
        Source source = new(stringReader);

        PascalStringToken actual = new(source);

        Assert.AreEqual(expected, actual.Text);
        Assert.AreEqual("Barry'Wallis", actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    public void Ctor_SimpleStringStartingWithTabs_TokenWithSpacesInsteadOfTabs()
    {

        string expected = "'\tBarry\t\tWallis'";
        StringReader stringReader = new(expected);
        Source source = new(stringReader);

        PascalStringToken actual = new(source);

        Assert.AreEqual(expected.Replace('\t', ' '), actual.Text);
        Assert.AreEqual(" Barry  Wallis", actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    public void Ctor_NoEndingSingleQuote_TokenTypeErrorAndValueUnexpectedEof()
    {

        string expected = "'Barry";
        StringReader stringReader = new(expected);
        Source source = new(stringReader);

        PascalStringToken actual = new(source);

        Assert.AreEqual(PascalTokenType.Error, actual.TokenType);
        Assert.AreEqual(PascalErrorCode.UnexpectedEof, actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }
}
