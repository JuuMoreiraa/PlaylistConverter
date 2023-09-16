using System;
using System.IO;
using System.Text;
using System.Xml;

class PlaylistConverter
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Uso: PlaylistConverter.exe 'C:\\arquivoentrada.playlist' 'C:\\arquivosaida.txt'");
            return;
        }

        string playlistEntrada = args[0];
        string txtSaida = args[1];

        try
        {
            string textContent = ConverteParaTxt(playlistEntrada);

            File.WriteAllText(txtSaida, textContent, Encoding.UTF8);

            Console.WriteLine($"Conversão realizada com sucesso. Arquivo de texto gerado em: {txtSaida}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        }
    }

    static string ConverteParaTxt(string playlistEntrada)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(playlistEntrada);

        StringBuilder result = new StringBuilder();

        ExtracaoDeMetodos(doc.DocumentElement, result, "");

        return result.ToString();
    }

    static void ExtracaoDeMetodos(XmlNode node, StringBuilder result, string currentNamespace)
    {
        foreach (XmlNode child in node.ChildNodes)
        {
            if (child.Name == "Property" && child.Attributes["Name"]?.Value == "Namespace")
            {
                currentNamespace = child.Attributes["Value"].Value;
            }
            else if (child.Name == "Property" && child.Attributes["Name"]?.Value == "DisplayName")
            {
                string displayName = child.Attributes["Value"].Value;
                result.AppendLine($"{currentNamespace}.{displayName}");
            }
            else
            {
                ExtracaoDeMetodos(child, result, currentNamespace);
            }
        }
    }
}
