using Microsoft.AspNetCore.Mvc;
using PatientTracker.Models;
namespace PatientTracker.Controllers
{
	public class PatientController : Controller
	{
		private static List<PatientModel> patients = new List<PatientModel>();
		private static List<PatientModel> criticalPatients = new List<PatientModel>();
		private static int nextId = 1;

		//Return and populate a ListOfPatients View with the list of all patients created
		public IActionResult ListOfPatients()
		{
			return View(patients);
		}

		//Return and populate a ListOfCriticalPatients View with the list of only critical patients 
		public IActionResult ListOfCriticalPatients()
		{
			var criticals = patients.Where(p =>
				  p.headachePainValue == 10 ||
				  p.chestPainValue == 10 ||
				  p.muscleAcheValue == 10).ToList();
			return View(criticals);
		}

		//Return the CreatePatient View
		[HttpGet]
		public IActionResult CreatePatient()
		{
			return View();
		}

		//Write a method to take the new Patient object from the CreatePatient View and add it to the list of patients.
		[HttpPost]
		public IActionResult CreatePatient(PatientModel patient)
		{
			if (ModelState.IsValid)
			{
				patient.Id = nextId++;
				patients.Add(patient);
				return RedirectToAction("ListOfPatients");
			}
			return View(patient);
		}

		// Helper methods for VisitController
		public static PatientModel GetPatientById(int id)
		{
			return patients.FirstOrDefault(p => p.Id == id);
		}

		public static List<PatientModel> GetAllPatients()
		{
			return patients;
		}
	}
}
