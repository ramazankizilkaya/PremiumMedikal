using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseModel
    {
        protected readonly MedikalMarketContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly IErrorLogRepository _errorRepo;
        

        public Repository(MedikalMarketContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #region Others
        public bool AnyEntity(Func<T, bool> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public bool AnyEntity()
        {
            return _context.Set<T>().Any();
        }

        public int CountEntity(Func<T, bool> predicate)
        {
            return _context.Set<T>().Count(predicate);
        }

        public List<int> GetIdList()
        {
            List<int> idList = new List<int>();
            foreach (var item in _dbSet)
            {
                idList.Add(item.Id);
            }
            return idList;
        }
        #endregion

        #region Create
        public bool CreateEntities(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = false;
                entity.CreatedDate = DateTime.Now;
                _context.Add(entity);
            }
            return Save();
        }

        public bool CreateEntity(T entity)
        {
            entity.IsDeleted = false;
            entity.CreatedDate = DateTime.Now;
            _context.Add(entity);
            return Save();
        }
        #endregion

        #region Read

        public IEnumerable<T> FindEntities(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T FindEntity(Func<T, bool> predicate)
        {
            return _context.Set<T>().First(predicate);
        }

        public IEnumerable<T> FindEntitiesNoTrack(Func<T, bool> predicate)
        {
            return _context.Set<T>().AsNoTracking().Where(predicate);
        }

        public IEnumerable<T> FindEntitiesWithFullJoin(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllEntities()
        {
            return _context.Set<T>();
        }

        public IEnumerable<T> GetAllEntitiesNoTrack()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IEnumerable<T> GetAllEntitiesWithFullJoin()
        {
            throw new NotImplementedException();
        }

        public T GetEntityById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T GetEntityByIdWithFullJoin(int id)
        {
            throw new NotImplementedException();
        }
        #endregion



        #region Delete

        public bool HardDeleteEntity(T entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public bool HardDeleteGivenEntities(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Remove(entity);
            }
            return Save();
        }

        public bool SoftDeleteEntity(T entity)
        {
            entity.IsDeleted = true;
            _context.Update(entity);
            return Save();
        }

        public bool SoftDeleteGivenEntities(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                _context.Update(entity);
            }
            return Save();
        }
        #endregion


        #region Update
        public bool UpdateEntities(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedDate = DateTime.Now;
                _context.Entry(entity).State = EntityState.Modified;
            }
            return Save();
        }

        public bool UpdateEntity(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;
            //_context.Update(entity);
            return Save();
        }
        #endregion

        
        protected bool Save()
        {
            try
            {
                return _context.SaveChanges() > 0 ? true : false;
            }
            catch (Exception e)
            {
                _context.ErrorLogs.Add(new ErrorLog
                {
                    ErrorDetail = e.ToString(),
                    ErrorLocation = "Repository Save method"
                });
                _context.SaveChanges();
                return false;
            }
        }
    }
}
