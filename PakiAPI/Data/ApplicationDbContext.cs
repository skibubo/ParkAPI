using Microsoft.EntityFrameworkCore;
using PakiAPI.Models;
//using System.Data.Entity;

namespace PakiAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}
 