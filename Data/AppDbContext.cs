using Microsoft.EntityFrameworkCore;
using CsharpWeather.Models;

namespace CsharpWeather.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<SearchHistory> SearchHistories { get; set; }
    }
}
