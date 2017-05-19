using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using Schneider.ManagementSystem.BAL.DepartmentRepository;
using Schneider.ManagementSystem.DAL;
using Schneider.ManagementSystem.Shared;

namespace Schneider.ManagementSystem.BAL.EmployeeRepository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        #region Constructor

        public EmployeeRepository()
        {
            Init();
        }
        #endregion

        #region Public Propertied
        public List<Employee> Employees { get; set; }
        public List<Employee> SearchEmployees { get; set; }
        public Employee Entity { get; set; }
        public ModelStateDictionary Messages { get; set; }
        public string PageMode { get; set; }
        public bool IsValid { get; set; }
        #endregion

        #region Init Method

        public void Init()
        {
            Employees = new List<Employee>();
            Entity = new Employee();
            SearchEmployees = new List<Employee>();
            Messages = new ModelStateDictionary();
            PageMode = PageConstants.List;
            IsValid = true;
        }


        #endregion

        #region Get Methods 

        public void Get()
        {
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                Employees = _database.Employees.OrderBy(d => d.Name).ToList();
            }
        }
        public void Get(int employeeId)
        {
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                Entity = _database.Employees.Find(employeeId);
            }
        }
        #endregion

        #region Save Method 

        public void Save()
        {
            Messages.Clear();
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                try
                {
                    SaveChangesToDataBase(_database);
                    //Get All Changes
                    Get();
                }
                catch (DbEntityValidationException exception)
                {
                    CatchErrorToDispaly(exception);
                }
            }
        }

        private void CatchErrorToDispaly(DbEntityValidationException exception)
        {
            IsValid = false;
            //Validation Errors
            foreach (var errors in exception.EntityValidationErrors)
            {
                foreach (var item in errors.ValidationErrors)
                {
                  //    Messages.AddError(item.PropertyName, item.ErrorMessage);
                }
            }
        }

        private void SaveChangesToDataBase(SchneiderSqlModel _database)
        {
            //Check if it update of insert
            if (PageMode == PageConstants.Edit)
            {
            //    _database.Entry(Entity).State = EntityState.Modified;
            //    _database.SaveChanges();
            }
            else if (PageMode == PageConstants.Add)
            {
                //_database.Employees.Add(Entity);
             //   _database.SaveChanges();
            }
        }

        #endregion

        #region Delete Method

        public void Delete(int employeeId)
        {
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                var employee = _database.Employees.Find(employeeId);
                if (employee != null)
                    _database.Employees.Remove(employee);
                _database.SaveChanges();
                Get();
            }
        }

        #endregion

    }
}
