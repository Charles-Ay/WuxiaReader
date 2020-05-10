using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Xaml.Behaviors;
using WuxiaReader.Shared;

namespace WuxiaReader.Interface.Behaviors
{
    public class StylizedTextBehaviour : Behavior<TextBlock>
    {
        public ChapterElement CurrentChapter
        {
            get => (ChapterElement) GetValue(CurrentChapterProperty);
            set => SetValue(CurrentChapterProperty, value);
        }
        
        protected override void OnAttached()
        {
            AssociatedObject.Inlines.Clear();

            foreach (var split in CurrentChapter.StylizedSplits)
            {
                AssociatedObject.Inlines.Add(new Run
                {
                    Text = split.Content,
                    FontStyle = split.FontStyle,
                    FontWeight = split.FontWeight
                });
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Inlines.Clear();
        }
        
        public static readonly DependencyProperty CurrentChapterProperty =
            DependencyProperty.Register(nameof(CurrentChapter), typeof(ChapterElement), typeof(StylizedTextBehaviour));
    }
}