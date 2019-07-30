using System;
using System.IO;
using System.Text;

namespace AzureDevOpsWikiToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 1 || !Directory.Exists(args[0]))
            {
                Console.WriteLine("第一引数にWikiディレクトリを第二引数に出力先フォルダを指定してください。");
                return;
            }

            var source = args[0];
            var dest = args[1];

            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);


            var wikiDirectory = WikiDirectory.Parse(source);
            foreach (var wikiEntry in wikiDirectory.WikiEntries)
            {
                using var streamWriter = 
                    new StreamWriter(
                        Path.Combine(dest, wikiEntry.MarkdownName), 
                        false, 
                        new UTF8Encoding(false));
                wikiEntry.Write(streamWriter);
                streamWriter.Flush();
            }
            using var catalogReader =
                new StreamReader(
                    Path.Combine(source, "catalog-md.yml"),
                    new UTF8Encoding(false));
            var catalog = Catalog.Load(catalogReader);

            catalog.Append(wikiDirectory);

            using var catalogWriter = 
                new StreamWriter(
                    Path.Combine(dest, "catalog.yml"),
                    false,
                    new UTF8Encoding(false));
            catalog.SaveToReViewCatalog(catalogWriter);

            Console.WriteLine("Completed.");
        }
    }
}
