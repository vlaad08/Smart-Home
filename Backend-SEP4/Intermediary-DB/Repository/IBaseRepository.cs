namespace DBComm.Repository;

public interface IBaseRepository
{
    Task getOne<T>(T element);
    Task get<T>(T element);
    Task create<T>(T element);
    Task update<T>(T element);
    Task delete<T>(T element);


}