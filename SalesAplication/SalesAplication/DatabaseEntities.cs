using SalesAplication.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;

namespace SalesAplication
{

    public partial class DatabaseEntities : DbContext
    {
        public DatabaseEntities()
            : base("name=DatabaseEntities")
          
        {
            this.Database.Log = x =>
            {
                using (StreamWriter sw = new StreamWriter(@"C:\Projetos\logs\logSQL.txt", true))
                {
                    sw.WriteLine(x);
                    sw.Close();
                }
            };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Classification> Classification { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<User> User { get; set; }


        public TEntity Get<TEntity>(params object[] ids)
        where TEntity : class
        {
            return this.Set<TEntity>().Find(ids);
        }

        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            return this.Set<TEntity>();
        }
    }
}
