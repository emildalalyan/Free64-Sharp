using System.Windows.Forms;

namespace Free64
{
    /// <summary>
    /// Abstract <see langword="class"/>, represents all graphical <b>tools</b> in Free64
    /// </summary>
    public abstract class Free64Tool
    {
        /// <summary>
        /// Represents <see cref="Free64Tool"/> main form
        /// </summary>
        public abstract object MainForm { get; }

        /// <summary>
        /// Shows main form of this <see cref="Free64Tool"/>
        /// </summary>
        public abstract void Show();

        /// <summary>
        /// Relative path directory, where tools is storing <b>resources</b> 
        /// </summary>
        public const string ToolsResourcesLocation = "res";
    }
}