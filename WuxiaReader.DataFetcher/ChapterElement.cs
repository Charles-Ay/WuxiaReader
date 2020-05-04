using System.ComponentModel;

namespace WuxiaReader.DataFetcher
{
    public class ChapterElement : INotifyPropertyChanged
    {
        public ChapterElement(StylizedText[] stylizedSplits, int spacing = 0, bool hasSpacerAfter = false)
        {
            StylizedSplits = stylizedSplits;
            Spacing = spacing;
            HasSpacerAfter = hasSpacerAfter;
        }

        public StylizedText[] StylizedSplits { get; }
        public int Spacing { get; set; }
        public bool HasSpacerAfter { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}