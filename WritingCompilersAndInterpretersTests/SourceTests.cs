using WritingCompilersAndInterpretersLib.FrontEnd;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class SourceTests
{
    [TestMethod]
    public void GetCurrentCharacter_StartOfText_ReturnsFirstCharacter()
    {
        const string testString = "abcde";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();

        char actual = source.GetCurrentCharacter();

        Assert.AreEqual(testString[0], actual);
    }

    [TestMethod]
    public void GetCurrentCharacter_EmptyLine_ReturnsEndOfFile()
    {
        const string testString = "";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();

        char actual = source.GetCurrentCharacter();

        Assert.AreEqual(Source.EndOfFile, actual);
    }

    [TestMethod]
    public void GetCurrentCharacter_EndOfLine_ReturnsEndOfLineCharacter()
    {
        const string testString = "\n";
        Source source = new(new StringReader(testString));

        char actual = source.GetCurrentCharacter();

        Assert.AreEqual(Source.EndOfLine, actual);
    }

    [TestMethod]
    public void GetCurrentCharacter_EndOfFile_ReturnsEndOfFileCharacter()
    {
        const string testString = "";
        Source source = new(new StringReader(testString));

        char actual = source.GetCurrentCharacter();

        Assert.AreEqual(Source.EndOfFile, actual);
    }

    [TestMethod]
    public void GetNextCharacter_EmptyLine_ReturnsEndOfFile()
    {
        const string testString = "";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();

        char actual = source.GetNextCharacter();

        Assert.AreEqual(Source.EndOfFile, actual);
    }

    [TestMethod]
    public void GetNextCharacter_StartOfText_ReturnsSecondCharacter()
    {
        const string testString = "abcde";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();

        char actual = source.GetNextCharacter();

        Assert.AreEqual(testString[1], actual);
    }

    [TestMethod]
    public void GetNextCharacter_AfterStartOfText_ReturnsSecondCharacter()
    {
        const string testString = "abcde";
        StringReader stringReader = new(testString);
        Source source = new(stringReader);
        _ = source.GetCurrentCharacter();
        _ = source.GetNextCharacter();

        char actual = source.GetNextCharacter();

        Assert.AreEqual(testString[2], actual);
    }

    [TestMethod]
    public void GetNextCharacter_EndOfLine_ReturnsEndOfLineCharacter()
    {
        const string testString = "a\n";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();

        char actual = source.GetNextCharacter();

        Assert.AreEqual(Source.EndOfLine, actual);
    }

    [TestMethod]
    public void GetNextCharacter_EndOfFile_ReturnsEndOfFileCharacter()
    {
        const string testString = "";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();

        char actual = source.GetNextCharacter();

        Assert.AreEqual(Source.EndOfFile, actual);
    }

    [TestMethod]
    public void GetNextCharacter_ReadPastEndOfLine_ReturnsFirstCharacterPastEndOfLine()
    {
        const string testString = "a\nb";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();
        _ = source.GetNextCharacter();

        char actual = source.GetNextCharacter();

        Assert.AreEqual(testString[^1], actual);
    }

    [TestMethod]
    public void PeekNextCharacter_AtFirstCharacter_ReturnsSecondCharacter()
    {
        const string testString = "abc";
        Source source = new(new StringReader(testString));

        char actual = source.PeekNextCharacter();

        Assert.AreEqual(testString[1], actual);
    }

    [TestMethod]
    public void PeekNextCharacter_BeforeEndOfLine_ReturnsEndOfLine()
    {
        const string testString = "a\nb";
        Source source = new(new StringReader(testString));

        char actual = source.PeekNextCharacter();

        Assert.AreEqual(Source.EndOfLine, actual);
    }

    [TestMethod]
    public void PeekNextCharacter_AfterEndOfLine_ReturnsNextCharacterAfterEndOfLine()
    {
        const string testString = "a\nb";
        Source source = new(new StringReader(testString));
        _ = source.GetCurrentCharacter();
        _ = source.GetNextCharacter();

        char actual = source.PeekNextCharacter();

        Assert.AreEqual(Source.EndOfLine, actual);
    }

    [TestMethod]
    public void PeekNextCharacter_AtEndOfFile_ReturnsEndOfLine()
    {
        const string testString = "a";
        Source source = new(new StringReader(testString));

        char actual = source.PeekNextCharacter();

        Assert.AreEqual(Source.EndOfLine, actual);
    }
}
