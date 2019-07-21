using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureDevOpsWikiToPdf
{
    public class Catalog
    {
        private readonly WikiDirectory _wikiDirectory;
        private bool _enablePredef;

        public Catalog(WikiDirectory wikiDirectory, bool enablePredef = true)
        {
            _wikiDirectory = wikiDirectory;
            _enablePredef = enablePredef;
        }

        public void Write(TextWriter textWriter)
        {
            IWikiEntry predef = null;
            IEnumerable<IWikiEntry> chaps = _wikiDirectory.WikiEntries;
            //IWikiEntry appendix = null;
            //IWikiEntry postdef = null;

            if (_enablePredef)
            {
                predef = chaps.First();
                chaps = chaps.Skip(1);
            }

            textWriter.WriteLine("PREDEF:");

            if (predef != null)
            {
                textWriter.WriteLine($"  - {predef.ReViewName}");
            }
            textWriter.WriteLine();

            textWriter.WriteLine("CHAPS:");
            foreach (var wikiEntry in chaps)
            {
                textWriter.WriteLine($"  - {wikiEntry.ReViewName}");
            }
            textWriter.WriteLine();

            textWriter.WriteLine("APPENDIX:");
            textWriter.WriteLine();

            textWriter.WriteLine("POSTDEF:");

            textWriter.Flush();
        }
    }
}