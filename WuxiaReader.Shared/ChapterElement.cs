using System.ComponentModel;
using System.Windows;

namespace WuxiaReader.Shared
{
    public class ChapterElement : INotifyPropertyChanged
    {
        public ChapterElement
        (
            StylizedText[] stylizedSplits,
            int bottomMargin,
            TextAlignment alignment,
            bool hasSpacerAfter = false
        ) : this(stylizedSplits, new Thickness(0, 0, 0, bottomMargin), alignment, hasSpacerAfter)
        {
            
        }
        
        public ChapterElement
        (
            StylizedText[] stylizedSplits, 
            Thickness margin, 
            TextAlignment alignment, 
            bool hasSpacerAfter = false
        )
        {
            StylizedSplits = stylizedSplits;
            Margin = margin;
            Alignment = alignment;
            HasSpacerAfter = hasSpacerAfter;
        }

        public StylizedText[] StylizedSplits { get; }
        public Thickness Margin { get; }
        public TextAlignment Alignment { get; }
        public bool HasSpacerAfter { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}