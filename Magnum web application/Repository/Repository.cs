using Magnum_web_application.Data;
using Magnum_web_application.Models;
using Magnum_web_application.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Magnum_web_application.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
        {
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await SaveAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await SaveAsync();
		}

		public async Task <List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeproperties = null)
		{
			IQueryable<T> query = _dbSet;
			if(includeproperties != null)
			{
				foreach(var property in includeproperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}

			if(filter != null)
			{
				query = query.Where(filter);
			}

			return await query.ToListAsync();
		}

		public async Task <T> GetByIdAsync(Expression<Func<T, bool>> filter = null, string includeproperties = null, bool tracked = true)
		{
			IQueryable<T> query = _dbSet;

			if(tracked == false)
			{
				query = query.AsNoTracking();
			}

			if (includeproperties != null)
			{
				foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
