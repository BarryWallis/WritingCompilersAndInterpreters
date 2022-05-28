using WritingCompilersAndInterpretersLib.FrontEnd;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class TokenFactoryTests
{
    [TestMethod]
    public void Create_EmptySource_ReturnsEndOfFileToken()
    {
        string test = "";
        Source source = new(new StringReader(test));
        TokenFactory tokenFactory = new(source);

        Token actual = tokenFactory.Create();

        Assert.IsInstanceOfType(actual, typeof(EndOfFileToken));
    }
}
