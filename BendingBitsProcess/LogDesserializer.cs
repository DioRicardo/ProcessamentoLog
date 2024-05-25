using System.Text.RegularExpressions;

namespace BendingBitsProcess;

internal class LogDesserializer
{
    Regex HashRegex = new Regex(@"item\.MensagemHash:\s(?<hash>[A-Z0-9]+)");
    Regex realmRegex = new Regex(@"item.RealmInicial:\s(?<realm>\w+)");
    string textError = "System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.";
    string textBlockStart = "Iniciando interpretação da mensagem...";
    string textBlockEnd = "Trabalho com pedidos foi finalizado.";

    public Dictionary<string, List<string>> GroupHashInfo(string[] fileLog)
    {
        
        Dictionary<string, List<string>> hashByRealm = new Dictionary<string, List<string>>();

        // Verificador de início de blocos.
        bool isBlock = false;

        // Lista para armazenar as linhas de cada bloco.
        List<string> currentBlock = new List<string>();

        foreach (string line in fileLog)
        {
            if(line.Contains(textBlockStart))
            {
                isBlock = true;
                currentBlock.Clear();
            }

            if(isBlock)
            {
                currentBlock.Add(line);
            }


            // Verifica se chegou ao final de cada bloco, para agrupar as informações necessárias
            // de RealmInicial e MensagemHash dos blocos que contém o erro no dicionário hashByRealm.
            if (line.Contains(textBlockEnd))
            {
                isBlock = false;

                if(currentBlock.Any(line => line.Contains(textError)))
                {
                    string hash = null;
                    string realm = null;

                    foreach(string lineBlock in currentBlock)
                    {
                        Match hashMatch = HashRegex.Match(lineBlock);
                        if(hashMatch.Success)
                        {
                            hash = hashMatch.Groups["hash"].Value;
                        }

                        Match realmMatch = realmRegex.Match(lineBlock);

                        if(realmMatch.Success)
                        {
                            realm = realmMatch.Groups["realm"].Value;
                        }
                    }

                    if(!string.IsNullOrEmpty(realm) && !string.IsNullOrEmpty(hash))
                    {
                        if(!hashByRealm.ContainsKey(realm))
                        {
                            hashByRealm[realm] = new List<string>();
                        }
                        hashByRealm[realm].Add(hash);
                    }
                }
            }
        }

        return hashByRealm;

    }
}
