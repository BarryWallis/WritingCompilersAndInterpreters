using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class PascalWordTokenTests
{
    [TestMethod]
    [DataRow("b")]
    [DataRow("begin")]
    [DataRow("begin\n")]
    [DataRow("begin\t")]
    [DataRow("begin   ")]
    public void Ctor_ValidWord_ValidToken(string expected)
    {
        TextReader textReader = new StringReader(expected);
        Source source = new(textReader);

        Token actual = new PascalWordToken(source);

        Assert.AreEqual(expected.Trim(), actual.Text);
        Assert.AreEqual(1, actual.LineNumber);
        Assert.AreEqual(0, actual.Position);
        Assert.IsInstanceOfType(actual, typeof(PascalWordToken));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Ctor_InvalidWord_ThrowsInvalidOperationException()
    {
        TextReader textReader = new StringReader("1");
        Source source = new(textReader);

        _ = new PascalWordToken(source);
    }
}
