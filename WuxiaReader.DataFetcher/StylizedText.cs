using System.ComponentModel;

namespace WuxiaReader.DataFetcher
{
    public class StylizedText : INotifyPropertyChanged
    {
        public StylizedText(string content, bool isItalic = false, int fontWeight = 400)
        {
            Content = content;
            IsItalic = isItalic;
            FontWeight = fontWeight;
        }

        public string Content { get; }
        public bool IsItalic { get; }
        public int FontWeight { get; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}