using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace WuxiaReader.DataFetcher
{
    internal static class WuxiaScraper
    {
        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/81.0.4044.129 Safari/537.36";

        public static async Task<List<ChapterElement>> FetchChapterElements(string url)
        {
            var page = await FetchPageContents(url);
            var html = ParseWebpage(page);
            
            var children = html.QuerySelectorAll("#chapter-content > *").ToArray();

            var elements = new List<ChapterElement>();

            foreach (var baseNode in children)
            {
                switch (baseNode.Name)
                {
                    case "p":
                        ParseParagraph(elements, baseNode);
                        break;
                    
                    case "a":
                        // Ignore: Previus and Next chapter buttons
                        Console.WriteLine("Skipped {0} node containing: {1}", 
                            baseNode.Name, baseNode.InnerText.Replace("\n", "\\n"));
                        break;
                    
                    case "hr":
                        elements.Add(new ChapterElement(new StylizedText[0], 10, true));
                        break;

                    case "br":
                        elements.Add(new ChapterElement(new StylizedText[0], 10));
                        break;
                    
                    default:
                        throw new Exception("Unexpected node in #chapter-content: " + baseNode.Name);
                }
            }

            FilterTranslationNotes(elements);
            
            return elements;
        }

        private static void FilterTranslationNotes(IList<ChapterElement> elements)
        {
            if (elements.Count < 10)
                return;
            
            var last = elements.Count - 1;
            for (var i = last - 2; i > last - 10; i--)
            {
                var current = elements[i];
                if (!current.HasSpacerAfter) 
                    continue;
                
                while (elements.Count > i + 1)
                {
                    Console.WriteLine("Skipped line {0} {1}", 
                        string.Join(' ', elements[i + 1].StylizedSplits.Select(x => x.Content)), 
                        elements[i + 1].HasSpacerAfter ? "(Spacer)" : string.Empty);
                    elements.RemoveAt(i + 1);
                }

                if (current.StylizedSplits.Length == 0)
                    elements.RemoveAt(i);
                else
                    current.HasSpacerAfter = false;
                break;
            }
        }

        [Conditional("DEBUG")]
        private static void AssertNoStyle(HtmlNode node)
        {
            Debug.Assert(!node.Attributes.Contains("class"));
            Debug.Assert(!node.Attributes.Contains("id"));

            if (!node.Attributes.Contains("style")) 
                return;
            
            var style = node.Attributes["style"].Value;

            if (style == string.Empty)
                return;
                
            Debug.Assert(style.IndexOf("font-weight", StringComparison.OrdinalIgnoreCase) < 0);
            Debug.Assert(style.IndexOf("font-family", StringComparison.OrdinalIgnoreCase) < 0);
            Debug.Assert(style.IndexOf("font-size", StringComparison.OrdinalIgnoreCase) < 0);
            Debug.Assert(style.IndexOf("font-style", StringComparison.OrdinalIgnoreCase) < 0);
            
            Console.WriteLine("Node({0}) with InnerText({1}) has Style({2})", node.Name, node.InnerText, style);
        }
        
        private static void ParseParagraph
        (
            ICollection<ChapterElement> elements, 
            HtmlNode node 
        )
        {
            AssertNoStyle(node);

            var splits = new List<StylizedText>();
            
            var children = node.Children().ToArray();
            foreach (var child in children)
            {
                if (child.Name == "hr")
                {
                    Debug.Assert(children.Length == 1); // Assert hr is the only child
                    elements.Add(new ChapterElement(splits.ToArray(), 10, true));
                    return;
                }

                ParseParagraphElement(splits, child);
            }
                
            elements.Add(new ChapterElement(splits.ToArray(), 10));
        }

        private static void ParseParagraphElement
        (
            ICollection<StylizedText> splits, 
            HtmlNode node, 
            bool isItalic = false, 
            int fontWeight = 400
        )
        {
            AssertNoStyle(node);
            
            switch (node.Name)
            {
                case "p":
                    foreach (var child in node.Children())
                        ParseParagraphElement(splits, child, isItalic, fontWeight);
                    break;
                
                case "em":
                case "i":
                    foreach (var child in node.Children())
                        ParseParagraphElement(splits, child, true, fontWeight);
                    break;
                
                case "strong":
                case "b":
                    foreach (var child in node.Children())
                        ParseParagraphElement(splits, child, isItalic, 800);
                    break;    
                
                case "span":
                    foreach (var child in node.Children())
                        ParseParagraphElement(splits, child, isItalic, fontWeight);
                    break;

                case "#text":
                {
                    var decodedText = HttpUtility.HtmlDecode(node.InnerText);
                    splits.Add(new StylizedText(decodedText, isItalic, fontWeight));
                    break;
                }

                case "a":
                {
                    var decodedText = HttpUtility.HtmlDecode(node.InnerText);
                    splits.Add(new StylizedText(decodedText, isItalic, fontWeight));
                    break;
                }

                case "br":
                {
                    splits.Add(new StylizedText("\n"));
                    break;
                }
                
                default:
                    throw new Exception("Unexpected node in paragraph: " + node.Name);
            }
        }
        
        private static HtmlNode ParseWebpage(string page)
        {
            var html = new HtmlDocument();
            html.LoadHtml(page);
            return html.DocumentNode;
        }

        private static async Task<string> FetchPageContents(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Get;
            request.UserAgent = UserAgent;

            using var response = (HttpWebResponse) await request.GetResponseAsync();
            await using var stream = response.GetResponseStream();

            if (stream == null)
                throw new Exception("Failed to fetch webpage");
            
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }
    }
}