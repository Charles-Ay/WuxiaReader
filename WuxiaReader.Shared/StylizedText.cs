using System.ComponentModel;
using System.Windows;

namespace WuxiaReader.Shared
{
    public class StylizedText : INotifyPropertyChanged
    {
        public StylizedText(string content, FontStyle fontStyle, FontWeight fontWeight)
        {
            Content = content;
            FontStyle = fontStyle;
            FontWeight = fontWeight;
        }

        public string Content { get; }
        public FontStyle FontStyle { get; }
        public FontWeight FontWeight { get; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}