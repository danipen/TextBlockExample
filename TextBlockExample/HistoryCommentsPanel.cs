using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace TextBlockExample
{
    internal class HistoryCommentsPanel : StackPanel
    {
        // exposed for testing
        internal Button ToggleCommentDetailsButton => mToggleCommentDetailsButton;
        internal TextBox FirstLineCommentLabel => mFirstLineCommentLabel;
        internal TextBox CommentRestLabel => mCommentRestLabel;

        internal HistoryCommentsPanel()
        {
            BuildComponents(150);
        }

        internal void Dispose()
        {
            mToggleCommentDetailsButton.Click -= ToggleCommentDetailsButton_Click;
        }

        internal void SetComment(string comment, bool bIsShelve = false)
        {
            comment = string.IsNullOrEmpty(comment) ?
                "no comment" : comment;

            mSplitComment = SplitComment.Split(comment);

            bool hasRestText = !string.IsNullOrEmpty(mSplitComment.Rest);

            UpdateCommentText(hasRestText);

            mToggleCommentDetailsButton.IsVisible = hasRestText;
            mCommentRestLabel.IsVisible = mAreCommentsExpanded && hasRestText;
        }

        internal void CollapseComments()
        {
            mAreCommentsExpanded = false;
            UpdateExpandedCommentControls();
        }

        void UpdateCommentText(bool hasRestText)
        {
            if (mSplitComment == null)
                return;

            if (NeedSplitSeparator(hasRestText))
            {
                mFirstLineCommentLabel.Text = mSplitComment.FirstLine.TrimEnd('.') + SPLIT_SEPARATOR;
                mCommentRestLabel.Text = SPLIT_SEPARATOR + mSplitComment.Rest.TrimStart('.');
                return;
            }

            mFirstLineCommentLabel.Text = mSplitComment.FirstLine;
            mCommentRestLabel.Text = mSplitComment.Rest;
        }

        bool NeedSplitSeparator(bool hasRestText)
        {
            if (!hasRestText)
                return false;

            if (!mAreCommentsExpanded)
                return true;

            if (!mSplitComment.IsWordSplitOrLineBreak)
                return true;

            return false;
        }

        void ToggleCommentDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            mAreCommentsExpanded = !mAreCommentsExpanded;
            UpdateExpandedCommentControls();
        }

        void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            mAreCommentsExpanded = true;
            UpdateExpandedCommentControls();
        }

        void UpdateExpandedCommentControls()
        {
            mCommentRestLabel.IsVisible = mAreCommentsExpanded;

            UpdateCommentText(mToggleCommentDetailsButton.IsVisible);
        }

        void BuildComponents(int commentsRestLabelMaxHeight)
        {
            StackPanel firstLinePanel = new StackPanel();
            firstLinePanel.Orientation = Orientation.Horizontal;

            int imageSize = 16;

            mToggleCommentDetailsButton = new Button();
            mToggleCommentDetailsButton.Content = "Toggle";
            mToggleCommentDetailsButton.MinHeight = 0;
            mToggleCommentDetailsButton.Padding = new Thickness(0);
            mToggleCommentDetailsButton.Margin = new Thickness(0, 0, 2, 0);

            mToggleCommentDetailsButton.Click += ToggleCommentDetailsButton_Click;

            mFirstLineCommentLabel = ControlBuilder.CreateSelectableLabel(null);
            mFirstLineCommentLabel.VerticalAlignment = VerticalAlignment.Center;
            mFirstLineCommentLabel.VerticalContentAlignment = VerticalAlignment.Center;
            mFirstLineCommentLabel.MinHeight = 0;

            mCommentRestLabel = ControlBuilder.CreateSelectableMultilineLabel();
            mCommentRestLabel.TextWrapping = TextWrapping.Wrap;
            mCommentRestLabel.MinHeight = 0;
            mCommentRestLabel.MaxHeight = commentsRestLabelMaxHeight;
            mCommentRestLabel.Margin = new Thickness(imageSize + 2, 0, 0, 0);

            firstLinePanel.Children.Add(mToggleCommentDetailsButton);
            firstLinePanel.Children.Add(mFirstLineCommentLabel);

            Children.Add(firstLinePanel);
            Children.Add(mCommentRestLabel);
        }

        const string SPLIT_SEPARATOR = "...";

        Button mToggleCommentDetailsButton;
        TextBox mFirstLineCommentLabel;
        TextBox mCommentRestLabel;

        SplitComment mSplitComment;

        bool mAreCommentsExpanded = true;
    }

    public class SplitComment
    {
        public bool IsWordSplitOrLineBreak { get; private set; }
        public string FirstLine { get; private set; }
        public string Rest { get; private set; }

        public static SplitComment Split(string comment)
        {
            return Split(comment, 100);
        }

        public static SplitComment Split(string comment, int splitIndex)
        {
            SplitComment result = new SplitComment();

            if (string.IsNullOrEmpty(comment))
                return result;

            comment = comment.Replace("\r\n", "\n");
            string[] lines = comment.Split(new char[] { '\n' });

            if (lines[0].Length > splitIndex)
            {
                int wordSplitIndex = lines[0]
                    .Substring(0, splitIndex)
                    .LastIndexOfAny(WORD_SEPARATORS.ToCharArray());

                if (wordSplitIndex < Math.Max(0, splitIndex - 20))
                    wordSplitIndex = splitIndex;

                result.IsWordSplitOrLineBreak = IsSplitByWord(splitIndex, wordSplitIndex, lines[0].Length);

                result.FirstLine = lines[0].Substring(0, wordSplitIndex).Trim();
                result.Rest = lines[0].Substring(wordSplitIndex).Trim();
            }
            else
            {
                result.IsWordSplitOrLineBreak = true;
                result.FirstLine = lines[0];
            }

            if (lines.Length <= 1)
                return result;

            if (!string.IsNullOrEmpty(result.Rest))
                result.Rest += Environment.NewLine;

            result.Rest += string.Join(Environment.NewLine, lines, 1, lines.Length - 1);
            return result;
        }

        static bool IsSplitByWord(int splitIndex, int wordSplitIndex, int lineLength)
        {
            if (lineLength <= splitIndex)
                return true;

            if (wordSplitIndex != splitIndex)
                return true;

            return false;
        }

        const string WORD_SEPARATORS = " ,.;:!?)]/\\|-_=+~&^";
    }

    public class ControlBuilder
    {
        public static TextBox CreateSelectableLabel(string text)
        {
            TextBox result = new TextBox();
            result.BorderThickness = new Thickness(0);
            result.Text = text;
            result.TextWrapping = TextWrapping.NoWrap;
            result.VerticalAlignment = VerticalAlignment.Center;
            result.IsReadOnly = true;

            KeyboardNavigation.SetIsTabStop(result, false);
            ScrollViewer.SetVerticalScrollBarVisibility(result, ScrollBarVisibility.Hidden);

            return result;
        }

        public static TextBox CreateSelectableMultilineLabel()
        {
            TextBox result = new TextBox();
            result.AcceptsReturn = true;
            result.TextWrapping = TextWrapping.Wrap;
            result.VerticalContentAlignment = VerticalAlignment.Center;
            result.IsReadOnly = true;

            KeyboardNavigation.SetIsTabStop(result, false);
            ScrollViewer.SetVerticalScrollBarVisibility(result, ScrollBarVisibility.Auto);

            return result;
        }
    }
}
