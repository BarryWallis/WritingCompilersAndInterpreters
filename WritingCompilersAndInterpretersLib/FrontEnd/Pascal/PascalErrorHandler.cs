using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal
{
    /// <summary>
    /// Error handler for Pascal syntax errors.
    /// </summary>
    public class PascalErrorHandler
    {
        private const int _maxErrors = 25;

        public static int ErrorCount { get; private set; }

        /// <summary>
        /// Abort the translation.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="parser">The parser.</param>
#pragma warning disable CA1822 // Mark members as static
        public void AbortTranslation(PascalErrorCode errorCode, Parser parser)
#pragma warning restore CA1822 // Mark members as static
        {
            string fatalText = $"FATAL ERROR: {errorCode}";
            parser.SendMessage(new SyntaxErrorMessage(0, 0, "", fatalText));
            Environment.Exit(errorCode.Status);
        }

        /// <summary>
        /// Flag an error in a source line.
        /// </summary>
        /// <param name="errorToken">The bad token.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="pascalParserTopDown">The parser.</param>
        public void Flag(Token errorToken, PascalErrorCode errorCode, Parser parser)
        {
            Debug.Assert(errorCode.ToString() is not null);
            parser.SendMessage(new SyntaxErrorMessage(errorToken.LineNumber, errorToken.Position, errorToken.Text,
                                                      errorCode.ToString()!));
            if (++ErrorCount > _maxErrors)
            {
                AbortTranslation(PascalErrorCode.TooManyErrors, parser);
            }
        }
    }
}