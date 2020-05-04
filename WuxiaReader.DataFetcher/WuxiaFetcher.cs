using System;
using System.Threading.Tasks;

namespace WuxiaReader.DataFetcher
{
    public static class WuxiaFetcher
    {
        public static async Task<Chapter> FetchChapter(string url, int chapterNumber)
        {
            return new Chapter(
                chapterNumber,
                await WuxiaScraper.FetchChapterElements(string.Format(url, chapterNumber)));
        }
    }
}