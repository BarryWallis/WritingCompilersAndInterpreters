using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class ParserSummaryMessageTests
{
    [TestMethod]
    public void Ctor_ValidArguments_ConstructParserSummaryMessage()
    {
        int numberOfLines = 1;
        int errorCount = 2;
        float elapsedTime = 1f;

        ParserSummaryMessage actual = new(numberOfLines, errorCount, elapsedTime);

        Assert.AreEqual(numberOfLines, actual.NumberOfLines);
        Assert.AreEqual(errorCount, actual.ErrorCount);
        Assert.AreEqual(elapsedTime, actual.ElapsedTIme);
    }

    [TestMethod]
    [DataRow(-1, 2, 1.0f)]
    [DataRow(1, -2, 1.0f)]
    [DataRow(1, 2, -1.0f)]
    [DataRow(-1, -2, -1.0f)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Ctor_InvalidArguments_ThrowsArgumentOutOfRangeException(int numberOfLines, int errorCount, float elapsedTIme)
        => _ = new InterpreterSummaryMessage(numberOfLines, errorCount, elapsedTIme);
}
