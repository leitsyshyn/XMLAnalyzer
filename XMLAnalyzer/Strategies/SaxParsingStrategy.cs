using System.Collections.Generic;
using System.Xml;
using XMLAnalyzer.Models;
using XMLAnalyzer.Services;

namespace XMLAnalyzer.Strategies
{
    public class SaxParsingStrategy : IParsingStrategy
    {
        public List<StaffMember> Parse(string xmlFilePath, string? xpathQuery = null, StaffFilter? filter = null)
        {
            var staffMembers = new List<StaffMember>();
            var currentMember = new StaffMember();
            string currentFaculty = null;
            string currentDepartment = null;

            using (XmlReader reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Faculty":
                                currentFaculty = reader.GetAttribute("name");
                                break;

                            case "Department":
                                currentDepartment = reader.GetAttribute("name");
                                break;

                            case "Scientist":
                                currentMember = new StaffMember
                                {
                                    Id = int.Parse(reader.GetAttribute("id")),
                                    LastName = reader.GetAttribute("lastName"),
                                    FirstName = reader.GetAttribute("firstName"),
                                    MiddleName = reader.GetAttribute("middleName"),
                                    Faculty = currentFaculty,
                                    Department = currentDepartment
                                };
                                staffMembers.Add(currentMember);
                                break;

                            case "Degree":
                                currentMember.DegreeLevel = reader.GetAttribute("name");
                                currentMember.DegreeSpecialization = reader.GetAttribute("specialization");
                                currentMember.DegreeAwardDate = reader.GetAttribute("awardDate");
                                break;

                            case "Title":
                                currentMember.TitleName = reader.GetAttribute("name");
                                currentMember.TitleStartDate = reader.GetAttribute("startDate");
                                currentMember.TitleEndDate = reader.GetAttribute("endDate");
                                break;
                        }
                    }
                }
            }

            if (filter != null)
            {
                staffMembers = ApplyFilters(staffMembers, filter);
            }

            return staffMembers;
        }

        private List<StaffMember> ApplyFilters(List<StaffMember> staffMembers, StaffFilter filter)
        {
            return staffMembers.Where(member =>
                (string.IsNullOrEmpty(filter.Faculty) || member.Faculty == filter.Faculty) &&
                (string.IsNullOrEmpty(filter.Department) || member.Department == filter.Department) &&
                (string.IsNullOrEmpty(filter.Degree) || member.DegreeLevel == filter.Degree) &&
                (string.IsNullOrEmpty(filter.Title) || member.TitleName == filter.Title)
            ).ToList();
        }
    }

}
