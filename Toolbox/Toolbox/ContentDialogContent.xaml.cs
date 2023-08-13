using Microsoft.UI.Xaml.Controls;

namespace Toolbox
{
    public sealed partial class ContentDialogContent : Page
    {
        private string _content;

        public ContentDialogContent()
        {
            this.InitializeComponent();
        }

        public ContentDialogContent(string content) : this()
        {
            Content.Text = content;
        }
    }
}
