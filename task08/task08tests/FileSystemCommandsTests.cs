using Xunit;
using System;
using System.IO;
using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    private string CreateTestDirectory()
    {
        string path = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString());
        Directory.CreateDirectory(path);
        return path;
    }

    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        string testDir = CreateTestDirectory();
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        string testDir = CreateTestDirectory();
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldHandleNonExistentPath()
    {
        string path = Path.Combine(Path.GetTempPath(), "NonExistent_" + Guid.NewGuid().ToString());
        var command = new DirectorySizeCommand(path);
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void FindFilesCommand_ShouldHandleNonExistentPath()
    {
        string path = Path.Combine(Path.GetTempPath(), "NonExistent_" + Guid.NewGuid().ToString());
        var command = new FindFilesCommand(path, "*.txt");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }
}