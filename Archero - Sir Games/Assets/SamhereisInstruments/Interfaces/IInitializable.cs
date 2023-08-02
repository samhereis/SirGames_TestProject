namespace Interfaces
{
    public interface IInitializable
    {
        public void Initialize();
    }

    public interface IInitializable<T>
    {
        public void Initialize(T type);
    }
}