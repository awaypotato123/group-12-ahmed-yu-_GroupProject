namespace group_12_ahmed_yu__GroupProject.dto
{
    public class CreateAppointmentDto
    {

       
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; }
        public string ReasonForVisit { get; set; }
        public string Notes { get; set; }
        
    }
}
