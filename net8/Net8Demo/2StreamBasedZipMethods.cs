using System.Diagnostics;
using System.IO.Compression;
using System.Text;

public class StreamZipDemo
{
    public StreamZipDemo()
    {
        using var stream = new MemoryStream();
        ZipFile.CreateFromDirectory(Environment.CurrentDirectory, stream, CompressionLevel.Fastest, true, Encoding.Default);
        ZipFile.ExtractToDirectory(stream, Environment.CurrentDirectory, Encoding.Default, true);
    }
}