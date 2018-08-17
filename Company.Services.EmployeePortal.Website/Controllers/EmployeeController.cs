using Company.Services.EmployeePortal.Website.Models;
using Company.Services.Components.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly INotifier notifier;
        string EmployeePortalApiurl = "http://localhost/EmployeeAPI/";

        public EmployeeController(INotifier notifier)
        {
            this.notifier = notifier;
        }

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            IEnumerable<Employee> employeelist = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(EmployeePortalApiurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Response = await client.GetAsync("api/Employee/GetEmployees");

                if (Response.IsSuccessStatusCode)
                {
                    var ResultSet = Response.Content.ReadAsStringAsync().Result;
                    employeelist = JsonConvert.DeserializeObject<List<Employee>>(ResultSet);
                }
                if (employeelist == null)
                {
                    return HttpNotFound();
                }
                return View(employeelist);
            }
        }

        // GET: Employee/Details
        public async Task<ActionResult> Details(string empId)
        {
            Employee employee = await GetEmployeeByIDAsync(empId);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmployeeId,FirstName,LastName,PrimaryEmail,PhoneNumber,IsActive")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(EmployeePortalApiurl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = CreateRequest(HttpMethod.Post, new System.Uri(EmployeePortalApiurl + "api/Employee/AddEmployee"), employee);
                    string content = JsonConvert.SerializeObject(employee);
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage Response = await client.SendAsync(request);

                    if (Response.IsSuccessStatusCode)
                    {
                        var ResultSet = Response.Content.ReadAsStringAsync().Result;
                        employee = JsonConvert.DeserializeObject<Employee>(ResultSet);
                        notifier.Success("Employee Saved Sucessfully..");
                    }
                    else
                    { 
                    notifier.Error("There was a problem while Employee Saving..");
                    }
                    return View(employee);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(string empId)
        {
            Employee employee = await GetEmployeeByIDAsync(empId);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmployeeId,FirstName,LastName,PrimaryEmail,PhoneNumber,IsActive")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(EmployeePortalApiurl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = CreateRequest(HttpMethod.Put, new System.Uri(EmployeePortalApiurl + "api/Employee/UpdateEmployee"), employee);
                    string content = JsonConvert.SerializeObject(employee);
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage Response = await client.SendAsync(request);

                    if (Response.IsSuccessStatusCode)
                    {
                        var ResultSet = Response.Content.ReadAsStringAsync().Result;
                        employee = JsonConvert.DeserializeObject<Employee>(ResultSet);
                        notifier.Success("Employee updated Sucessfully..");
                    }
                    else
                        notifier.Error("There was a problem while Employee update..");
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    return View(employee);
                }
            }
            return View(employee);
        }


        // GET: Employee/Delete/5
        public async Task<ActionResult> Delete(string empId)
        {
            Employee employee = await GetEmployeeByIDAsync(empId);
            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(FormCollection fcNotUsed, string empId)
        {
            bool EmployeeDeleted = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(EmployeePortalApiurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = CreateRequest(HttpMethod.Post, new System.Uri(EmployeePortalApiurl + "api/Employee/DeleteEmployee"), empId);
                string content = JsonConvert.SerializeObject(empId);
                request.Content = new StringContent(content);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage Response = await client.SendAsync(request);

                if (Response.IsSuccessStatusCode)
                {
                    var ResultSet = Response.Content.ReadAsStringAsync().Result;
                    EmployeeDeleted = JsonConvert.DeserializeObject<bool>(ResultSet);
                    notifier.Success("Employee deleted Sucessfully..");
                }
                else
                    notifier.Error("There was a problem while Employee deletion..");
                if (EmployeeDeleted == false)
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Index");
        }

        private async Task<Employee> GetEmployeeByIDAsync(string employeeId)
        {
            Employee employee = new Employee();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(EmployeePortalApiurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Response = await client.GetAsync("api/Employee/GetEmployeeById?empId=" + employeeId);

                if (Response.IsSuccessStatusCode)
                {
                    var ResultSet = Response.Content.ReadAsStringAsync().Result;
                    employee = JsonConvert.DeserializeObject<Employee>(ResultSet);
                }
                return employee;
            }
        }

        internal static HttpRequestMessage CreateRequest<TRequest>(HttpMethod verb, Uri uri, TRequest requestContent)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = verb
            };

            string content = JsonConvert.SerializeObject(requestContent);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
