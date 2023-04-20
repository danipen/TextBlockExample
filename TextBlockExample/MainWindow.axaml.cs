using Avalonia.Controls;

namespace TextBlockExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mButton1.Click += Button1_Click;
            mButton2.Click += Button2_Click;
            mButton3.Click += Button3_Click;

            mHistoryCommentsPanel = new HistoryCommentsPanel();
            mContentPanel.Children.Add(mHistoryCommentsPanel);
            mHistoryCommentsPanel.SetComment(TEXT3);
        }

        private void Button1_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mHistoryCommentsPanel.SetComment(TEXT1);
        }

        private void Button2_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mHistoryCommentsPanel.SetComment(TEXT2);
        }

        private void Button3_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mHistoryCommentsPanel.SetComment(TEXT3);
        }

        HistoryCommentsPanel mHistoryCommentsPanel;

        const string TEXT1 = "The WebUI annotate is the only one that uses basecommands directly (and the possible CmConnection calls inside), so just pass the handlers and remove the ugly FakeCmConnection from the server to ensure that is never used again. The right way will be just passing the server handlers to do the things...";
        const string TEXT2 = "Shelve changes";
        const string TEXT3 = "We found these errors in Plastic version 11.0.16.7792:\r\nIf you are on a label (regular workspace) and then open the workspace in GluonX, you will see that the status for all items is an error message. If you try to convert to a partial workspace, GluonX will show an error window. So:\r\n\r\nPlease check what legacy Gluon does! I'm not sure.\r\nWe should decide what GluonX is expected to do:\r\nAt the very least it should not fail like that!\r\nAlso, the status message should not be an error.\r\nMaybe, it could handle the case and convert the workspace. If too hard, consider something else.\r\nWe found these errors in Plastic version 11.0.16.7792:\r\n\r\nIf you are on a label (regular workspace) and then open the workspace in GluonX, you will see that the status for all items is an error message.\r\nIf you try to convert to a partial workspace, GluonX will show an error window.\r\nSo:\r\n\r\nPlease check what legacy Gluon does! I'm not sure.\r\nWe should decide what GluonX is expected to do:\r\nAt the very least it should not fail like that!\r\nAlso, the status message should not be an error.\r\nMaybe, it could handle the case and convert the workspace. If too hard, consider something else.We found these errors in Plastic version 11.0.16.7792:\r\n\r\nIf you are on a label (regular workspace) and then open the workspace in GluonX, you will see that the status for all items is an error message.\r\nIf you try to convert to a partial workspace, GluonX will show an error window.\r\nSo:\r\n\r\nPlease check what legacy Gluon does! I'm not sure.\r\nWe should decide what GluonX is expected to do:\r\nAt the very least it should not fail like that!\r\nAlso, the status message should not be an error.\r\nMaybe, it could handle the case and convert the workspace. If too hard, consider something else.";
    }
}