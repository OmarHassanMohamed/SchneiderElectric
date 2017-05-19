using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schneider.ManagementSystem.Shared
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Employee Name")]
        [MaxLength(200)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        public virtual IQueryable<Employee>Employees{ get; set; }

    }
}
