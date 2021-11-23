using Microsoft.EntityFrameworkCore;
using MicroService.Models;

namespace MicroService.DBContexts
{
    public class PersonneContext : DbContext
    {
        public DbSet<PersonneModel> Personnes { get; set; }

        public PersonneContext(DbContextOptions<PersonneContext> option) : base(option) {

      
                        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           

            base.OnModelCreating(modelBuilder);
        }



    }
}
