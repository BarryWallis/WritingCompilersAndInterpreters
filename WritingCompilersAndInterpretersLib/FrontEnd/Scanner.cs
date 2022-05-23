namespace WritingCompilersAndInterpretersLib.FrontEnd
{
    /// <summary>
    /// A language-independent framework class. This abstract scanner class will be implemented by language-specific subclasses.
    /// </summary>
    public abstract class Scanner
    {
        protected Source Source;

        /// <value>The current <see cref="Token"/>.</value>
        public Token? CurrentToken { get; protected set; } = null;

        /// <summary>
        /// Create a new <see cref="Scanner"/> to scan the given <see cref="Source"/>.
        /// </summary>
        /// <param name="source">The source to scan.</param>
        public Scanner(Source source) => Source = source;

        /// <summary>
        /// Return the next <see cref=" Token"/> from the <see cref="Source"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/>.</returns>
        public Token GetNextToken()
        {
            CurrentToken = ExtractToken();
            return CurrentToken;
        }

        /// <summary>
        /// Do the actual work of extracting and returning the next <see cref="Token"/> from the <see cref="Source"/>. 
        /// Implemented by <see cref="Scanner"/> subclasses.
        /// </summary>
        /// <returns>The next token.</returns>
        protected abstract Token ExtractToken();

        /// <summary>
        /// Calls the <see cref="Source.GetCurrentCharacter"/> method.
        /// </summary>
        /// <returns>The current character from the <see cref="Source"/>.</returns>
        public char GetCurrentCharacter() => Source.GetCurrentCharacter();

        /// <summary>
        /// Call <see cref="Source.GetNextCharacter()"/> method,
        /// </summary>
        /// <returns>the next character from the <see cref="Source"/>.</returns>
        public char GetNextCharacter() => Source.GetNextCharacter();
    }
}