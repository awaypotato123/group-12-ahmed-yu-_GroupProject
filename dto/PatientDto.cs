namespace group_12_ahmed_yu__GroupProject.dto
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Address { get; set; }
        public string MedicalRecordNumber { get; set; }
        public DateTime Created { get; set; }



    }
}
