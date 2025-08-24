using System.Collections.Generic;
using System.Linq;
using DeskBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Services;

public class CrudService<T> where T : class
{
    public List<T> GetAll()
    {
        using var db = new AppDbContext();
        return db.Set<T>().ToList();
    }

    public T? GetById(int id)
    {
        using var db = new AppDbContext();
        return db.Set<T>().Find(id);
    }

    public void Add(T entity)
    {
        using var db = new AppDbContext();
        db.Set<T>().Add(entity);
        db.SaveChanges();
    }

    public void Update(T entity)
    {
        using var db = new AppDbContext();
        db.Set<T>().Update(entity);
        db.SaveChanges();
    }

    public void Delete(int id)
    {
        using var db = new AppDbContext();
        var entity = db.Set<T>().Find(id);
        if (entity != null)
        {
            db.Set<T>().Remove(entity);
            db.SaveChanges();
        }
    }
}
