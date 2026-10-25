#if NET11_0_OR_GREATER
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net11Demo;

public class LibrariesDemo
{
    public async Task Run()
    {
        // Zero-copy stream adapters
        using (Stream stringStream = new StringStream("key: value", Encoding.UTF8))
        using (var reader = new StreamReader(stringStream))
        {
            Console.WriteLine($"StringStream: {await reader.ReadToEndAsync()}");
        }

        ReadOnlyMemory<byte> payload = "cached payload"u8.ToArray();
        using (Stream memoryStream = new ReadOnlyMemoryStream(payload))
        {
            Console.WriteLine($"ReadOnlyMemoryStream length: {memoryStream.Length}");
        }

        // StringBuilder.MoveChunks transfers buffers without copying
        var source = new StringBuilder("Hello, ");
        source.Append("World!");
        StringBuilder destination = StringBuilder.MoveChunks(source);
        Console.WriteLine($"MoveChunks: '{destination}', source length: {source.Length}");

        // Zstandard
        var text = string.Concat(Enumerable.Repeat("dotnet 11 ", 1000));
        using var compressed = new MemoryStream();
        using (var zstd = new ZstandardStream(compressed, CompressionMode.Compress, leaveOpen: true))
        {
            zstd.Write(Encoding.UTF8.GetBytes(text));
        }
        Console.WriteLine($"Zstandard: {text.Length} bytes -> {compressed.Length} bytes");

        // Password-protected ZIP
        var zipPath = Path.Combine(Path.GetTempPath(), "net11-secure.zip");
        File.Delete(zipPath);
        using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
        {
            var entry = archive.CreateEntry("secret.txt", "demo-password", ZipEncryptionMethod.Aes256);
            await using var entryStream = await entry.OpenAsync();
            await entryStream.WriteAsync("top secret"u8.ToArray());
        }

        using (var archive = ZipFile.OpenRead(zipPath))
        {
            var entry = archive.GetEntry("secret.txt")!;
            using var reader = new StreamReader(await entry.OpenAsync("demo-password"));
            Console.WriteLine($"Encrypted ZIP ({entry.EncryptionMethod}): '{await reader.ReadToEndAsync()}'");
        }

        // System.Text.Json: PascalCase policy, class-level ignore, per-member naming
        var options = new JsonSerializerOptions { DictionaryKeyPolicy = JsonNamingPolicy.PascalCase };
        var data = new EventData
        {
            EventName = "Launch",
            ExternalId = "ext-42",
            Tags = new() { ["releaseChannel"] = "rc2" }
        };
        Console.WriteLine(JsonSerializer.Serialize(data, options));
    }
}

[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
public class EventData
{
    public string EventName { get; set; } = "";

    [JsonNamingPolicy(JsonKnownNamingPolicy.SnakeCaseLower)]
    public string? ExternalId { get; set; }

    public string? Notes { get; set; } // null - skipped by the class-level [JsonIgnore]

    public Dictionary<string, string> Tags { get; set; } = [];
}
#endif