namespace Company.Services.Storage
{
    using Company.Services.Components.Models;
    using System;
    using System.Data.Entity;

    public partial class EmployeeDBContext : DbContext
    {
        
        Type providerService = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        public EmployeeDBContext()
            : base("name=EmployeeDBContext")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
