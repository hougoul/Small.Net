using System;

namespace WorldEditor.Core
{
    public interface IUnknown
    {
        /// <summary>
        /// Queries the supported COM interface on this instance.
        /// </summary>
        /// <param name="guid">The guid of the interface.</param>
        /// <param name="comObject">The output COM object reference.</param>
        /// <returns>If successful, <see cref="Result.Ok"/> </returns>
        Result QueryInterface(ref Guid guid, out IntPtr comObject);

        /// <summary>
        /// Increments the reference count for an interface on this instance.
        /// </summary>
        /// <returns>The method returns the new reference count.</returns>
        int AddReference();

        /// <summary>
        /// Decrements the reference count for an interface on this instance.
        /// </summary>
        /// <returns>The method returns the new reference count.</returns>
        int Release();
    }
}