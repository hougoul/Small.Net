using System;
using System.Collections.Generic;

namespace WorldEditor.Core.Diagnostics
{
    // <summary>
    /// Provides <see cref="IEqualityComparer{T}"/> for default value types.
    /// </summary>
    internal static class EqualityComparer
    {
        /// <summary>
        /// A default <see cref="IEqualityComparer{T}"/> for <see cref="System.IntPtr"/>.
        /// </summary>
        public static readonly IEqualityComparer<IntPtr> DefaultIntPtr = new IntPtrComparer();

        internal class IntPtrComparer : EqualityComparer<IntPtr>
        {
            public override bool Equals(IntPtr x, IntPtr y)
            {
                return x == y;
            }

            public override int GetHashCode(IntPtr obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}