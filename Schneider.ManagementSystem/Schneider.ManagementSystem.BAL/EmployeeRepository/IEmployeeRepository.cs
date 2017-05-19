using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schneider.ManagementSystem.BAL.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        void Delete(int employeeId);
        void Get();
        void Get(int departmentId);
        void Save();
    }
}
