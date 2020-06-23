using Small.Net.Graphic.Core;
using System;
using System.Runtime.CompilerServices;
using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
#pragma warning disable CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
#pragma warning disable CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
    public unsafe ref struct ComPtr<T> where T : unmanaged
#pragma warning restore CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
#pragma warning restore CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
    {
        private IntPtr _ptr;

        public ComPtr(T* ptr)
        {
            _ptr = (IntPtr)ptr;
        }

        public ComPtr(IntPtr ptr)
        {
            _ptr = ptr;
        }

        public static implicit operator ComPtr<T>(T* value) => new ComPtr<T>(value);
        public static implicit operator ComPtr<T>(ComPtrField<T> value) => new ComPtr<T>(value.Detach());
        public static implicit operator ComPtr<IUnknown>(ComPtr<T> value) => new ComPtr<IUnknown>((IUnknown*)value._ptr);
        public static implicit operator IUnknown*(ComPtr<T> value) => (IUnknown*)value._ptr;

        public static bool operator ==(ComPtr<T> left, ComPtr<T> right) => left.Ptr == right.Ptr;
        public static bool operator !=(ComPtr<T> left, ComPtr<T> right) => !(left == right);

        public T* Detach()
        {
            IntPtr temp = _ptr;
            _ptr = (IntPtr)null;
            return (T*)temp;
        }

        public T* Ptr => (T*)_ptr;

        public T** GetAddressOf() => (T**)Unsafe.AsPointer(ref _ptr);
        public T** ReleaseGetAddressOf()
        {
            if (_ptr != IntPtr.Zero)
            {
                ((IUnknown*)_ptr)->Release();
            }

            return GetAddressOf();
        }

        public ComPtr<TOutput> As<TOutput>(Guid iid) where TOutput : unmanaged
        {
            void* outPut;
            Result result = ((IUnknown*)_ptr)->QueryInterface(&iid, &outPut);
            result.CheckError();
            return new ComPtr<TOutput>((IntPtr)outPut);
        }

        public void Dispose()
        {
            IntPtr temp = _ptr;

            if (temp != (IntPtr)null)
            {
                _ptr = (IntPtr)null;

                ((IUnknown*)temp)->Release();
            }
        }
        public void Release() => Dispose();
    }
}
