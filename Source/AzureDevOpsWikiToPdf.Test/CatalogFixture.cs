using System.IO;
using System.Linq;
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
        public void Load()
        {
            using var reader = new StreamReader(Path.Combine("Catalog", "case01-md.yml"), Encoding.UTF8);

            var catalog = Catalog.Load(reader);

            Assert.NotNull(catalog);

            Assert.NotNull(catalog.Predef);
            Assert.Single(catalog.Predef);
            Assert.Equal("chap00-preface.md", catalog.Predef.Single());
            
            Assert.NotNull(catalog.Chaps);
            Assert.Equal(2, catalog.Chaps.Count);
            Assert.Equal("chap01-starter.md", catalog.Chaps[0]);
            Assert.Equal("chap02-faq.md", catalog.Chaps[1]);

            Assert.Null(catalog.Appendix);

            Assert.NotNull(catalog.Postdef);
            Assert.Single(catalog.Postdef);
            Assert.Equal("chap99-postscript.md", catalog.Postdef.Single());
        }

        [Fact]
        public void Scenario()
        {
            using var reader = new StreamReader(Path.Combine("Catalog", "scenario-md.yml"), Encoding.GetEncoding("Shift_JIS"));

            var catalog = Catalog.Load(reader);
            catalog.Append(WikiDirectory.Parse("Wiki"));

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.GetEncoding("Shift_JIS"));

            catalog.SaveToReViewCatalog(writer);
            writer.Flush();

            Assert.Equal(
                File.ReadAllText(Path.Combine("Catalog", "scenario-re.yml"), Encoding.GetEncoding("Shift_JIS")),
                Encoding.GetEncoding("Shift_JIS").GetString(stream.ToArray()));
        }
    }
}