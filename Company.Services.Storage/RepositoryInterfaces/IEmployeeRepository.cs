using Company.Services.Components.Models;
using System.Collections.Generic;

namespace Company.Services.Storage
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees();
        Employee GetEmployeeById(string employeeId);

        Employee AddEmployee(Employee employee);

        Employee UpdateEmployee(Employee employee);

        bool DeleteEmployee(string employeeId);

    }
}
