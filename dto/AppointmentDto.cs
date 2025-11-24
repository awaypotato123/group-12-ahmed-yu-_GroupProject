namespace group_12_ahmed_yu__GroupProject.dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; }
        public string ReasonForVisit { get; set; }
        public string Notes { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
    }
}
