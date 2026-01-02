namespace PatientTracker.Models
{
	public class Visit
	{
		public int VisitId { get; set; }
		public int PatientId { get; set; }
		public DateTime VisitDate { get; set; }
		public string VisitType { get; set; }
		public string Status { get; set; }
		public string ReasonForVisit { get; set; }
		public string Notes { get; set; }
		public string ProviderId { get; set; }
		public int Duration { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		// Navigation property
		public PatientModel Patient { get; set; }
	}
}
