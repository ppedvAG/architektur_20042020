﻿using ppedv.TransportVisu.Model;
using ppedv.TransportVisu.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ppedv.TransportVisu.Data.EF
{
    public class EfRepository : IRepository
    {
        EfContext context = new EfContext();

        public void Add<T>(T entity) where T : Entity
        {
            if (typeof(T) == typeof(Waschmaschine))
                context.Waschmaschinen.Add(entity as Waschmaschine);

            context.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : Entity
        {
            context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll<T>() where T : Entity
        {
            return context.Set<T>().ToList();
        }

        public T GetById<T>(int id) where T : Entity
        {
            return context.Set<T>().Find(id);
        }

        public void SaveAll()
        {
            context.SaveChanges();
        }

        public void Update<T>(T entity) where T : Entity
        {
            var loaded = GetById<T>(entity.Id);
            if (loaded != null)
                context.Entry(loaded).CurrentValues.SetValues(entity);
        }
    }
}
