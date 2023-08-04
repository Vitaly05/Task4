namespace Task4.Repositories
{
    public interface IRepository<T>
    {
        void Create(T item);

        void Delete(T item);

        List<T> GetAll();
    }
}
