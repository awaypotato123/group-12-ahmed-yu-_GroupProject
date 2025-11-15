namespace group_12_ahmed_yu__GroupProject.dto
{
    public class PatchDoctorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Specialization { get; set; }
       
        public decimal ConsultationFee { get; set; }

        
    }
}
