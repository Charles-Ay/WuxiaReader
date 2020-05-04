using System;
using WuxiaReader.DataFetcher;

namespace WuxiaReader.Test
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            const string url = "https://www.wuxiaworld.com/novel/overgeared/og-chapter-{0}";
            var a =WuxiaFetcher.FetchChapter(url, 18);
            ;
        }
    }
}