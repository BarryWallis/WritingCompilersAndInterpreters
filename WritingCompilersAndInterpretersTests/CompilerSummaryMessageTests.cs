using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class CompilerSummaryMessageTestsS
{
    [TestMethod]
    public void Ctor_ValidArguments_ConstructCompilerSummaryMessage()
    {
        int instructionCount = 1;
        float elapsedTime = 1f;

        CompilerSummaryMessage actual = new(instructionCount, elapsedTime);

        Assert.AreEqual(instructionCount, actual.InstructionCount);
        Assert.AreEqual(elapsedTime, actual.ElapsedTime);
    }

    [TestMethod]
    [DataRow(-1, 1.0f)]
    [DataRow(1, -1.0f)]
    [DataRow(-1, -1.0f)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Ctor_InvalidArguments_ThrowsArgumentOutOfRangeException(int instructionCount, float elapsedTime)
        => _ = new CompilerSummaryMessage(instructionCount, elapsedTime);
}
