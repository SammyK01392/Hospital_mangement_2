using Hospital_mangement_2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_mangement_2.Controllers
{
    public class EncountersController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _url = "https://localhost:7145/api/Encounters/";

        // Constructor to inject HttpClient
        public EncountersController(HttpClient client)
        {
            _client = client;
        }

        // Index - Retrieve all encounters
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Encounter> encounters = new List<Encounter>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    encounters = JsonConvert.DeserializeObject<List<Encounter>>(result) ?? new List<Encounter>();
                }
                else
                {
                    TempData["error_message"] = "Failed to retrieve encounters.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(encounters);
        }

        // Create - Show form to add a new encounter
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create - Post new encounter data to API
        [HttpPost]
        public async Task<IActionResult> Create(Encounter encounter)
        {
            if (!ModelState.IsValid)
                return View(encounter);

            try
            {
                string data = JsonConvert.SerializeObject(encounter);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_url, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["insert_message"] = "Encounter data added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to create encounter.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View();
        }

        // Edit - Get encounter by ID
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Encounter encounter = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    encounter = JsonConvert.DeserializeObject<Encounter>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch encounter for editing.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(encounter);
        }

        // Edit - Post updated encounter data
        [HttpPost]
        public async Task<IActionResult> Edit(Encounter encounter)
        {
            if (!ModelState.IsValid)
                return View(encounter);

            try
            {
                string data = JsonConvert.SerializeObject(encounter);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync(_url + encounter.HospitalId, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["update_message"] = "Encounter data updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to update encounter.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(encounter);
        }

        // Details - Show encounter details by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Encounter encounter = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    encounter = JsonConvert.DeserializeObject<Encounter>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch encounter details.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(encounter);
        }

        // Delete - Get encounter by ID
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Encounter encounter = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    encounter = JsonConvert.DeserializeObject<Encounter>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch encounter for deletion.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(encounter);
        }

        // Delete - Confirm encounter deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    TempData["delete_message"] = "Encounter data deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to delete encounter.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View();
        }
    }
}
