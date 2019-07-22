using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            Write(textWriter, 0);
        }

        internal override void Write(TextWriter textWriter, int indent)
        {
            var indentBuilder = new StringBuilder();
            var imagePathBuilder = new StringBuilder();
            for (int i = 0; i < indent; i++)
            {
                indentBuilder.Append('#');
                imagePathBuilder.Append("../");
            }
            var indentString = indentBuilder.ToString();
            var imagePath = imagePathBuilder.ToString();

            foreach (var line in File.ReadAllLines(_fileInfo.FullName))
            {
                if (line.StartsWith("#"))
                {
                    textWriter.WriteLine($"{indentString}{line}");
                }
                else
                {
                    textWriter.WriteLine(indent == 0 ? line : line.Replace(imagePath, string.Empty));
                }
            }
        }
    }
}