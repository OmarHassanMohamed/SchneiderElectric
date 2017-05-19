using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Schneider.ManagementSystem.BAL.DepartmentRepository;
using Schneider.ManagementSystem.BAL.EmployeeRepository;
using Schneider.ManagementSystem.Shared;

namespace Schneider.ManagementSystem.SVC.Controllers
{
    public class EmployeeController : ApiController
    {
        
        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            EmployeeRepository vm = new EmployeeRepository();
            vm.Get();
            if (vm.Employees.Count > 0)
                result = Ok(vm.Employees);
            else
                result = NotFound();
            return result;
        }

        // GET api/<controller>/5
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            EmployeeRepository vm = new EmployeeRepository();
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
        public IHttpActionResult Post(Employee employee)
        {
            IHttpActionResult result = null;
            EmployeeRepository vm = new EmployeeRepository();
            vm.Entity = employee;
            vm.PageMode = PageConstants.Add;
            vm.Save();
            if (vm.IsValid)
            {
                result = Ok();
                //result = Created<Employee>(
                //      Request.RequestUri +
                //      employee.Id.ToString(),
                //      employee);

            }
            else
                result = NotFound();
            return result;
        }

        // PUT api/<controller>/5
        [HttpPut()]
        public IHttpActionResult Put(int id, [FromBody]Employee employee)
        {
            IHttpActionResult result = null;
            EmployeeRepository vm = new EmployeeRepository();
            vm.PageMode = PageConstants.Edit;
            vm.Entity = employee;
            vm.Save();
            if (vm.IsValid)
                // result = Ok(employee);
                result = Ok();
            else
                result = NotFound();
            return result;
        }

        // DELETE api/<controller>/5
        [HttpDelete()]
        public IHttpActionResult Delete(int id)
        {
            IHttpActionResult result = null;
            EmployeeRepository vm = new EmployeeRepository();
            vm.Get(id);
            if (vm.Entity.Id > 0)
            {
              //  vm.Delete(id);
                result = Ok(true);
            }
            else
                result = NotFound();
            return result;
        }
    }
}