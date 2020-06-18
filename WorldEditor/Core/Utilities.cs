using System;

namespace WorldEditor.Core
{
    public static class Utilities
    {
        /// <summary>
        /// Gets the <see cref="System.Guid"/> from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The guid associated with this type.</returns>
        public static Guid GetGuidFromType(Type type)
        {
            return type.GUID;
        }

        /// <summary>
        /// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">A struct to evaluate.</typeparam>
        /// <returns>Size of this struct.</returns>
        public static int SizeOf<T>() where T : struct
        {
            return Interop.SizeOf<T>();
        }
    }
}