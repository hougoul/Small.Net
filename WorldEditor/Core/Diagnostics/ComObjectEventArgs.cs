using System;

namespace WorldEditor.Core.Diagnostics
{
    /// <summary>
    /// Event args for <see cref="ComObject"/> used by <see cref="ObjectTracker"/>.
    /// </summary>
    public class ComObjectEventArgs : EventArgs
    {
        /// <summary>
        /// The object being tracked/untracked.
        /// </summary>
        public ComObject Object;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComObjectEventArgs"/> class.
        /// </summary>
        /// <param name="o">The o.</param>
        public ComObjectEventArgs(ComObject o)
        {
            Object = o;
        }
    }
}