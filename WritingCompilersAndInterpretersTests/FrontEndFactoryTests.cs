using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class FrontEndFactoryTests
{
    [TestMethod]
    public void Parser_PascalParserTopDownArguments_ReturnsPascalParserTopDown()
    {
        Source source = new(new StringReader(""));

        Parser actual = FrontEndFactory.CreateParser("pAsCaL", "tOp-DoWn", source);

        Assert.IsInstanceOfType(actual, typeof(PascalParserTopDown));
    }

    [TestMethod]
    [DataRow("pascal", "x")]
    [DataRow("x", "top-down")]
    [DataRow("x", "x")]
    [ExpectedException(typeof(ArgumentException))]
    public void Parser_InvalidArgument_ThrowsArgumentException(string language, string type)
    {
        Source source = new(new StringReader(""));

        _ = FrontEndFactory.CreateParser(language, type, source);
    }

}
