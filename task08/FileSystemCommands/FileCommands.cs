using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _path;

    public DirectorySizeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Папка не найдена: {_path}");
            return;
        }

        long totalSize = 0;
        var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            totalSize += new FileInfo(file).Length;
        }

        Console.WriteLine($"Размер папки '{_path}': {totalSize} байт ({totalSize / 1024.0:F2} КБ)");
    }
}

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _mask;

    public FindFilesCommand(string path, string mask)
    {
        _path = path;
        _mask = mask;
    }

    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Папка не найдена: {_path}");
            return;
        }

        var files = Directory.GetFiles(_path, _mask, SearchOption.AllDirectories);
        Console.WriteLine($"Найдено файлов по маске '{_mask}': {files.Length}");

        foreach (var file in files)
        {
            Console.WriteLine($"  {file}");
        }
    }
}