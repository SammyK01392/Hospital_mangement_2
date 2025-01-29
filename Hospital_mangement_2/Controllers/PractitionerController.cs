using Hospital_mangement_2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_mangement_2.Controllers
{
    public class PractitionerController : Controller
    {
        private readonly string _url = "https://localhost:7145/api/Practitioners/";
        private readonly HttpClient _client;

        // Inject HttpClient using DI
        public PractitionerController(HttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string name, string specialty)
        {
            List<Practitioner> practitioners = new List<Practitioner>();

            // Build the search URL
            string searchUrl = _url;
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(specialty))
            {
                searchUrl += $"search?name={name}&specialty={specialty}";
            }

            HttpResponseMessage response = await _client.GetAsync(searchUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                practitioners = JsonConvert.DeserializeObject<List<Practitioner>>(result) ?? new List<Practitioner>();
            }

            // Pass search parameters to ViewBag
            ViewBag.Name = name;
            ViewBag.Specialty = specialty;

            return View(practitioners);
        }
        // Create - Show form to add a new practitioner
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create - Post new practitioner data to API
        [HttpPost]
        public async Task<IActionResult> Create(Practitioner practitioner)
        {
            string data = JsonConvert.SerializeObject(practitioner);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["insert_message"] = "Practitioner data added successfully.";
                return RedirectToAction("Index");
            }
            TempData["error_message"] = "Failed to add practitioner.";
            return View();
        }

        // Edit - Get practitioner by ID
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Practitioner practitioner = null;
            HttpResponseMessage response = await _client.GetAsync(_url + id);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                practitioner = JsonConvert.DeserializeObject<Practitioner>(result);
            }

            return View(practitioner);
        }

        // Edit - Post updated practitioner data
        [HttpPost]
        public async Task<IActionResult> Edit(Practitioner practitioner)
        {
            string data = JsonConvert.SerializeObject(practitioner);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PutAsync(_url + practitioner.HospitalId, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Practitioner data updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["error_message"] = "Failed to update practitioner.";
            return View(practitioner);
        }

        // Details - Show practitioner details by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Practitioner practitioner = null;
            HttpResponseMessage response = await _client.GetAsync(_url + id);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                practitioner = JsonConvert.DeserializeObject<Practitioner>(result);
            }

            return View(practitioner);
        }

        // Delete - Get practitioner by ID
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Practitioner practitioner = null;
            HttpResponseMessage response = await _client.GetAsync(_url + id);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                practitioner = JsonConvert.DeserializeObject<Practitioner>(result);
            }

            return View(practitioner);
        }

        // Delete - Confirm practitioner deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync(_url + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["delete_message"] = "Practitioner data deleted successfully.";
                return RedirectToAction("Index");
            }

            TempData["error_message"] = "Failed to delete practitioner.";
            return View();
        }
        // Login - Post patient login credentials and get the JWT token
        

    }
}
