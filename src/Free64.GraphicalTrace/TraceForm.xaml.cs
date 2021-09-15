using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Free64.Common;

namespace Free64.GraphicalTrace
{
    /// <summary>
    /// Represents <see cref="GraphicalTraceListener"/> main form.
    /// </summary>
    public partial class TraceForm : Window
    {
        public TraceForm()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            this.HideAdditionalButtons();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TraceListBox.SelectedItem == null) return;
            Clipboard.SetText((string)((TraceListBox.SelectedItem as ListBoxItem).Content as ContentControl).Content);
        }
    }
}
