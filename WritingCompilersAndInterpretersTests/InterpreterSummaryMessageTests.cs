using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class InterpreterSummaryMessageTests
{
    [TestMethod]
    public void Ctor_ValidArguments_ConstructInterpreterSummaryMessage()
    {
        int executionCount = 1;
        int runtimeErrors = 2;
        float elapsedTime = 1f;

        InterpreterSummaryMessage actual = new(executionCount, runtimeErrors, elapsedTime);

        Assert.AreEqual(executionCount, actual.ExecutionCount);
        Assert.AreEqual(runtimeErrors, actual.RuntimeErrors);
        Assert.AreEqual(elapsedTime, actual.ElapsedTime);
    }

    [TestMethod]
    [DataRow(-1, 2, 1.0f)]
    [DataRow(1, -2, 1.0f)]
    [DataRow(1, 2, -1.0f)]
    [DataRow(-1, -2, -1.0f)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Ctor_InvalidArguments_ThrowsArgumentOutOfRangeException(int executionCount, int runtimeErrors,
                                                                        float elapsedTime)
        => _ = new InterpreterSummaryMessage(executionCount, runtimeErrors, elapsedTime);
}
