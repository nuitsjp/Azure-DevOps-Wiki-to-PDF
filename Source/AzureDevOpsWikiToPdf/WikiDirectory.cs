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

        public WikiDirectory(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        public override string FullName => _directoryInfo.FullName;
        public override string MarkdownName => $"{_directoryInfo.Name}.md";
        public override string ReViewName => $"{_directoryInfo.Name}.re";

        public override IReadOnlyList<IWikiEntry> WikiEntries => _wikiEntries;
        public override void Write(TextWriter textWriter)
        {
            Write(textWriter, string.Empty);
        }

        internal override void Write(TextWriter textWriter, string indent)
        {
            bool isFirstPage = true;
            var childIndent = indent;
            foreach (var wikiEntry in _wikiEntries)
            {
                wikiEntry.Write(textWriter, childIndent);
                textWriter.WriteLine();
                if (isFirstPage)
                {
                    childIndent = childIndent + "#";
                    isFirstPage = false;
                }
            }
        }

        private void AddWikiEntry(WikiEntry wikiEntry) => _wikiEntries.Add(wikiEntry);

        public static WikiDirectory Parse(string path)
        {
            var wikiDirectory = new WikiDirectory(new DirectoryInfo(path));
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
                    .Select(x => x.Name.Substring(0, x.Name.Length - x.Extension.Length));

                children = directories.Union(files).OrderBy(x => x);
            }

            foreach (var child in children)
            {
                var childPath = Path.Combine(wikiDirectory.FullName, child);
                if (File.Exists($"{childPath}.md"))
                {
                    wikiDirectory.AddWikiEntry(new WikiFile(new FileInfo($"{childPath}.md")));
                }
                else if (Directory.Exists(childPath))
                {
                    var childWikiDirectory = new WikiDirectory(new DirectoryInfo(childPath));
                    Parse(childWikiDirectory);
                    wikiDirectory.AddWikiEntry(childWikiDirectory);
                }
                else
                {
                    throw new InvalidOperationException($"{childPath} is not exists.");
                }
            }
        }
    }
}