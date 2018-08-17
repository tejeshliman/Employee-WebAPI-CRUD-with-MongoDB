using Company.Services.Components.Models;
using Company.Services.Storage;
using System.Collections.Generic;
using System.Web.Http;

namespace SampleWebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        IEmployeeRepository _employeeRepo;

        [Route("api/Employee/GetEmployees")]
        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            _employeeRepo = new EmployeeRepository(ModelFactory<MongoContext>.GetContext());
            return _employeeRepo.GetEmployees();
        }

        [Route("api/Employee/GetEmployeeById")]
        [HttpGet]
        public Employee GetEmployeeById(string empId)
        {
            _employeeRepo = new EmployeeRepository(ModelFactory<MongoContext>.GetContext());
            return _employeeRepo.GetEmployeeById(empId);
        }

        [Route("api/Employee/AddEmployee")]
        [HttpPost]
        public Employee AddEmployee(Employee employee)
        {
            _employeeRepo = new EmployeeRepository(ModelFactory<MongoContext>.GetContext());
            return _employeeRepo.AddEmployee(employee);
        }

        [Route("api/Employee/UpdateEmployee")]
        [HttpPut]
        public Employee UpdateEmployee([FromBody]Employee employee)
        {
            _employeeRepo = new EmployeeRepository(ModelFactory<MongoContext>.GetContext());
            return _employeeRepo.UpdateEmployee(employee);
        }

        [Route("api/Employee/DeleteEmployee")]
        [HttpPost]
        public bool DeleteEmployee([FromBody]string id)
        {
            _employeeRepo = new EmployeeRepository(ModelFactory<MongoContext>.GetContext());
            return _employeeRepo.DeleteEmployee(id);
        }
    }
}
