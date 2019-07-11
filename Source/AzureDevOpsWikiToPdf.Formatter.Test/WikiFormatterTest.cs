using System;
using System.IO;
using System.Text;
using Xunit;

namespace AzureDevOpsWikiToPdf.Formatter.Test
{
    public class WikiFormatterTest
    {
        [Fact]
        public void Format()
        {
            var wikiFormatter = new WikiFormatter();
            wikiFormatter.Format(new DirectoryInfo("Wiki"), new DirectoryInfo("Dest"));
        }
    }
}
