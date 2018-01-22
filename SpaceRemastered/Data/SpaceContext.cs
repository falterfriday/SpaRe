using Microsoft.EntityFrameworkCore;
using SpaceRemastered.Data.Entities;

namespace SpaceRemastered.Data
{
	public class SpaceContext : DbContext
	{
		public SpaceContext(DbContextOptions<SpaceContext> options) : base(options) { }

		public DbSet<PhotoEntity> Photos { get; set; }

	}
}
