namespace _02.Scirpts
{
    /// <summary>
    /// 팩토리 인터페이스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFactory<T>
    {
        public T Create();
    }
}