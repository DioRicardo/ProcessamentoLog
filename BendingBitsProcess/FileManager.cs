using System.Threading.Channels;

namespace BendingBitsProcess;

internal class FileManager
{
    
    string logFileName = "processamento.log";
    string resultFileName = "result.txt";
    string directoryFilePath = null;


    private string ReadFilePath()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        DirectoryInfo dirInfo = new DirectoryInfo(baseDirectory);
        string basicPath = dirInfo.Parent?.Parent?.Parent?.FullName;
        string directoryFilesName = "ArquivoLog";
        directoryFilePath = Path.Combine(basicPath, directoryFilesName);

        string logFullPath = Path.Combine(directoryFilePath, logFileName);

        return logFullPath;
    }

    public string[] ReadLogFile()
    {
        string logFullPath = ReadFilePath();
        

        try
        {
            return File.ReadAllLines(logFullPath);
        }
        catch (FileNotFoundException ex)
        {
            string[] errorMessage = { $"{ex.Message}" };
            return errorMessage;
        }
        
    }

    public void CreateResultFile(Dictionary<string, List<string>> groupData)
    {
        string resultFilePath = Path.Combine(directoryFilePath, resultFileName);

        using (StreamWriter streamWriter = new StreamWriter(resultFilePath))
        {
            foreach(var realm in groupData)
            {
                streamWriter.WriteLine($"{realm.Key}");
                foreach(var hash in realm.Value)
                {
                    streamWriter.WriteLine($"\"{hash}\",");
                }
                streamWriter.WriteLine("\n=====================================================\n");
            }
        }

        Console.WriteLine("Processamento finalizado. Hashes organizadas.");
        Console.WriteLine($"Resultado salvo no arquivo \"{resultFileName}\"");
        Console.WriteLine($"Caminho do arquivo: {resultFilePath}");
    }
}
