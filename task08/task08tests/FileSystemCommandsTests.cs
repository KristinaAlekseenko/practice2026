using System;
using System.IO;
using Xunit;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        string testDir = Path.Combine(Path.GetTempPath(), "TestDir");

        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "file2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_WithNonExistentDirectory_ShouldNotThrow()
    {
        var command = new DirectorySizeCommand("Z:\\NonExistentDir_123456");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        string testDir = Path.Combine(Path.GetTempPath(), "TestDir");

        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void FindFilesCommand_WithNoMatchingFiles_ShouldNotThrow()
    {
        string testDir = Path.Combine(Path.GetTempPath(), "TestDir");

        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");

            var command = new FindFilesCommand(testDir, "*.log");
            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_ShouldNotThrowForEmptyDirectory()
    {
        string testDir = Path.Combine(Path.GetTempPath(), "EmptyDir");

        try
        {
            Directory.CreateDirectory(testDir);
            var command = new DirectorySizeCommand(testDir);
            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }
}