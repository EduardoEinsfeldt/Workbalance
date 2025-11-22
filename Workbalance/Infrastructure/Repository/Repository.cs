using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Workbalance.Infrastructure.Context;

namespace Workbalance.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(object id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();


        // ============================================
        //         EXECUTAR PROCEDURE ORACLE
        // ============================================
        public async Task ExecutarProcedureAsync(string procedureName, Dictionary<string, object> parametros)
        {
            // Usa a MESMA conexão do Entity Framework
            var conn = (OracleConnection)_context.Database.GetDbConnection();

            bool wasClosed = conn.State == ConnectionState.Closed;
            if (wasClosed)
                await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            // Adiciona parâmetros (IN e OUT)
            foreach (var param in parametros)
            {
                OracleParameter oracleParam;

                if (param.Value == null) // OUT parameter
                {
                    oracleParam = new OracleParameter(param.Key, OracleDbType.Varchar2, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                }
                else
                {
                    oracleParam = new OracleParameter(param.Key, param.Value)
                    {
                        Direction = ParameterDirection.Input
                    };
                }

                cmd.Parameters.Add(oracleParam);
            }

            // Executa
            await cmd.ExecuteNonQueryAsync();

            // Copia valores OUT de volta ao dicionário
            foreach (OracleParameter p in cmd.Parameters)
            {
                if (p.Direction == ParameterDirection.Output)
                    parametros[p.ParameterName] = p.Value?.ToString() ?? "";
            }

            if (wasClosed)
                await conn.CloseAsync();
        }


        // ============================================
        //         EXECUTAR FUNÇÃO ORACLE
        // ============================================
        public async Task<TResult?> ExecutarFunctionAsync<TResult>(string functionName, Dictionary<string, object>? parametros = null)
        {
            var conn = (OracleConnection)_context.Database.GetDbConnection();

            bool wasClosed = conn.State == ConnectionState.Closed;
            if (wasClosed)
                await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = functionName;

            // RETURN
            cmd.Parameters.Add("RETURN_VALUE", OracleDbType.Varchar2, 4000)
                .Direction = ParameterDirection.ReturnValue;

            if (parametros != null)
            {
                foreach (var param in parametros)
                {
                    cmd.Parameters.Add(new OracleParameter(param.Key, param.Value)
                    {
                        Direction = ParameterDirection.Input
                    });
                }
            }

            await cmd.ExecuteNonQueryAsync();

            var raw = cmd.Parameters["RETURN_VALUE"].Value;

            if (wasClosed)
                await conn.CloseAsync();

            if (raw == DBNull.Value)
                return default;

            return (TResult?)Convert.ChangeType(raw, typeof(TResult));
        }
    }
}
