using System.Threading.Tasks;
using WuxiaReader.Shared;

namespace WuxiaReader.DataFetcher
{
    public static class WuxiaFetcher
    {
        public static async Task<Chapter> FetchChapter(string chapterUrl, int chapterNumber)
        {
            return new Chapter(
                chapterNumber,
                await WuxiaScraper.FetchChapterElements(chapterUrl));
        }
    }
}