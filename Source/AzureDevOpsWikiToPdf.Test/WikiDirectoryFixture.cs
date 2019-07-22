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

            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�͂��߂�.md")).FullName, dir.WikiEntries[0].FullName);

            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�")).FullName, dir.WikiEntries[1].FullName);
            Assert.Equal(3, dir.WikiEntries[1].WikiEntries.Count);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�", "����.md")).FullName, dir.WikiEntries[1].WikiEntries[0].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�", "�����ɂ���.md")).FullName, dir.WikiEntries[1].WikiEntries[1].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�", "��O��")).FullName, dir.WikiEntries[1].WikiEntries[2].FullName);
            Assert.Equal(3, dir.WikiEntries[1].WikiEntries[2].WikiEntries.Count);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�", "��O��", "��ꍀ.md")).FullName, dir.WikiEntries[1].WikiEntries[2].WikiEntries[0].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�", "��O��", "�����ɂ���.md")).FullName, dir.WikiEntries[1].WikiEntries[2].WikiEntries[1].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ŏ��̏�", "��O��", "��O��.md")).FullName, dir.WikiEntries[1].WikiEntries[2].WikiEntries[2].FullName);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ɂ΂�߂̏�")).FullName, dir.WikiEntries[2].FullName);
            Assert.Equal(1, dir.WikiEntries[2].WikiEntries.Count);
            Assert.Equal(new FileInfo(Path.Combine("Wiki", "�ɂ΂�߂̏�", "�ɂ΂�߂̏͂̑���.md")).FullName, dir.WikiEntries[2].WikiEntries[0].FullName);
        }

        [Fact]
        public void Write()
        {
            var dir = WikiDirectory.Parse("Wiki");

            Assert.Equal(File.ReadAllText(Path.Combine("Output", "�͂��߂�.md"), Encoding.UTF8), ToString(dir.WikiEntries[0]));
            Assert.Equal(File.ReadAllText(Path.Combine("Output", "�ŏ��̏�.md"), Encoding.UTF8), ToString(dir.WikiEntries[1]));
            Assert.Equal(File.ReadAllText(Path.Combine("Output", "�ɂ΂�߂̏�.md"), Encoding.UTF8), ToString(dir.WikiEntries[2]));
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
