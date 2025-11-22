namespace Workbalance.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();

        // Permite via procedure.
        Task ExecutarProcedureAsync(string procedureName, Dictionary<string, object> parametros);

        // Permite via function.
        Task<TResult?> ExecutarFunctionAsync<TResult>(string functionName, Dictionary<string, object>? parametros = null);
    }
}
