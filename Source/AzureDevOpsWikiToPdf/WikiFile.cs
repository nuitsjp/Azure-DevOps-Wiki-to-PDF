using System.Collections.Generic;
using System.IO;

namespace AzureDevOpsWikiToPdf
{
    public class WikiFile : WikiEntry
    {
        private readonly FileInfo _fileInfo;

        public WikiFile(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public override string FullName => _fileInfo.FullName;
        public override string MarkdownName => _fileInfo.Name.Replace(_fileInfo.Extension, ".md");
        public override string ReViewName => _fileInfo.Name.Replace(_fileInfo.Extension, ".re");
        public override IReadOnlyList<IWikiEntry> WikiEntries { get; } = new List<IWikiEntry>();
        public override void Write(TextWriter textWriter)
        {
            Write(textWriter, string.Empty);
        }

        internal override void Write(TextWriter textWriter, string indent)
        {
            foreach (var line in File.ReadAllLines(_fileInfo.FullName))
            {
                if (line.StartsWith("#"))
                {
                    textWriter.WriteLine($"{indent}{line}");
                }
                else
                {
                    textWriter.WriteLine(line);
                }
            }
        }
    }
}