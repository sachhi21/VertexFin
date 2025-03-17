

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedRepository.IRepository;
using VertexFin.Domain.Data;
using VertexFin.Domain.DTOModels;
using VertexFin.Domain.Models;


using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;

namespace PortRec.RepositoryLayer.Repository
{
    public class Repository : IRepository
    {
        #region property
        private readonly ApplicationDbContext _dbContext;
        #endregion

        #region Constructor
        public Repository(ApplicationDbContext dbContext)
        {
         
            _dbContext = dbContext;
        }
        #endregion

        public async Task Delete<T>(T entity) where T : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Set<T>().Remove(entity);
            await SaveChanges();
        }

        public async Task<Tout?> Get<T, Tout>(Guid Id) where Tout : BaseEntity
        {
            if (Id == null)
            {
                throw new ArgumentNullException(nameof(Id));
            }
            return await _dbContext.Set<Tout>().SingleOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : BaseEntity
        {
            return await Task.FromResult(_dbContext.Set<T>().AsEnumerable());
        }

        public async Task Insert<T>(T entity) where T : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _dbContext.Set<T>().AddAsync(entity);
            await SaveChanges();
        }

        public async Task Remove<T>(T entity) where T : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Set<T>().Remove(entity);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update<T>(T entity) where T : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Set<T>().Update(entity);
            await SaveChanges();
        }

        public async Task<IEnumerable<T>> Search<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> ExecWithStoreProcedure<T>(string query, params object[] parameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            return await Task.FromResult(_dbContext.Database.SqlQueryRaw<T>(query, parameters)); ;
        }

        public async Task<List<T>> ExecuteReaderWithSingleResult<T>(string spName, params object[] parameters)
        {
            if (spName == null)
            {
                throw new ArgumentNullException(nameof(spName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            List<T> result = new List<T>();

            using (SqlConnection sqlConnection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync();
                }

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = spName;

                    if (parameters != null && parameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        result = await DbReaderToObject<T>(sqlDataReader) ?? new List<T>();
                    }
                }
            }

            return result;
        }

        public async Task<ResultSets> ExecuteReader(string spName, params object[] parameters)
        {
            if (spName == null)
            {
                throw new ArgumentNullException(nameof(spName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            ResultSets resultSets;

            using (SqlConnection sqlConnection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync();
                }

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = spName;

                    if (parameters != null && parameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        resultSets = await GetData(sqlDataReader);
                    }
                }
            }

            return resultSets;
        }

        public async Task<T?> ExecuteScalar<T>(string spName, params object[] parameters)
        {
            if (spName == null)
            {
                throw new ArgumentNullException(nameof(spName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            T? result = default;

            using (SqlConnection sqlConnection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync();
                }

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = spName;

                    if (parameters != null && parameters.Length > 0)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }

                    var executeScalar = await sqlCommand.ExecuteScalarAsync();
                    if (executeScalar != null && executeScalar != DBNull.Value)
                    {
                        result = (T)Convert.ChangeType(executeScalar, typeof(T));
                    }
                }
            }

            return result;
        }

        public async Task<int> ExecuteNonQuery(string query, params object[] parameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return await _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public DbSet<T> GetDbSet<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>();
        }

        
        private async Task<Dictionary<int, PropertyInfo>> MapColumns<T>(DbDataReader rd)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<int, PropertyInfo> keyValuePairs = new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                if (property.CanWrite)
                {
                    keyValuePairs.Add(rd.GetOrdinal(property.Name), property);
                }
            }
            return await Task.FromResult(keyValuePairs);
        }

        private async Task<List<T>> DbReaderToObject<T>(DbDataReader rd)
        {
            List<T> result = new List<T>();
            if (rd.FieldCount > 0 && rd.HasRows)
            {
                while (await rd.ReadAsync())
                {
                    Dictionary<string, object?> keyValuePairs = new Dictionary<string, object?>();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        keyValuePairs.Add(rd.GetName(i), rd.GetValue(i));
                    }
                    T? obj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(keyValuePairs));
                    if (obj != null)
                    {
                        result.Add(obj);
                    }
                }
            }

            return await Task.FromResult(result);
        }

        private async Task<ResultSets> GetData(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            List<ResultSet> resultSets = new List<ResultSet>();
            do
            {
                List<ResultSetRow> resultSetRows = new List<ResultSetRow>();
                while (await reader.ReadAsync())
                {
                    Dictionary<string, object?> columns = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columns.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    resultSetRows.Add(new ResultSetRow(columns));
                }
                resultSets.Add(new ResultSet(resultSetRows));
            } while (await reader.NextResultAsync());

            return new ResultSets(resultSets);
        }

        //public async Task<Tout?> Get<T, Tout>(Guid Id) where Tout : BaseEntity
        //{
        //    if (Id == null)
        //    {
        //        throw new ArgumentNullException(nameof(Id));
        //    }
        //    return await _dbContext.Set<Tout>().SingleOrDefaultAsync(c => c.Id == Id);
        //}
    }
}
