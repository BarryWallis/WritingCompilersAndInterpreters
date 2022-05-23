using WritingCompilersAndInterpretersLib.Message;

namespace PascalApp;

public class BackEndMessageListener : IObserver<Message>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(Message value)
    {
        switch (value)
        {
            case InterpreterSummaryMessage interpreterSummaryMessage:
                Console.WriteLine();
                Console.WriteLine($"{interpreterSummaryMessage.ExecutionCount,20} statements executed.");
                Console.WriteLine($"{interpreterSummaryMessage.RuntimeErrors,20} runtime errors.");
                Console.WriteLine($"{interpreterSummaryMessage.ElapsedTime,20:F2} seconds total execution timel");
                break;
            case CompilerSummaryMessage compilerSummaryMessage:
                Console.WriteLine();
                Console.WriteLine($"{compilerSummaryMessage.InstructionCount,20} instructions executed.");
                Console.WriteLine($"{compilerSummaryMessage.ElapsedTime,20:F2} seconds total code generation time.");
                break;
        }
    }
}
