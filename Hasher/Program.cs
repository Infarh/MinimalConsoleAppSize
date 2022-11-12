using System.Globalization;
using System.Security.Cryptography;
using System.Text;

var files = args.Length == 0
    ? Directory.EnumerateFiles(Directory.GetCurrentDirectory())
    : args;

var current_file = Path.GetFileName(Environment.ProcessPath);

var i = 1;
foreach (var file_path in files)
    if (File.Exists(file_path) && !string.Equals(Path.GetFileName(file_path), current_file, StringComparison.OrdinalIgnoreCase))
    {
        if (i > 1) Console.WriteLine();

        Console.WriteLine("File    {0}: {1}", i++, Path.GetFileName(file_path));
        Console.WriteLine("         : {0}", file_path);

        try
        {
            var file = new FileInfo(file_path);
            var sha256 = ComputeSha256(file_path);
            var sha512 = ComputeSha512(file_path);
            var md5 = ComputeMD5(file_path);

            Console.WriteLine("  length : {0}", GetLength(file.Length).ToString(CultureInfo.InvariantCulture));
            Console.WriteLine("  created: {0:HH:mm:ss dd.MM.yyyy}", file.CreationTime);
            Console.WriteLine(" modified: {0:HH:mm:ss dd.MM.yyyy}", file.LastWriteTime);
            Console.WriteLine("   access: {0:HH:mm:ss dd.MM.yyyy}", file.LastAccessTime);
            Console.WriteLine("   SHA256: {0}", sha256);
            Console.WriteLine("   SHA512: {0}", sha512);
            Console.WriteLine("      MD5: {0}", md5);
        }
        catch (Exception error)
        {
            Console.WriteLine("   error: {0}", error.Message);
        }
    }

static FormattableString GetLength(long Length) => Length switch
{
    > 0x10000000000L => $"{(double)Length / 0x10000000000L:0.##}TB ({Length}b)",
    > 0x40000000L    => $"{(double)Length / 0x40000000L:0.##}GB ({Length}b)",
    > 0x100000L      => $"{(double)Length / 0x100000:0.##}MB ({Length}b)",
    > 0x400L         => $"{(double)Length / 0x400:0.##}kB ({Length}b)",
    _                => $"{Length}b"
};

static string ComputeSha256(string FilePath)
{
    using var file = File.OpenRead(FilePath);
    var hash_bytes = SHA256.HashData(file);

    var builder = new StringBuilder(64);

    foreach (var b in hash_bytes)
        builder.Append(b.ToString("x2"));

    return builder.ToString();
}
static string ComputeSha512(string FilePath)
{
    using var file = File.OpenRead(FilePath);
    var hash_bytes = SHA512.HashData(file);

    var builder = new StringBuilder(128);

    foreach (var b in hash_bytes)
        builder.Append(b.ToString("x2"));

    return builder.ToString();
}

static string ComputeMD5(string FilePath)
{
    using var file = File.OpenRead(FilePath);
    var hash_bytes = MD5.HashData(file);

    var builder = new StringBuilder(32);

    foreach (var b in hash_bytes)
        builder.Append(b.ToString("x2"));

    return builder.ToString();
}
