using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using WritingCompilersAndInterpretersLib.FrontEnd;

using static System.Net.Mime.MediaTypeNames;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class TokenTests
{
    [TestMethod]
    public void Ctor_ValidArguments_ConstructToken()
    {
        int lineNumber = 1;
        int position = 2;
        string text = "text";
        string value = "value";

        Token actual = new(lineNumber, position, text, value);

        Assert.AreEqual(lineNumber, actual.LineNumber);
        Assert.AreEqual(position, actual.Position);
        Assert.AreEqual(text, actual.Text);
        Assert.AreEqual(value, actual.Value);
    }

    [TestMethod]
    [DataRow(-1, 2)]
    [DataRow(1, -2)]
    [DataRow(-1, -2)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Ctor_InvalidArguments_ThrowsArgumentOutOfRange(int lineNumber, int position)
    {
        string text = "text";
        string value = "value";

        _ = new Token(lineNumber, position, text, value);
    }

}
