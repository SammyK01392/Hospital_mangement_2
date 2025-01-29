using Hospital_mangement_2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_mangement_2.Controllers
{
    public class PatientController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _url = "https://localhost:7145/api/Patients/";

        // Constructor to inject HttpClient
        public PatientController(HttpClient client)
        {
            _client = client;
        }

        // Index - Retrieve all patients
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    patients = JsonConvert.DeserializeObject<List<Patient>>(result) ?? new List<Patient>();
                }
                else
                {
                    TempData["error_message"] = "Failed to retrieve patients.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(patients);
        }

        // Create - Show form to add a new patient
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create - Post new patient data to API
        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            try
            {
                string data = JsonConvert.SerializeObject(patient);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_url, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["insert_message"] = "Patient data added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to create patient.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View();
        }

        // Edit - Get patient by ID
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Patient patient = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch patient for editing.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(patient);
        }

        // Edit - Post updated patient data
        [HttpPost]
        public async Task<IActionResult> Edit(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            try
            {
                string data = JsonConvert.SerializeObject(patient);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync(_url + patient.HospitalId, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["update_message"] = "Patient data updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to update patient.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(patient);
        }

        // Details - Show patient details by ID
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Patient patient = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch patient details.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(patient);
        }

        // Delete - Get patient by ID
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Patient patient = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(result);
                }
                else
                {
                    TempData["error_message"] = "Failed to fetch patient for deletion.";
                }
            }
            catch (Exception ex)
            {
                TempData["error_message"] = $"Error: {ex.Message}";
            }
            return View(patient);
        }

        // Delete - Confirm patient deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(_url + id);
                if (response.IsSuccessStatusCode)
                {
                    TempData["delete_message"] = "Patient data deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error_message"] = "Failed to delete patient.";
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
