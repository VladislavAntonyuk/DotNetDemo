using System.Diagnostics;

namespace Net11Demo;

// Runtime Async: with <Features>runtime-async=on</Features> the compiler no longer generates
// an IAsyncStateMachine per async method.
// Run with the flag removed from the csproj: every level shows up twice plus AsyncMethodBuilderCore.Start frames.
public class RuntimeAsyncDemo
{
    public async Task Run()
    {
        await OuterAsync();
    }

    static async Task OuterAsync()
    {
        await Task.CompletedTask;
        await MiddleAsync();
    }

    static async Task MiddleAsync()
    {
        await Task.CompletedTask;
        await InnerAsync();
    }

    static async Task InnerAsync()
    {
        await Task.CompletedTask;
        Console.WriteLine(new StackTrace(fNeedFileInfo: true));
    }
}
