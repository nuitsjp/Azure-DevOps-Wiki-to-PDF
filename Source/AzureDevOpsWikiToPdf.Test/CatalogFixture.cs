using System.IO;
using System.Text;
using Xunit;

namespace AzureDevOpsWikiToPdf.Test
{
    public class CatalogFixture
    {
        static CatalogFixture()
        {
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        [Fact]
        public void WhenEnablePredef()
        {
            var catalog = new Catalog(WikiDirectory.Parse("Wiki"));
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.GetEncoding("Shift_JIS"));

            catalog.Write(writer);
            Assert.Equal(
                File.ReadAllText(Path.Combine("Output", "catalog01.yml"), Encoding.GetEncoding("Shift_JIS")),
                Encoding.GetEncoding("Shift_JIS").GetString(stream.ToArray()));
        }
    }
}