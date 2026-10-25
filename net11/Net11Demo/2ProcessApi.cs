#if NET11_0_OR_GREATER
using System.Diagnostics;

namespace Net11Demo;

public class ProcessApiDemo
{
    public async Task Run()
    {
        // Before: ProcessStartInfo + RedirectStandardOutput + OutputDataReceived + WaitForExit
        // .NET 11: one call, stdout and stderr captured without deadlocks
        ProcessTextOutput result = await Process.RunAndCaptureTextAsync("dotnet", ["--version"]);
        Console.WriteLine($"dotnet --version: {result.StandardOutput.Trim()}");
        Console.WriteLine($"Exit code: {result.ExitStatus.ExitCode}");

        // Stream output line by line, stdout vs stderr
        using var process = Process.Start(new ProcessStartInfo("dotnet", ["--list-sdks"])
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true
        })!;
        await foreach (ProcessOutputLine line in process.ReadAllLinesAsync())
        {
            Console.WriteLine(line.StandardError ? $"[ERR] {line.Content}" : $"[OUT] {line.Content}");
        }

        if (OperatingSystem.IsWindows())
        {
            await StartSuspendedDemo();
        }

        // Fire-and-forget: returns the PID, the handle is released immediately
        int pid = Process.StartAndForget("dotnet", ["--version"]);
        Console.WriteLine($"Started and forgot PID {pid}");

        // Lookup without exceptions
        Console.WriteLine(Process.TryGetProcessById(Environment.ProcessId, out var self)
            ? $"Found myself: {self.ProcessName}"
            : "Not found");
        Console.WriteLine(Process.TryGetProcessById(int.MaxValue, out _)
            ? "Found"
            : $"Process {int.MaxValue} not found - and no ArgumentException");
    }

    // Start frozen, attach diagnostics, then resume. The child is killed if this process dies.
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    static async Task StartSuspendedDemo()
    {
        var startInfo = new ProcessStartInfo("dotnet", ["--version"])
        {
            StartSuspended = true,
            KillOnParentExit = true
        };
        using var suspended = Process.Start(startInfo)!;
        Console.WriteLine($"Started suspended: PID {suspended.Id}");
        suspended.SafeHandle.Resume();
        ProcessExitStatus status = await suspended.WaitForExitStatusAsync();
        Console.WriteLine($"Resumed and exited with code {status.ExitCode}");
    }
}
#endif