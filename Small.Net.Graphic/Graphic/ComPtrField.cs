using TerraFX.Interop;

namespace Small.Net.Graphic.Graphic
{
#pragma warning disable CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
#pragma warning disable CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
    public unsafe struct ComPtrField<T> where T : unmanaged
#pragma warning restore CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
#pragma warning restore CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
    {
        private T* _ptr;

        public ComPtrField(T* ptr)
        {
            _ptr = ptr;
        }

        public static implicit operator ComPtrField<T>(void* value) => new ComPtrField<T>((T*)value);

        public static implicit operator ComPtrField<T>(ComPtr<T> value) => new ComPtrField<T>(value.Detach());

        public static implicit operator ComPtrField<IUnknown>(ComPtrField<T> value) => new ComPtrField<IUnknown>((IUnknown*)value._ptr);

        public static implicit operator IUnknown*(ComPtrField<T> value) => (IUnknown*)value._ptr;

        public static bool operator ==(ComPtrField<T> left, ComPtrField<T> right) => left.Ptr == right.Ptr;

        public static bool operator !=(ComPtrField<T> left, ComPtrField<T> right) => !(left == right);

        public T* Detach()
        {
            T* temp = _ptr;
            _ptr = null;
            return temp;
        }

        public T* Ptr => _ptr;
        public ref T* GetPinnableReference()
        {
            fixed (T** p = &_ptr)
            {
                return ref *p;
            }
        }

        public ref T* ReleaseGetPinnableReference()
        {
            ((IUnknown*)_ptr)->Release();
            return ref GetPinnableReference();
        }

        public void Release() => Dispose();

        public void Dispose()
        {
            T* temp = _ptr;

            if (temp != null)
            {
                _ptr = null;

                ((IUnknown*)temp)->Release();
            }
        }
    }
}
