using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class SourceLineMessageTests
{
    [TestMethod]
    public void Ctor_ValidArguments_ConstructSourceLineMessage()
    {
        int lineNumber = 1;
        string line = "line";

        SourceLineMessage actual = new(lineNumber, line);

        Assert.AreEqual(lineNumber, actual.LineNumber);
        Assert.AreEqual(line, actual.Line);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Ctor_InvalidArguments_ThrowsArgumentOutOfRangeException() => _ = new SourceLineMessage(-1, "text");
}
