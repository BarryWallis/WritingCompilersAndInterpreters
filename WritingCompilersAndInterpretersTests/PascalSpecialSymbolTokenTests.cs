using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class PascalSpecialSymbolTokenTests
{
    [TestMethod]
    [DataRow('+')]
    [DataRow('-')]
    [DataRow('*')]
    [DataRow('/')]
    [DataRow(',')]
    [DataRow(';')]
    [DataRow('=')]
    [DataRow('(')]
    [DataRow(')')]
    [DataRow('[')]
    [DataRow(']')]
    [DataRow('{')]
    [DataRow('}')]
    [DataRow('^')]
    public void Ctor_SingleCharacterToken_CreateToken(char expected)
    {
        StringReader stringReader = new(expected.ToString());
        Source source = new(stringReader);

        PascalSpecialSymbolToken actual = new(source);

        Assert.AreEqual(expected.ToString(), actual.Text);
        Assert.AreEqual(PascalTokenType.LookupSpecialSymbols[expected.ToString()], actual.TokenType);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }

    [TestMethod]
    [DataRow(":=")]
    [DataRow("<=")]
    [DataRow("<>")]
    [DataRow(">=")]
    [DataRow("..")]
    public void Ctor_DualCharacterToken_CreateToken(string expected)
    {
        StringReader stringReader = new(expected);
        Source source = new(stringReader);

        PascalSpecialSymbolToken actual = new(source);

        Assert.AreEqual(expected, actual.Text);
        Assert.AreEqual(PascalTokenType.LookupSpecialSymbols[expected], actual.TokenType);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
    }
}
