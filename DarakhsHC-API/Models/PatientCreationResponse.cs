namespace DarakhsHC_API.Models
{
    public class PatientCreationResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}