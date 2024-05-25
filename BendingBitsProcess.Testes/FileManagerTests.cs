using BendingBitsProcess.Modelos;
using Xunit;

namespace BendingBitsProcess.Testes;

public class FileManagerTests
{
    [Fact]
    public void ReadLogFileTest()
    {
        // Arrange
        var fileManager = new FileManager();

        // Act
        var result = fileManager.ReadLogFile();

        // Assert
        Assert.Single(result);
        Assert.Contains("Could not find file", result[0]);
    }

    [Fact]
    public void CreateResultFileTest()
    {
        // Arrange
        var fileManager = new FileManager();
        string filePath = fileManager.ReadFilePath();
        var testGroupData = new Dictionary<string, List<string>>
        {
            { "Realm1", new List<string> { "hash1", "hash2" } },
            { "Realm2", new List<string> { "hash3", "hash4" } }
        };

        // Act
        fileManager.CreateResultFile(testGroupData);

        // Assert
        string resultFilePath = Path.Combine(fileManager.directoryFilePath, "result.txt");
        Assert.True(File.Exists(resultFilePath));

        var lines = File.ReadAllLines(resultFilePath);
        Assert.Contains("Realm1", lines);
        Assert.Contains("\"hash1\",", lines);
        Assert.Contains("\"hash2\",", lines);
        Assert.Contains("Realm2", lines);
        Assert.Contains("\"hash3\",", lines);
        Assert.Contains("\"hash4\",", lines);

        // Cleanup
        File.Delete(resultFilePath);
    }
}