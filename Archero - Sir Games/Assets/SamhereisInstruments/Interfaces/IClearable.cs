namespace Interfaces
{
    public interface IClearable
    {
        public void Clear();
    }

    public interface IClearable<T>
    {
        public void Clear(T type);
    }
}