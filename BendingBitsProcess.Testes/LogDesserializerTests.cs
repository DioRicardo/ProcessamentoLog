using Xunit;
using BendingBitsProcess.Modelos;

namespace BendingBitsProcess.Testes;

public class LogDesserializerTests
{
    [Fact]
    public void GroupHashInfo_ValidLog_ReturnsCorrectDictionary()
    {
        // Arrange
        var logDesserializer = new LogDesserializer();
        string[] log = new string[]
        {
            "Iniciando interpretação da mensagem...",
            "item.RealmInicial: Realm1",
            "item.MensagemHash: ABC123",
            "System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.",
            "Trabalho com pedidos foi finalizado.",
            "Iniciando interpretação da mensagem...",
            "item.RealmInicial: Realm2",
            "item.MensagemHash: DEF456",
            "System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.",
            "Trabalho com pedidos foi finalizado."
        };

        // Act
        var result = logDesserializer.GroupHashInfo(log);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey("Realm1"));
        Assert.True(result.ContainsKey("Realm2"));
        Assert.Contains("ABC123", result["Realm1"]);
        Assert.Contains("DEF456", result["Realm2"]);
    }

    [Fact]
    public void GroupHashInfo_NoErrorInBlock_ReturnsEmptyDictionary()
    {
        // Arrange
        var logDesserializer = new LogDesserializer();
        string[] log = new string[]
        {
            "Iniciando interpretação da mensagem...",
            "item.RealmInicial: Realm1",
            "item.MensagemHash: ABC123",
            "Trabalho com pedidos foi finalizado.",
            "Iniciando interpretação da mensagem...",
            "item.RealmInicial: Realm2",
            "item.MensagemHash: DEF456",
            "Trabalho com pedidos foi finalizado."
        };

        // Act
        var result = logDesserializer.GroupHashInfo(log);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GroupHashInfo_MissingHashOrRealm_ReturnsEmptyDictionary()
    {
        // Arrange
        var logDesserializer = new LogDesserializer();
        string[] log = new string[]
        {
            "Iniciando interpretação da mensagem...",
            "item.RealmInicial: Realm1",
            "System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.",
            "Trabalho com pedidos foi finalizado.",
            "Iniciando interpretação da mensagem...",
            "item.MensagemHash: DEF456",
            "System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.",
            "Trabalho com pedidos foi finalizado."
        };

        // Act
        var result = logDesserializer.GroupHashInfo(log);

        // Assert
        Assert.Empty(result);
    }
}
