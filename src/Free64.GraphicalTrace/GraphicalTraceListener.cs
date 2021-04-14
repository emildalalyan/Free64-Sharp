using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Free64.GraphicalTrace
{
    /// <summary>
    /// Class intended to trace events in <see cref="Window"/>
    /// </summary>
    public class GraphicalTraceListener : TraceListener
    {
        public GraphicalTraceListener() {}

        /// <summary>
        /// <see cref="Window"/> of graphical trace listener 
        /// </summary>
        protected fmTrace Form = new();

        protected double __LeftOffset = 6;
        
        /// <summary>
        /// Left offset (margin) of text in <see cref="ListBox"/>
        /// </summary>
        public double LeftOffset
        {
            get
            {
                return __LeftOffset;
            }
            set
            {
                __LeftOffset = value;

                if (Form.TraceListBox.Items.Count < 1) return;
                foreach(ListBoxItem Item in Form.TraceListBox.Items)
                {
                    ((ContentControl)Item.Content).Margin = new System.Windows.Thickness()
                    {
                        Left = value
                    };
                }
            }
        }

        /// <summary>
        /// Color of <c>module></c> elements in <see cref="ListBox"/>
        /// </summary>
        public Brush ModuleElementsColor { get; set; } = new SolidColorBrush(new()
        {
            R = 75,
            G = 75,
            B = 75,
            A = 255
        });

        /// <summary>
        /// Write <see cref="string"/> with a line terminator.
        /// </summary>
        /// <param name="Message"></param>
        public override void WriteLine(string Message)
        {
            bool ismodulemsg = Message.StartsWith("module> ");

            if (Form.TraceListBox.Items.Count > 0 && (bool)((ListBoxItem)Form.TraceListBox.Items[^1]).Tag == false)
            {
                ((ContentControl)((ListBoxItem)Form.TraceListBox.Items[^1]).Content).Content += Message;
                ((ListBoxItem)Form.TraceListBox.Items[^1]).Tag = true;
            }
            else Form.TraceListBox.Items.Add(new ListBoxItem()
            {
                Foreground = ismodulemsg ? ModuleElementsColor : new SolidColorBrush(Colors.Black),
                Content = new ContentControl()
                {
                    Margin = new()
                    {
                        Left = ismodulemsg ? __LeftOffset + 16 : __LeftOffset
                    },
                    Content = $"> {(ismodulemsg ? Message.Replace("module> ", "") : Message)}"
                },
                Height = 22,
                Tag = true
            });
        }

        /// <summary>
        /// Write <see cref="object"/> with a line terminator.
        /// </summary>
        /// <param name="Message"></param>
        public override void WriteLine(object Object)
        {
            if (Object != null) WriteLine(Object.ToString());
        }

        /// <summary>
        /// Write <see cref="object"/> without a line terminator.
        /// </summary>
        /// <param name="Message"></param>
        public override void Write(object Object)
        {
            if (Object != null) Write(Object.ToString());
        }

        /// <summary>
        /// Write <see cref="string"/> without a line terminator.
        /// </summary>
        /// <param name="Append"></param>
        /// <param name="Index"></param>
        public override void Write(string Append)
        {
            bool ismodulemsg = Append.StartsWith("module> ");

            if (Form.TraceListBox.Items.Count < 1 || (bool)((ListBoxItem)Form.TraceListBox.Items[^1]).Tag == true)
            {
                Form.TraceListBox.Items.Add(new ListBoxItem()
                {
                    Foreground = ismodulemsg ? ModuleElementsColor : new SolidColorBrush(Colors.Black),
                    Content = new ContentControl()
                    {
                        Margin = new()
                        {
                            Left = ismodulemsg ? __LeftOffset + 16 : __LeftOffset
                        },
                        Content = $"> {(ismodulemsg ? Append.Replace("module> ", "") : Append)}"
                    },
                    Height = 22,
                    Tag = false
                });
            }
            else ((ContentControl)((ListBoxItem)Form.TraceListBox.Items[^1]).Content).Content += Append;
        }

        /// <summary>
        /// Clear debug messages.
        /// </summary>
        public void Clear()
        {
            Form.TraceListBox.Items.Clear();
        }

        /// <summary>
        /// Show Debug Window
        /// </summary>
        public void Show()
        {
            this.Form.Show();
            this.Form.Focus();
        }

        /// <summary>
        /// Unload resources.
        /// </summary>
        public new void Dispose()
        {
            this.Form.Close();
        }
    }
}
