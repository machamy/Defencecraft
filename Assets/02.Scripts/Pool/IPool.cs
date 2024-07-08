namespace _02.Scirpts.Pool
{
    public interface IPool<T>
    {
        void init(int n);
        T Get();
        void Return(T e);
    }
}