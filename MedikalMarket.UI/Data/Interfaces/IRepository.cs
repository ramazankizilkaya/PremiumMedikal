using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        #region Create
        bool CreateEntity(T entity);
        bool CreateEntities(IEnumerable<T> entities);
        #endregion

        #region Read
        IEnumerable<T> GetAllEntities();
        IEnumerable<T> GetAllEntitiesNoTrack();
        IEnumerable<T> GetAllEntitiesWithFullJoin();

        T GetEntityById(int id);
        T GetEntityByIdWithFullJoin(int id);

        T FindEntity(Func<T, bool> predicate);
        IEnumerable<T> FindEntities(Func<T, bool> predicate);
        IEnumerable<T> FindEntitiesNoTrack(Func<T, bool> predicate);
        IEnumerable<T> FindEntitiesWithFullJoin(Func<T, bool> predicate);
        #endregion

        #region Update
        bool UpdateEntity(T entity);
        bool UpdateEntities(IEnumerable<T> entities);
        #endregion

        #region Delete
        bool SoftDeleteEntity(T entity);
        bool SoftDeleteGivenEntities(IEnumerable<T> entities);
        bool HardDeleteEntity(T entity);
        bool HardDeleteGivenEntities(IEnumerable<T> entities);
        #endregion

        #region Others
        int CountEntity(Func<T, bool> predicate);
        bool AnyEntity(Func<T, bool> predicate);
        bool AnyEntity();
        List<int> GetIdList();
        #endregion
    }
}
