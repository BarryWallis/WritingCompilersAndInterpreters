using WritingCompilersAndInterpretersLib.BackEnd;
using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Utility;

namespace PascalApp;

/// <summary>
/// Compile or interpret a Pascal source program.
/// </summary>
public class Pascal
{

    public Pascal(string operation, string filePath, string flags)
    {
        try
        {
            bool intermediate = flags.IndexOf('i') > -1;
            bool crossReference = flags.IndexOf('x') > -1;

            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new(fileStream);
            Source source = new(streamReader);
            _ = source.Subscribe(new SourceMessageListener());
            Parser parser = FrontEndFactory.CreateParser("pascal", "top-down", source);
            _ = parser.Subscribe(new ParserMessageListener());

            BackEnd backEnd = BackEndFactory.CreateBackEnd(operation);
            _ = backEnd.Subscribe(new BackEndMessageListener());

            parser.Parse();
            IIntermediateCode? intermediateCode = parser.IntermediateCode; // TODO Remove question mark
            ISymbolTable? symbolTable = Parser.SymbolTable; // TODO Remove question mark

            ISymbolTableStack symbolTableStack = parser.SymbolTableStack;
            if (crossReference)
            {
                CrossReferencer crossReferencer = new();
                crossReferencer.Print(symbolTableStack);
            }

            backEnd.Process(intermediateCode!, symbolTableStack!); // TODO: Remove bangs
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine("***** Internal translator error *****");
            Console.Error.WriteLine(exception);
        }
    }
}
