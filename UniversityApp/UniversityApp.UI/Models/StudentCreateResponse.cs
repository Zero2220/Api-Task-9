namespace UniversityApp.UI.Models
{
    public class StudentCreateResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public IFormFile formFile { get; set; }
        public int GroupId { get; set; }
    }
}
