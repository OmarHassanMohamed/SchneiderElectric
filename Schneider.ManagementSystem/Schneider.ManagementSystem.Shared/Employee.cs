using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schneider.ManagementSystem.Shared
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Employee Name")]
        [MaxLength(200)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [DisplayName("E-Mail")]
        [MaxLength(200)]
        [Required(ErrorMessage = "Email is required")]
        public string Mail { get; set; }
        public Department Department { get; set; }
    }
}
