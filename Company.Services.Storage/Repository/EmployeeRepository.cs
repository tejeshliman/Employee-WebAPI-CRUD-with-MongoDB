using Company.Services.Components.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Company.Services.Storage
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private MongoContext _mongoContext;

        public EmployeeRepository(MongoContext employeedbContext)
        {
            _mongoContext = employeedbContext;
        }

        public Employee AddEmployee(Employee employee)
        {
            var document = _mongoContext._database.GetCollection<BsonDocument>("employee");
            var query = Query.And(Query.EQ("FirstName", employee.FirstName), Query.EQ("LastName", employee.LastName));
            var count = document.FindAs<Employee>(query).Count();
            if (count == 0)
            {
                var sortBy = SortBy.Descending("EmployeeID");
                Employee lastEmployee = _mongoContext._database.GetCollection<Employee>("employee").FindAll().SetSortOrder(sortBy).SetLimit(1).Single();
                int lastempid;
                int.TryParse(lastEmployee.EmployeeID, out lastempid);
                employee.EmployeeID = (lastempid + 1).ToString();
                var result = document.Insert(employee);
            }
            return GetEmployeeById(employee.EmployeeID);
        }

        public bool DeleteEmployee(string empId)
        {
            var empToBeDelete = Query<Employee>.Where(x => x.EmployeeID == empId);
            var result = _mongoContext._database.GetCollection<Employee>("employee").Remove(empToBeDelete, RemoveFlags.Single);
            return true;
        }

        public Employee GetEmployeeById(string empId)
        {
            var query = Query<Employee>.Where(x => x.EmployeeID == empId);
            return _mongoContext._database.GetCollection<Employee>("employee").FindOne(query);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            var employees = _mongoContext._database.GetCollection<Employee>("employee").FindAll().ToList();
            return employees;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var empObjectId = Query<Employee>.EQ(p => p.EmployeeID, employee.EmployeeID);
            var collection = _mongoContext._database.GetCollection<Employee>("employee");
            var result = collection.Update(empObjectId, Update.Replace(employee), UpdateFlags.None);
            return GetEmployeeById(employee.EmployeeID);
        }

    }
}
