namespace DBComm.Repository;

public interface IBaseRepository
{
    Task<Object?> getOne(Object element);
    Task<Object?> get(Object element);
    Task<Object?> create(Object element);
    Task<Object?> update(Object element);
    Task<Object?> delete(Object element);
}