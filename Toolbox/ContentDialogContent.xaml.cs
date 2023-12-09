using Microsoft.UI.Xaml.Controls;

namespace Toolbox
{
    public sealed partial class ContentDialogContent : Page
    {
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
