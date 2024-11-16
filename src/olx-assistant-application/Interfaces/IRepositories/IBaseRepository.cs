namespace olx_assistant_application.Interfaces.IRepositories;
public interface IBaseRepository<T>
{
    public void Create(T obj);
    public void Update(T obj);

}
