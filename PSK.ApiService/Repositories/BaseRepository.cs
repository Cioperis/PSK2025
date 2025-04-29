using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;

namespace PSK.ApiService.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    protected BaseRepository(AppDbContext db)
    {
        _context = db;
    }
    
    public T GetById<TId>(TId id)
    {
        return _context.Set<T>().Find(id);
    }
    
    public async Task<T?> GetByIdAsync<TId>(TId id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public IEnumerable<T> GetWhere(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression).ToList();
    }
    
    public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().Where(expression).ToListAsync();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
    
    public void BulkUpdate(IEnumerable<T> entity)
    {
        _context.Set<T>().UpdateRange(entity);
    }
    
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}