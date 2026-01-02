using Microsoft.AspNetCore.Mvc;
using PatientTracker.Models;

namespace PatientTracker.Controllers
{
	public class VisitController : Controller
	{
		private static List<Visit> visits = new List<Visit>();
		private static int nextVisitId = 1;

		// List all visits
		public IActionResult ListOfVisits()
		{
			// Sort by most recent first
			var sortedVisits = visits.OrderByDescending(v => v.VisitDate).ToList();

			// Add patient info to each visit for display
			foreach (var visit in sortedVisits)
			{
				visit.Patient = PatientController.GetPatientById(visit.PatientId);
			}

			return View(sortedVisits);
		}

		// Patient-specific visits
		public IActionResult PatientVisits(int patientId)
		{
			var patient = PatientController.GetPatientById(patientId);

			if (patient == null)
			{
				return NotFound();
			}

			// Get all visits for this patient
			var patientVisits = visits.Where(v => v.PatientId == patientId)
									  .OrderByDescending(v => v.VisitDate)
									  .ToList();

			ViewBag.Patient = patient;
			return View(patientVisits);
		}

		// GET: Schedule new visit
		[HttpGet]
		public IActionResult ScheduleVisit(int? patientId)
		{
			var model = new Visit();

			if (patientId.HasValue)
			{
				model.PatientId = patientId.Value;
				var patient = PatientController.GetPatientById(patientId.Value);
				ViewBag.PatientName = $"{patient?.FirstName} {patient?.LastName}";
			}

			ViewBag.Patients = PatientController.GetAllPatients();
			return View(model);
		}

		// POST: Schedule new visit
		[HttpPost]
		public IActionResult ScheduleVisit(Visit visit)
		{
			// Just save it regardless of validation
			visit.VisitId = nextVisitId++;
			visit.CreatedDate = DateTime.Now;
			visit.Status = "Scheduled";

			// Set defaults for optional fields
			if (string.IsNullOrEmpty(visit.Notes))
				visit.Notes = "";
			if (string.IsNullOrEmpty(visit.ProviderId))
				visit.ProviderId = "";
			if (visit.Duration == 0)
				visit.Duration = 30;

			visits.Add(visit);

			return RedirectToAction("ListOfVisits");
		}

		// View visit details
		public IActionResult VisitDetails(int id)
		{
			var visit = visits.FirstOrDefault(v => v.VisitId == id);

			if (visit == null)
			{
				return NotFound();
			}

			// Add patient info
			visit.Patient = PatientController.GetPatientById(visit.PatientId);

			return View(visit);
		}

		// Complete a visit (change status)
		public IActionResult CompleteVisit(int id)
		{
			var visit = visits.FirstOrDefault(v => v.VisitId == id);
			if (visit != null)
			{
				visit.Status = "Completed";
				visit.UpdatedDate = DateTime.Now;
			}

			return RedirectToAction("ListOfVisits");
		}

		// Cancel a visit
		public IActionResult CancelVisit(int id)
		{
			var visit = visits.FirstOrDefault(v => v.VisitId == id);
			if (visit != null)
			{
				visit.Status = "Cancelled";
				visit.UpdatedDate = DateTime.Now;
			}

			return RedirectToAction("ListOfVisits");
		}

		// Helper method to get visits by patient
		public static List<Visit> GetVisitsByPatientId(int patientId)
		{
			return visits.Where(v => v.PatientId == patientId).ToList();
		}
	}
}