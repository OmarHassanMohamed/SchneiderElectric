using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Schneider.ManagementSystem.BAL.DepartmentRepository;
using Schneider.ManagementSystem.Shared;

namespace Schneider.ManagementSystem.SVC.Controllers
{
    public class DepartmentController : ApiController
    {
        [HttpGet()]
        [Route("api/Department/GetSearchDepartments")]
        public IHttpActionResult GetSearchDepartments()
        {
            IHttpActionResult result = null;
            DepartmentRepository vm = new DepartmentRepository();
            vm.LoadSearchDepartments();
            if (vm.SearchDepartments.Count > 0)
                result = Ok(vm.SearchDepartments);
            else
                result = NotFound();
            return result;
        }

        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            DepartmentRepository vm = new DepartmentRepository();
            vm.Get();
            if (vm.Departments.Count > 0)
                result = Ok(vm.Departments);
            else
                result = NotFound();
            return result;
        }

        // GET api/<controller>/5
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            DepartmentRepository vm = new DepartmentRepository();
            IHttpActionResult result;
            vm.Get(id);
            if (vm.Entity != null)
                result = Ok(vm.Entity);
            else
                result = NotFound();
            return result;
        }

        // POST api/<controller>
        [HttpPost()]
        public IHttpActionResult Post(Department department)
        {
            IHttpActionResult result = null;
            DepartmentRepository vm = new DepartmentRepository();
            vm.Entity = department;
            vm.PageMode = PageConstants.Add;
            vm.Save();
            if (vm.IsValid)
            {
                result = Created<Department>(
                    Request.RequestUri +
                    department.Id.ToString(),
                    department);
            }
            else
                result = NotFound();
            return result;
        }

        // PUT api/<controller>/5
        [HttpPut()]
        public IHttpActionResult Put(int id,Department department)
        {
            IHttpActionResult result = null;
            DepartmentRepository vm = new DepartmentRepository();
            vm.PageMode = PageConstants.Edit;
            vm.Entity = department;
            vm.Save();
            if (vm.IsValid)
                result = Ok(department);
            else
                result = NotFound();
            return result;
        }

        // DELETE api/<controller>/5
        [HttpDelete()]
        public IHttpActionResult Delete(int id)
        {
            IHttpActionResult result = null;
            DepartmentRepository vm = new DepartmentRepository();
            vm.Get(id);
            if (vm.Entity.Id > 0)
            {
                vm.Delete(id);
                result = Ok(true);
            }
            else
                result = NotFound();
            return result;
        }
    }
}