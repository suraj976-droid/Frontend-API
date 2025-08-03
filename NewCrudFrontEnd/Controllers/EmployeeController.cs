using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NewCrudFrontEnd.Models;
using System.Net.Http.Headers;
using System.Text;

namespace NewCrudFrontEnd.Controllers
{
    public class EmployeeController : Controller
    {
        HttpClient client;
        string baseUrl = "https://localhost:7040/api/Employee/";

        public EmployeeController()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
        }

        // GET: /Employee
        public IActionResult Index()
        {
            List<Employee> empList = new List<Employee>();
            HttpResponseMessage response = client.GetAsync(baseUrl + "GetAllEmployee").Result;

            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                empList = JsonConvert.DeserializeObject<List<Employee>>(json);
            }

            return View(empList);
        }

        // GET: /Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            string jsonData = JsonConvert.SerializeObject(emp);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(baseUrl + "AddEmployee", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: /Employee/Edit/5
        public IActionResult Edit(int id)
        {
            Employee emp = new Employee();
            HttpResponseMessage response = client.GetAsync(baseUrl + "GetEmployeeById/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                emp = JsonConvert.DeserializeObject<Employee>(json);
            }

            return View(emp);
        }

        // POST: /Employee/Edit
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            string jsonData = JsonConvert.SerializeObject(emp);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(baseUrl + "UpdateEmployee", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(emp);
        }

        // GET: /Employee/Delete/5
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = client.PostAsync(baseUrl + "Delete/" + id, null).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Employee deleted successfully!";
            }
            else
            {
                TempData["error"] = "Error deleting employee!";
            }

            return RedirectToAction("Index");
        }


        //// POST: /Employee/DeleteConfirmed
        //[HttpPost]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    HttpResponseMessage response = client.PostAsync(baseUrl + "Delete/" + id, null).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Delete", new { id = id });
        //}
    }
}
