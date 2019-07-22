using System.Collections.Generic;
using System.IO;

namespace AzureDevOpsWikiToPdf
{
    public abstract class WikiEntry : IWikiEntry
    {
        public abstract string FullName { get; }
        public abstract string MarkdownName { get; }
        public abstract string ReViewName { get; }
        public abstract IReadOnlyList<IWikiEntry> WikiEntries { get; }
        public abstract void Write(TextWriter textWriter);
        internal abstract void Write(TextWriter textWriter, int indent);
    }
}