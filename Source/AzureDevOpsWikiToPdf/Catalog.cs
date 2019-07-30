using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AzureDevOpsWikiToPdf
{
    public class Catalog
    {
        [YamlMember(Alias = "PREDEF")]
        public List<string> Predef { get; set; }

        [YamlMember(Alias = "CHAPS")]
        public List<string> Chaps { get; set; }

        [YamlMember(Alias = "APPENDIX")]
        public List<string> Appendix { get; set; }

        [YamlMember(Alias = "POSTDEF")]
        public List<string> Postdef { get; set; }

        public static Catalog Load(TextReader reader)
        {
            return 
                new DeserializerBuilder()
                    .Build()
                    .Deserialize<Catalog>(reader);
        }

        public void Append(WikiDirectory wikiDirectory)
        {
            if (Predef == null) Predef = new List<string>();
            if (Chaps == null) Chaps = new List<string>();
            if (Appendix == null) Appendix = new List<string>();
            if (Postdef == null) Postdef = new List<string>();

            var items = 
                wikiDirectory
                    .WikiEntries
                    .Select(x => x.MarkdownName)
                    .Where(
                        x => !Predef.Contains(x)
                            && !Chaps.Contains(x)
                            && !Appendix.Contains(x)
                            && !Postdef.Contains(x));
            foreach (var item in items)
            {
                Chaps.Add(item);
            }
        }

        public void SaveToReViewCatalog(TextWriter writer)
        {
            var reViewCatalog = new Catalog
            {
                Predef = ToReView(Predef),
                Chaps = ToReView(Chaps),
                Appendix = ToReView(Appendix),
                Postdef = ToReView(Postdef)
            };

            var serializer = new SerializerBuilder().Build();
            serializer.Serialize(writer, reViewCatalog);
        }

        private List<string> ToReView(List<string> items)
        {
            return
                items?
                    .Select(x => x.Substring(0, x.Length - new FileInfo(x).Extension.Length) + ".re")
                    .ToList();
        }
    }
}