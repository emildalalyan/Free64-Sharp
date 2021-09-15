using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Free64.GraphicalTrace;

namespace Free64
{
    /// <summary>
    /// Class intended to listening <see cref="Trace"/> in <see cref="Window"/>
    /// </summary>
    public class GraphicalTraceListener : TraceListener
    {
        /// <summary>
        /// Creates an instance of the <see cref="GraphicalTraceListener"/> class.
        /// </summary>
        public GraphicalTraceListener() { }

        /// <summary>
        /// Trace listener <see cref="Window"/> instance.
        /// </summary>
        protected TraceForm Form { get; } = new();

        /// <summary>
        /// Offset (margin) of the text in <see cref="ListBox"/>
        /// </summary>
        public Thickness MessagesOffset { get; init; } = new(6, 0, 0, 0);

        /// <summary>
        /// Offset (margin) of the nested text in <see cref="ListBox"/>
        /// </summary>
        public Thickness NestedMessagesOffset { get; init; } = new(16, 0, 0, 0);

        /// <summary>
        /// Messages, which starts with this <see langword="value"/> will be considered as <b>nested</b>.
        /// <para>Default <see langword="value"/> of this property is <b>"\t"</b>.</para>
        /// </summary>
        public string NestedMessagesBeginning { get; init; } = "\t";

        /// <summary>
        /// Messages, which starts with this <see langword="value"/> will be displayed in <b>bold</b>.
        /// <para>Default <see langword="value"/> of this property is <b>"**"</b>.</para>
        /// </summary>
        public string BoldMessagesBeginning { get; init; } = "**";

        /// <summary>
        /// Color of nested messages in <see cref="ListBox"/>
        /// <para>Default <see langword="value"/> of this property is <b>"#4B4B4B"</b>.</para>
        /// </summary>
        public Brush NestedMessagesColor { get; init; } = new SolidColorBrush(new()
        {
            R = 0,
            G = 0,
            B = 0,
            A = 220
        });

        /// <summary>
        /// Color of messages in <see cref="ListBox"/>
        /// <para>This <see langword="value"/> is <b>#000000</b> by default.</para>
        /// </summary>
        public Brush MessagesColor { get; init; } = new SolidColorBrush(Colors.Black);

        /// <summary>
        /// Height of messages in <see cref="ListBox"/>
        /// <para>This <see langword="value"/> is <b>22</b> by default.</para>
        /// </summary>
        public double MessagesHeight { get; init; } = 22;

        /// <summary>
        /// Write <see cref="string"/> with a line terminator.
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            PrintMessage(message, true);
        }

        /// <summary>
        /// Indicates, whether <see cref="ListBox"/> is empty or not
        /// </summary>
        /// <returns></returns>
        public bool HasItems => !Form.TraceListBox.Items.IsEmpty;

        /// <summary>
        /// This method let you print message in <see cref="ListBox"/>. Write and WriteLine functions call this method to print message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="terminate"></param>
        protected void PrintMessage(string message, bool terminate)
        {
            if (string.IsNullOrEmpty(NestedMessagesBeginning)) throw new AggregateException("Beginning of nested messages cannot be null or empty");

            if (message == null) throw new ArgumentNullException(nameof(message), "Message cannot be null.");

            bool isbold = message.StartsWith(BoldMessagesBeginning);
            if (isbold) message = message[BoldMessagesBeginning.Length..];

            bool isnested = message.StartsWith(NestedMessagesBeginning);
            if (isnested) message = message[NestedMessagesBeginning.Length..];

            if (Form.TraceListBox.Items.Count > 0 && !((MessageInfo)((ListBoxItem)Form.TraceListBox.Items[^1]).Tag).Terminated)
            {
                ((ContentControl)((ListBoxItem)Form.TraceListBox.Items[^1]).Content).Content += message;
                ((MessageInfo)((ListBoxItem)Form.TraceListBox.Items[^1]).Tag).Terminated = true;
            }
            else
            {
                Form.TraceListBox.Items.Add(new ListBoxItem()
                {
                    Foreground = isnested ? NestedMessagesColor : MessagesColor,
                    Content = new ContentControl()
                    {
                        Margin = isnested ? NestedMessagesOffset : MessagesOffset,
                        FontWeight = isbold ? FontWeights.SemiBold : FontWeights.Normal,
                        Content = $"> {message}"
                    },
                    Height = MessagesHeight,
                    Tag = new MessageInfo()
                    {
                        IsNested = isnested,
                        Terminated = terminate
                    }
                });
            }
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
        /// <param name="message"></param>
        /// <param name="Index"></param>
        public override void Write(string message)
        {
            PrintMessage(message, false);
        }

        /// <summary>
        /// Clear all trace messages from <see cref="ListBox"/>.
        /// </summary>
        public void Clear() => Form.TraceListBox.Items.Clear();

        public override bool IsThreadSafe => false;

        /// <summary>
        /// Show trace <see cref="Window"/>
        /// </summary>
        public void Show()
        {
            this.Form.Show();
            this.Form.Focus();
        }
        
        /// <summary>
        /// Unload class resources (such as <see cref="Window"/>, etc).
        /// </summary>
        public new void Dispose() => Form.Close();

        /// <summary>
        /// This <see langword="record"/> contains information about message in <see cref="ListBox"/>.
        /// </summary>
        public record MessageInfo
        {
            /// <summary>
            /// This property is indicating whether the message is nested or not
            /// </summary>
            public bool IsNested { get; init; }

            /// <summary>
            /// This property is indicating whether the message is terminated or not
            /// </summary>
            public bool Terminated { get; set; }
        }
    }
}
