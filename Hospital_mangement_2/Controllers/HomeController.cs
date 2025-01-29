using System.Diagnostics;
using System.Text;
using Hospital_mangement_2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Policy;

namespace Hospital_mangement_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _url = "https://localhost:7145/api/Hospitals/";

        // Constructor to inject HttpClient
        public HomeController(HttpClient client)
        {
            _client = client;
        }

        // Index - Retrieve all hospitals
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Hospital> hospitals = new List<Hospital>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    hospitals = JsonConvert.DeserializeObject<List<Hospital>>(result) ?? new List<Hospital>();
                }
                else
                {
                    TempData["error_message"] = "Failed to retrieve hospitals.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(hospitals);
        }
       


        // Create - Show form to add a new hospital
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create - Post new hospital data to API
        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            if (!ModelState.IsValid)
                return View(hospital);

            try
            {
                string data = JsonConvert.SerializeObject(hospital);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_url, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["insert_message"] = "Hospital data added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to create hospital.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View();
        }

        // Edit - Get hospital by ID
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Hospital hospital = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    hospital = JsonConvert.DeserializeObject<Hospital>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch hospital for editing.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(hospital);
        }

        // Edit - Post updated hospital data
        [HttpPost]
        public async Task<IActionResult> Edit(Hospital hospital)
        {
            if (!ModelState.IsValid)
                return View(hospital);

            try
            {
                string data = JsonConvert.SerializeObject(hospital);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync(_url + hospital.HospitalId, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["update_message"] = "Hospital data updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to update hospital.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(hospital);
        }

        // Details - Show hospital details by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Hospital hospital = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    hospital = JsonConvert.DeserializeObject<Hospital>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch hospital details.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(hospital);
        }

        // Delete - Get hospital by ID
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Hospital hospital = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    hospital = JsonConvert.DeserializeObject<Hospital>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch hospital for deletion.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(hospital);
        }

        // Delete - Confirm hospital deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    TempData["delete_message"] = "Hospital data deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to delete hospital.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Search(string name, string city)
        {
           
            ViewBag.Name = name;
            ViewBag.City = city;

            
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(city))
            {
                return View(new List<Hospital>()); 
            }

            List<Hospital> hospitals = new List<Hospital>();

            try
            {
                // Properly format the API URL and encode query parameters
                string searchUrl = $"{_url}search?";

                if (!string.IsNullOrEmpty(name))
                {
                    searchUrl += $"name={Uri.EscapeDataString(name)}&";
                }
                if (!string.IsNullOrEmpty(city))
                {
                    searchUrl += $"city={Uri.EscapeDataString(city)}";
                }

                // Trim trailing '&' or '?' if necessary
                searchUrl = searchUrl.TrimEnd('&', '?');

                Console.WriteLine($"Calling API: {searchUrl}"); // Debugging log

                // Send request to API
                HttpResponseMessage response = await _client.GetAsync(searchUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    hospitals = JsonConvert.DeserializeObject<List<Hospital>>(result) ?? new List<Hospital>();
                }
                else
                {
                    TempData["error_message"] = "No hospitals found matching your search.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }

            return View(hospitals); 
        }
     

       



        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
