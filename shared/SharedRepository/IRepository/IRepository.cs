using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using VertexFin.Domain.DTOModels;
using VertexFin.Domain.Models;

namespace SharedRepository.IRepository
{
public interface IRepository
{
        Task<IEnumerable<T>> GetAll<T>() where T : BaseEntity;
        Task<Tout?> Get<C, Tout>(Guid Id) where Tout : BaseEntity;
        Task Insert<T>(T entity) where T : BaseEntity;
        Task Update<T>(T entity) where T : BaseEntity;
        Task Delete<T>(T entity) where T : BaseEntity;
        Task Remove<T>(T entity) where T : BaseEntity;
        Task SaveChanges();
        //Task<IEnumerable<T>> Search<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;
        Task<IEnumerable<T>> ExecWithStoreProcedure<T>(string query, params object[] parameters);
        Task<List<T>> ExecuteReaderWithSingleResult<T>(string query, params object[] parameters);
        Task<ResultSets> ExecuteReader(string query, params object[] parameters);
        Task<T?> ExecuteScalar<T>(string spName, params object[] parameters);
        Task<int> ExecuteNonQuery(string query, params object[] parameters);
        DbSet<T> GetDbSet<T>() where T : BaseEntity;
    }
}