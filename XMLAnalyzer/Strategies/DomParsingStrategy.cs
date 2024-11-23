using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XMLAnalyzer.Models;
using XMLAnalyzer.Services;

namespace XMLAnalyzer.Strategies
{
    public class DomParsingStrategy : IParsingStrategy
    {
        public List<StaffMember> Parse(string xmlFilePath, string? filterQuery = null, StaffFilter? staffFilter = null)
        {
            var staffMembers = new List<StaffMember>();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);

            XmlNodeList scientistNodes = string.IsNullOrEmpty(filterQuery)
             ? doc.SelectNodes("//Scientist")
             : doc.SelectNodes(filterQuery);


            foreach (XmlNode scientistNode in scientistNodes)
            {
                var facultyNode = scientistNode.ParentNode.ParentNode.ParentNode;
                var departmentNode = scientistNode.ParentNode.ParentNode;

                var member = new StaffMember
                {
                    Id = int.Parse(scientistNode.Attributes["id"].Value),
                    LastName = scientistNode.Attributes["lastName"].Value,
                    FirstName = scientistNode.Attributes["firstName"].Value,
                    MiddleName = scientistNode.Attributes["middleName"]?.Value,
                    Faculty = facultyNode.Attributes["name"].Value,
                    Department = departmentNode.Attributes["name"].Value,
                    DegreeLevel = scientistNode.SelectSingleNode("Degree").Attributes["name"].Value,
                    DegreeSpecialization = scientistNode.SelectSingleNode("Degree").Attributes["specialization"].Value,
                    DegreeAwardDate = scientistNode.SelectSingleNode("Degree").Attributes["awardDate"].Value,
                    TitleName = scientistNode.SelectSingleNode("Title").Attributes["name"].Value,
                    TitleStartDate = scientistNode.SelectSingleNode("Title").Attributes["startDate"].Value,
                    TitleEndDate = scientistNode.SelectSingleNode("Title").Attributes["endDate"]?.Value
                };

                staffMembers.Add(member);
            }

            return staffMembers;
        }
    }
}
