using System.Collections.Generic;
using System.IO;

namespace AzureDevOpsWikiToPdf
{
    public interface IWikiEntry
    {
        string FullName { get; }

        string MarkdownName { get; }

        string ReViewName { get; }

        IReadOnlyList<IWikiEntry> WikiEntries { get; }

        void Write(TextWriter textWriter);
    }
}