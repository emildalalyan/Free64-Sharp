using System;
using System.Windows.Controls;

namespace Free64.Debug
{
    public class Debug : IDisposable
    {
        public Debug() {}

        protected fmDebug Form = new fmDebug();

        protected int __LeftOffset = 6;
        
        public int LeftOffset
        {
            get
            {
                return __LeftOffset;
            }
            set
            {
                __LeftOffset = value;

                if (Form.DebugListBox.Items.Count < 1) return;
                foreach(ListBoxItem Item in Form.DebugListBox.Items)
                {
                    ((ContentControl)Item.Content).Margin = new System.Windows.Thickness()
                    {
                        Left = value
                    };
                }
            }
        }

        /// <summary>
        /// Send string message to Debug.
        /// </summary>
        /// <param name="Message"></param>
        /// <returns><see cref="int"/> ItemIndex</returns>
        public int SendMessage(string Message)
        {
            Message = $"> {Message}";
            return Form.DebugListBox.Items.Add(new ListBoxItem()
            {
                Content = new ContentControl()
                {
                    Content = Message,
                    Margin = new System.Windows.Thickness()
                    {
                        Left = __LeftOffset
                    }
                },
                Height = 22
            });
        }

        /// <summary>
        /// Append to a message by Index
        /// </summary>
        /// <param name="Append"></param>
        /// <param name="Index"></param>
        public void AppendToMessage(string Append, int Index)
        {
            if (Form.DebugListBox.Items[Index] != null) ((ContentControl)((ListBoxItem)Form.DebugListBox.Items[Index]).Content).Content += Append;
        }
        public void Clear()
        {
            Form.DebugListBox.Items.Clear();
        }
        public void Show()
        {
            this.Form.Show();
            this.Form.Focus();
        }
        public void Dispose()
        {
            this.Form.Close();
        }
    }
}
