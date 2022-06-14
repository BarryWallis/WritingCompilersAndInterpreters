using WritingCompilersAndInterpretersLib.Message;

namespace PascalApp;

public class BackEndMessageListener : IObserver<Message>
{
    private bool _firstOutputMessage = true;

    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    public void OnNext(Message value)
    {
        switch (value)
        {
            case InterpreterSummaryMessage interpreterSummaryMessage:
                ProcessInterpreterSummaryMessage(interpreterSummaryMessage);
                break;
            case CompilerSummaryMessage compilerSummaryMessage:
                ProcessCompilerSummaryMessage(compilerSummaryMessage);
                break;
            case AssignmentMessage assignmentMessage:
                ProcessAssignmentMessage(assignmentMessage);
                break;
            case RuntimeErrorMessage runtimeErrorMessage:
                ProcessRuntimeErrorMessage(runtimeErrorMessage);
                break;
        }
    }

    private static void ProcessRuntimeErrorMessage(RuntimeErrorMessage runtimeErrorMessage)
    {
        Console.Write("*** RUNTIME ERROR");
        if (runtimeErrorMessage.LineNumber is not null)
        {
            Console.Write($" AT LINE {runtimeErrorMessage.LineNumber:D3}");
        }

        Console.WriteLine($": {runtimeErrorMessage.ErrorMessage}");
    }

    private void ProcessAssignmentMessage(AssignmentMessage assignmentMessage)
    {
        if (_firstOutputMessage)
        {
            Console.WriteLine();
            Console.WriteLine($"===== OUTPUT =====");
            _firstOutputMessage = false;
        }

        Console.WriteLine($" >>> LINE {assignmentMessage.LineNumber:D3}: " +
            $"{assignmentMessage.VariableName} = {assignmentMessage.Value}");
    }

    private static void ProcessCompilerSummaryMessage(CompilerSummaryMessage compilerSummaryMessage)
    {
        Console.WriteLine();
        Console.WriteLine($"{compilerSummaryMessage.InstructionCount,20} instructions executed.");
        Console.WriteLine($"{compilerSummaryMessage.ElapsedTime,20:F2} seconds total code generation time.");
    }

    private static void ProcessInterpreterSummaryMessage(InterpreterSummaryMessage interpreterSummaryMessage)
    {
        Console.WriteLine();
        Console.WriteLine($"{interpreterSummaryMessage.ExecutionCount,20} statements executed.");
        Console.WriteLine($"{interpreterSummaryMessage.RuntimeErrors,20} runtime errors.");
        Console.WriteLine($"{interpreterSummaryMessage.ElapsedTime,20:F2} seconds total execution timel");
    }
}
