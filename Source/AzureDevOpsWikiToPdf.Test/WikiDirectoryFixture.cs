using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AzureDevOpsWikiToPdf.Test
{
    public class WikiDirectoryFixture
    {
        [Fact]
        public void ParseWhenNormal()
        {
            var dir = WikiDirectory.Parse("Wiki");

            Assert.Equal(new DirectoryInfo("Wiki").FullName, dir.FullName);
            Assert.Equal(3, dir.WikiEntries.Count());

            Assert.Equal(new FileInfo(Path.Combine("Wiki", "はじめに.md")).FullName, dir.WikiEntries[0].FullName);

            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章")).FullName, dir.WikiEntries[1].FullName);
            Assert.Equal(4, dir.WikiEntries[1].WikiEntries.Count);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "最初の章.md")).FullName, dir.WikiEntries[1].WikiEntries[0].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "第一節.md")).FullName, dir.WikiEntries[1].WikiEntries[1].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "だいにせつ.md")).FullName, dir.WikiEntries[1].WikiEntries[2].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "第三節")).FullName, dir.WikiEntries[1].WikiEntries[3].FullName);
            Assert.Equal(4, dir.WikiEntries[1].WikiEntries[3].WikiEntries.Count);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "第三節", "第三節.md")).FullName, dir.WikiEntries[1].WikiEntries[3].WikiEntries[0].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "第三節", "第一項.md")).FullName, dir.WikiEntries[1].WikiEntries[3].WikiEntries[1].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "第三節", "だいにこう.md")).FullName, dir.WikiEntries[1].WikiEntries[3].WikiEntries[2].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "最初の章", "第三節", "第三項.md")).FullName, dir.WikiEntries[1].WikiEntries[3].WikiEntries[3].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "にばんめの章")).FullName, dir.WikiEntries[2].FullName);
            Assert.Equal(2, dir.WikiEntries[2].WikiEntries.Count);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "にばんめの章", "にばんめの章.md")).FullName, dir.WikiEntries[2].WikiEntries[0].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "にばんめの章", "にばんめの章の第一節.md")).FullName, dir.WikiEntries[2].WikiEntries[1].FullName);
        }

        [Fact]
        public void Write()
        {
            var dir = WikiDirectory.Parse("Wiki");

            Assert.Equal(File.ReadAllText(Path.Combine("Output", "はじめに.md"), Encoding.UTF8), ToString(dir.WikiEntries[0]));
            Assert.Equal(File.ReadAllText(Path.Combine("Output", "最初の章.md"), Encoding.UTF8), ToString(dir.WikiEntries[1]));
            Assert.Equal(File.ReadAllText(Path.Combine("Output", "にばんめの章.md"), Encoding.UTF8), ToString(dir.WikiEntries[2]));
        }

        private string ToString(IWikiEntry wikiEntry)
        {
            using var memoryStream = new MemoryStream();
            using var textWriter = new StreamWriter(memoryStream, new UTF8Encoding(false));

            wikiEntry.Write(textWriter);

            textWriter.Flush();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}
