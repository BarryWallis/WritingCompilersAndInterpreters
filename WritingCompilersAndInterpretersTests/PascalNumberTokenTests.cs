using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class PascalNumberTokenTests
{
    [TestMethod]
    [DataRow("0", 0)]
    [DataRow("1", 1)]
    [DataRow("20", 20)]
    [DataRow("0000000000000000032", 32)]
    [DataRow("31415926", 31415926)]
    public void Ctor_ValidInteger_CreatesIntegerNumberToken(string digits, int value)
    {
        StringReader stringReader = new(digits);
        Source source = new(stringReader);
        _ = source.GetCurrentCharacter();

        PascalNumberToken actual = new(source);

        Assert.AreEqual(digits, actual.Text);
        Assert.AreEqual(PascalTokenType.Integer, actual.TokenType);
        Assert.IsNotNull(actual.Value);
        Assert.AreEqual(value, (int)actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    [DataRow("3.1415926", 3.1415925f)]
    [DataRow("3.1415926535897932384626433", 3.1415927f)]
    [DataRow("0.00031415926E4", 3.1415926f)]
    [DataRow("0.00031415926e+00004", 3.1415925f)]
    [DataRow("31415.926e-4", 3.1415925f)]
    [DataRow("3141592600000000000000000000000e-30", 3.1415925f)]
    public void Ctor_ValidReal_CreatesRealNumberToken(string digits, float value)
    {
        StringReader stringReader = new(digits);
        Source source = new(stringReader);
        _ = source.GetCurrentCharacter();

        PascalNumberToken actual = new(source);

        Assert.AreEqual(digits, actual.Text);
        Assert.AreEqual(PascalTokenType.Real, actual.TokenType);
        Assert.IsNotNull(actual.Value);
        Assert.AreEqual(value, (float)actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    public void Ctor_ValidIntegerFollowedByTwoDots_CreatesRealNumberToken()
    {
        const string digits = "3..14259";
        const float value = 3.0f;
        StringReader stringReader = new(digits);
        Source source = new(stringReader);
        _ = source.GetCurrentCharacter();

        PascalNumberToken actual = new(source);

        Assert.AreEqual("3", actual.Text);
        Assert.AreEqual(PascalTokenType.Integer, actual.TokenType);
        Assert.IsNotNull(actual.Value);
        Assert.AreEqual(value, (int)actual.Value);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }
}
