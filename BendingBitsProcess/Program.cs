using BendingBitsProcess.Modelos;

while (true)
{
    Console.Clear();
    Console.WriteLine("BEM-VINDO AO PROCESSAMENTO DE LOG's:\n");
    Console.WriteLine("====================================\n");
    Console.WriteLine("Antes de iniciar o processamento, por favor, mova o arquivo \"processamento.log\" para a pasta \"ArquivoLog\" dentro da solução.");
    Console.WriteLine();
    Console.WriteLine("O Arquivo de logs está dentro da pasta correta?");
    Console.WriteLine("Digite (S) - Sim ou (N) - Não ou (0) - Sair.");


    char op = char.Parse(Console.ReadLine().ToLower());

    if(op == 's')
    {
        Console.WriteLine("\nOk. Iniciando processamento de dados...\n");

        FileManager file = new FileManager();
        LogDesserializer logDesserializer = new LogDesserializer();


        string[] log = file.ReadLogFile();

        if (log[0].Contains("Could not find file"))
        {
            Console.WriteLine(log[0]);
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para reiniciar...");
            Console.ReadKey();            
            continue;
        }

        var dataOrganized = logDesserializer.GroupHashInfo(log);

        file.CreateResultFile(dataOrganized);

        break;
    }
    else if(op == 'n')
    {
        Console.WriteLine("Por favor, salve o arquivo na pasta correta.");
        continue;
    }
    else
    {
        break;
    }
}