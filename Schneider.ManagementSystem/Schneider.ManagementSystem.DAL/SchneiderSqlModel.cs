using Schneider.ManagementSystem.Shared;

namespace Schneider.ManagementSystem.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SchneiderSqlModel : DbContext
    {

        public SchneiderSqlModel()
            : base("name=SchmeiderSqlModel")
        {
        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
    }
}