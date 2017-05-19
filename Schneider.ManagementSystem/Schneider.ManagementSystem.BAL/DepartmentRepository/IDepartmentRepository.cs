using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schneider.ManagementSystem.Shared;

namespace Schneider.ManagementSystem.BAL.DepartmentRepository
{
    public interface IDepartmentRepository
    {
        void Delete(int departmentId);
        void Get();
        void Get(int departmentId);
        void Save();
    }
}
