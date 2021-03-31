using System.Data.Entity;

namespace VeraDemoNet.DataAccess
{
    public class BlabberDB : DbContext  
    {
        //const string DATA_SOURCE = "(localdb)\\MSSQLLocalDB";
        //const string DB_FILENAME = "|DataDirectory|VeraDemoNet.mdf";
        //const string LOGIN = "admin";
        //const string PASSWORD = "admin123";
        //const string OPTIONS = "Integrated Security=True;User Instance=False";
        // $"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|VeraDemoNet.mdf;User Id=admin;Password=admin123;Integrated Security=True;User Instance=False"
        public BlabberDB() : base("BlabberDB")  
        {  
        }  
  
        protected override void OnModelCreating(DbModelBuilder modelBuilder)  
        {  
            modelBuilder.Entity<User>().HasKey(x=>x.UserName);
        }


  
        public DbSet<User> Users { get; set; }  
    }  
}