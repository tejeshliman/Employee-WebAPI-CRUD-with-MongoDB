using System.Linq;

namespace Company.Services.Storage
{
    public class UserRepository 
    {
        private EmployeeDBContext _employeeModel;
        public UserRepository(EmployeeDBContext employeeModel)
        {
            this._employeeModel = employeeModel;
        }
        
        public bool ValidateUser(string username, string password)
        {
            var user = _employeeModel.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user != null;
        }
    }
}
