using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;
using WuxiaReader.DataFetcher;
using WuxiaReader.Interface.Commands;
using WuxiaReader.Interface.Helpers;

namespace WuxiaReader.Interface.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private string _formatUrl = "https://www.wuxiaworld.com/novel/overgeared/og-chapter-{0}";
        private int _currentChapter = 33;

        public MainViewModel()
        {
            Chapters = new BindingList<Chapter>();

            LoadChapterCommand = new AsyncCommand<int>(LoadChapter);
            NextChapterCommand = new AsyncCommand(NextChapter);
            PreviousChapterCommand = new AsyncCommand(PreviousChapter);
            SetBaseTheme = new ActionCommand<IBaseTheme>(SetApplicationBaseTheme);
        }

        public BindingList<Chapter> Chapters { get; }
        
        public ICommand LoadChapterCommand { get; }
        public ICommand NextChapterCommand { get; }
        public ICommand PreviousChapterCommand { get; }
        public ICommand SetBaseTheme { get; }

        public int CurrentChapter
        {
            get => _currentChapter;
            set
            {
                if (value == _currentChapter) return;
                _currentChapter = value;
                OnPropertyChanged();
            }
        }

        public string FormatUrl
        {
            get => _formatUrl;
            set
            {
                if (value == _formatUrl) return;
                _formatUrl = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadChapter(int chapterNumber)
        {
            if (chapterNumber < 1)
                throw new ArgumentException("Chapter's number must be a positive integer", nameof(chapterNumber));
            
            var insertIndex =
                CollectionHelper.BinarySearch(Chapters, new Chapter(chapterNumber, null), ChapterComparer.Comparer);

            // Already in the array
            if (insertIndex >= 0)
                return;

            var url = string.Format(FormatUrl, chapterNumber);
            var chapter = await WuxiaFetcher.FetchChapter(url, chapterNumber);
            
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

        private static void SetApplicationBaseTheme(IBaseTheme type)
        {
            var paletteHelper = new PaletteHelper();

            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(type);
            
            paletteHelper.SetTheme(theme);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}