using Online_Electronics_Shop.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Online_Electronics_Shop.Repository
{
    public class GenericRepository<Tbl_Entity> : IRepository<Tbl_Entity> where Tbl_Entity : class
    {
        DbSet<Tbl_Entity> _dbSet;

        private dbOnlineElectronicsShopEntities _DBEntity;
        public GenericRepository(dbOnlineElectronicsShopEntities DBEntity)
        {
            _DBEntity = DBEntity;
            _dbSet = _DBEntity.Set<Tbl_Entity>();


        }


        public IEnumerable<Tbl_Entity> GetProduct()
        {

            return _dbSet.ToList();

        }
        public void Delete(Tbl_Entity entity)
        {

            _dbSet.Attach(entity);
            _DBEntity.Entry(entity).State = EntityState.Modified;

            _dbSet.Remove(entity);
            _DBEntity.SaveChanges();




        }


       
        public void Add(Tbl_Entity entity)
        {
            //throw new NotImplementedException();
            _dbSet.Add(entity);
            _DBEntity.SaveChanges();
        }

        public int GetAllrecordCount()
        {
            //throw new NotImplementedException();
            return _dbSet.Count();
        }

        public IEnumerable<Tbl_Entity> GetAllRecords()
        {
            // throw new NotImplementedException();
            return _dbSet.ToList();
        }

        public IQueryable<Tbl_Entity> GetAllRecordsIQueryable()
        {
            //throw new NotImplementedException();
            return _dbSet;
        }

        public Tbl_Entity GetFirstorDefault(int recordId)
        {
            //throw new NotImplementedException();
            return _dbSet.Find(recordId);

        }

        public Tbl_Entity GetFirstorDefaultByParameter(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            // throw new NotImplementedException();
            return _dbSet.Where(wherePredict).FirstOrDefault();

        }

        public IEnumerable<Tbl_Entity> GetListParameter(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            //throw new NotImplementedException();
            return _dbSet.Where(wherePredict).ToList();

        }

        public IEnumerable<Tbl_Entity> GetRecordsToShow(int PageNo, int PageSize, int CurrentPage, Expression<Func<Tbl_Entity, bool>> wherePredict, Expression<Func<Tbl_Entity, int>> orderByPredict)
        {
            //throw new NotImplementedException();



            if (wherePredict != null)
            {
                return _dbSet.OrderBy(orderByPredict).Where(wherePredict).ToList();

            }
            else
            {
                return _dbSet.OrderBy(orderByPredict).ToList();

            }


        }

        public IEnumerable<Tbl_Entity> GetResultBySqlprocedure(string query, params object[] parameters)
        {
            //throw new NotImplementedException();
            if (parameters != null)
            {

                return _DBEntity.Database.SqlQuery<Tbl_Entity>(query, parameters).ToList();
            }
            else
                return _DBEntity.Database.SqlQuery<Tbl_Entity>(query).ToList();

        }

        public void InactiveAndDeleteMarkByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> ForEachPredict)
        {
            //throw new NotImplementedException();
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);

        }

        public void Remove(Tbl_Entity entity)
        {
            //throw new NotImplementedException();
            if (_DBEntity.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);




            }
        }

        public void RemoveByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            //throw new NotImplementedException();
            Tbl_Entity entity = _dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);

        }

        public void RemoveRangeByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            //throw new NotImplementedException();
            List<Tbl_Entity> entity = _dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {

                Remove(ent);

            }
        }

        public void Update(Tbl_Entity entity)
        {
            //throw new NotImplementedException();
            _dbSet.Attach(entity);
            _DBEntity.Entry(entity).State = EntityState.Modified;
            _DBEntity.SaveChanges();

        }

        public void UpdateByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> ForEachPredict)
        {
            // throw new NotImplementedException();
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);

        }

    }
}