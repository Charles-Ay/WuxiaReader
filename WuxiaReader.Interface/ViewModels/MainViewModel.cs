using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WuxiaReader.DataFetcher;
using WuxiaReader.Interface.Commands;
using WuxiaReader.Interface.Helpers;

namespace WuxiaReader.Interface.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string BaseUrl = "https://www.wuxiaworld.com/novel/overgeared/og-chapter-{0}";
        public int CurrentChapter { get; set; } = 30;

        public MainViewModel()
        {
            Chapters = new BindingList<Chapter>();

            LoadChapterCommand = new AsyncCommand<int>(LoadChapter);
            NextChapterCommand = new AsyncCommand(NextChapter);
            PreviousChapterCommand = new AsyncCommand(PreviousChapter);
        }

        public BindingList<Chapter> Chapters { get; }
        
        public ICommand LoadChapterCommand { get; }
        public ICommand NextChapterCommand { get; }
        public ICommand PreviousChapterCommand { get; }

        private async Task LoadChapter(int chapterNumber)
        {
            if (chapterNumber < 1)
                throw new ArgumentException("Chapter's number must be a positive integer", nameof(chapterNumber));
            
            var insertIndex =
                CollectionHelper.BinarySearch(Chapters, new Chapter(chapterNumber, null), ChapterComparer.Comparer);

            // Already in the array
            if (insertIndex >= 0)
                return;

            var chapter = await WuxiaFetcher.FetchChapter(BaseUrl, chapterNumber);
            
            Chapters.Insert(~insertIndex, chapter);
        }

        private async Task NextChapter()
        {
            await LoadChapter(++CurrentChapter);
        }

        private async Task PreviousChapter()
        {
            await LoadChapter(--CurrentChapter);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}