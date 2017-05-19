using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using Schneider.ManagementSystem.DAL;
using Schneider.ManagementSystem.Shared;

namespace Schneider.ManagementSystem.BAL.DepartmentRepository
{
    public class DepartmentRepository:IDepartmentRepository
    {
        #region Constructor

        public DepartmentRepository()
        {
            Init();
        }
        #endregion

        #region Public Propertied
        public List<Department> Departments { get; set; }
        public List<Department> SearchDepartments { get; set; }
        public Department Entity { get; set; }
        public ModelStateDictionary Messages { get; set; }
        public string PageMode { get; set; }
        public bool IsValid { get; set; }
        #endregion

        #region Init Method

        public void Init()
        {
            Departments = new List<Department>();
            Entity = new Department();
            SearchDepartments = new List<Department>();
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
                Departments = _database.Departments.OrderBy(d => d.Name).ToList();
            }
        }
        public void Get(int departmentId)
        {
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                Entity = _database.Departments.Find(departmentId);
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
                  //  Messages.AddError(item.PropertyName, item.ErrorMessage);
                }
            }
        }

        private void SaveChangesToDataBase(SchneiderSqlModel _database)
        {
            //Check if it update of insert
            if (PageMode == PageConstants.Edit)
            {
                _database.Entry(Entity).State = EntityState.Modified;
                _database.SaveChanges();
            }
            else if (PageMode == PageConstants.Add)
            {
                _database.Departments.Add(Entity);
                _database.SaveChanges();
            }
        }

        #endregion

        #region Delete Method

        public void Delete(int departmentId)
        {
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                var department = _database.Departments.Find(departmentId);
                if (department != null)
                    _database.Departments.Remove(department);
                _database.SaveChanges();
                Get();
            }
        }

        #endregion

        #region Load Search Departments

        public void LoadSearchDepartments()
        {
            using (SchneiderSqlModel _database = new SchneiderSqlModel())
            {
                if (Departments.Count == 0)//load from DB
                    SearchDepartments.AddRange(_database.Departments);
                else // load from deparmtments list 
                    SearchDepartments.AddRange(Departments);
                //Add Search All Categories for Search All
                Department department = new Department();
                department.Id = 0;
                department.Name = "-- Search All Departments --";
                //Insert Department At th e top 
                SearchDepartments.Insert(0, department);
            }
        }

        #endregion
    }
}
