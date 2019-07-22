using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureDevOpsWikiToPdf
{
    public class WikiDirectory : WikiEntry
    {
        private readonly DirectoryInfo _directoryInfo;
        private readonly List<WikiEntry> _wikiEntries = new List<WikiEntry>();

        private readonly WikiEntry _directoryWikiEntry;

        private WikiDirectory(DirectoryInfo directoryInfo, WikiEntry directoryWikiEntry)
        {
            _directoryInfo = directoryInfo;
            _directoryWikiEntry = directoryWikiEntry;
        }
        public override string FullName => _directoryInfo.FullName;
        public override string MarkdownName => $"{_directoryInfo.Name}.md";
        public override string ReViewName => $"{_directoryInfo.Name}.re";

        public override IReadOnlyList<IWikiEntry> WikiEntries => _wikiEntries;
        public override void Write(TextWriter textWriter)
        {
            Write(textWriter, 0);
        }

        internal override void Write(TextWriter textWriter, int indent)
        {
            _directoryWikiEntry.Write(textWriter, indent);
            textWriter.WriteLine();
            var childIndent = indent + 1;
            foreach (var wikiEntry in _wikiEntries)
            {
                wikiEntry.Write(textWriter, childIndent);
                textWriter.WriteLine();
            }
        }

        private void AddWikiEntry(WikiEntry wikiEntry) => _wikiEntries.Add(wikiEntry);

        public static WikiDirectory Parse(string path)
        {
            var wikiDirectory = new WikiDirectory(new DirectoryInfo(path), new NullWikiFile());
            Parse(wikiDirectory);
            return wikiDirectory;
        }

        private static void Parse(WikiDirectory wikiDirectory)
        {
            var dotOrderPath = Path.Combine(wikiDirectory.FullName, ".order");

            IEnumerable<string> children;
            if (File.Exists(dotOrderPath))
            {
                children = File.ReadAllLines(dotOrderPath);
            }
            else
            {
                var directories = wikiDirectory._directoryInfo
                    .GetDirectories()
                    .Select(x => x.Name);
                var files = wikiDirectory._directoryInfo
                    .GetFiles()
                    .Where(x => x.Extension == ".md")
                    .Select(x => x.Name.Substring(0, x.Name.Length - x.Extension.Length));

                children = directories.Union(files).OrderBy(x => x);
            }

            foreach (var child in children)
            {
                var childPath = Path.Combine(wikiDirectory.FullName, child);
                if (Directory.Exists(childPath))
                {
                    var directoryWikiEntry =
                        File.Exists($"{childPath}.md")
                            ? new WikiFile(new FileInfo($"{childPath}.md"))
                            : (WikiEntry)new NullWikiFile();

                    var childWikiDirectory = 
                        new WikiDirectory(new DirectoryInfo(childPath), directoryWikiEntry);
                    Parse(childWikiDirectory);
                    wikiDirectory.AddWikiEntry(childWikiDirectory);
                }
                else if (File.Exists($"{childPath}.md"))
                {
                    wikiDirectory.AddWikiEntry(new WikiFile(new FileInfo($"{childPath}.md")));
                }
                else
                {
                    throw new InvalidOperationException($"{childPath} is not exists.");
                }
            }
        }

        internal class NullWikiFile : WikiEntry
        {
            public override string FullName { get; } = string.Empty;
            public override string MarkdownName { get; } = string.Empty;
            public override string ReViewName { get; } = string.Empty;
            public override IReadOnlyList<IWikiEntry> WikiEntries { get; } = new List<IWikiEntry>();
            public override void Write(TextWriter textWriter)
            {
            }

            internal override void Write(TextWriter textWriter, int indent)
            {
            }
        }
    }
}