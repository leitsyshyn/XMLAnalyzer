
namespace XMLAnalyzer.Models
{
    public class StaffMember
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; } 

        public string Faculty { get; set; }
        public string Department { get; set; }

        public string DegreeLevel { get; set; }
        public string DegreeSpecialization { get; set; }
        public string DegreeAwardDate { get; set; }

        public string TitleName { get; set; }
        public string TitleStartDate { get; set; }
        public string TitleEndDate { get; set; } 
    }

}
