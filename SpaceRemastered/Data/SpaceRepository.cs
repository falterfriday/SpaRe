using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpaceRemastered.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpaceRemastered.Data
{
	public class SpaceRepository : ISpaceRepository
	{
		private readonly SpaceContext _ctx;

		public SpaceRepository(SpaceContext ctx)
		{
			_ctx = ctx;
		}

		public IEnumerable<PhotoEntity> GetAllPhotos()
		{
			return _ctx.Photos.OrderBy(d => d.DatePosted).ToList();
		}


		public async Task AddPhoto(PhotoEntity photo)
		{
			PhotoEntity test = await _ctx.Photos.FirstOrDefaultAsync(u => u.Url == photo.Url);

			if (test == null || Equals(test, default(PhotoEntity)))
			{
				_ctx.Photos.Add(photo);
				await _ctx.SaveChangesAsync();
			}
		}

		public void AddEntity(PhotoEntity model) => _ctx.AddAsync(model);

		public void SaveAll()
		{
			try
			{
				_ctx.SaveChangesAsync();
				return;
			}
			catch (Exception ex)
			{
				throw new Exception($"Something went wrong: {ex}");
			}
		}

		public IEnumerable<string> GetAllUrls()
		{
			var results = _ctx.Photos.Select(u => u.Url);
			return results;
		}


		public bool IsUrlPresent(string url)
		{
			var exists = _ctx.Photos.Where(u => u.Url == url);
			int count = exists.Count();

			return count != 0;
		}
	}

	public static class DbSetExtensions
	{
		public static EntityEntry<T> AddIfNotPresent<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
		{
			var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
			return !exists ? dbSet.Add(entity) : null;
		}
	}
}
