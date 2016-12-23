using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace WebTask.Controllers.Format
{
    public class HtmlMessageFormat
    {
        public string Format(string text)
        {
            var document = new HtmlDocument();
            document.LoadHtml(text);
            var parts = FormatInternal(document)
                .SelectMany(x=>x)
                .Select(x=>x.Trim());
            return string.Join("", parts);
        }

        private IEnumerable<IEnumerable<string>> FormatInternal(HtmlDocument document)
        {
            foreach (var node in document.DocumentNode.ChildNodes)
            {
                switch (node.Name.ToLower())
                {
                    case "#text":
                        yield return new [] {node.InnerText};
                        break;
                    case "img":
                        yield return ImageNode(node);
                        break;
                    case "h1":
                    case "h2":
                    case "h3":
                    case "b":
                    case "i":
                        yield return Node(node);
                        break;
                }
            }
        }

        private IEnumerable<string> ImageNode(HtmlNode node)
        {
            if (!node.HasChildNodes && node.Attributes.Contains("src"))
                yield return $"<img src={node.Attributes["src"].Value}/>";
        }

        private IEnumerable<string> Node(HtmlNode node)
        {
            if (node.ChildNodes.Count == 1 && node.ChildNodes.Single().Name == "#text")
                yield return $"<{node.Name}>{node.InnerText}</{node.Name}>";
        }
    }
}