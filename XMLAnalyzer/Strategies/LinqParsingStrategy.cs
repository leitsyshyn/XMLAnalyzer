using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLAnalyzer.Models;
using XMLAnalyzer.Services;
using System.Xml.XPath;

namespace XMLAnalyzer.Strategies
{
    public class LinqParsingStrategy : IParsingStrategy
    {
        public List<StaffMember> Parse(string xmlFilePath, string? filterQuery = null, StaffFilter? staffFilter = null)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            IEnumerable<XElement> elements = string.IsNullOrEmpty(filterQuery)
                ? doc.Descendants("Scientist")
                : doc.XPathSelectElements(filterQuery); // Apply XPath query if provided

            return elements.Select(scientist => new StaffMember
            {
                Id = (int)scientist.Attribute("id"),
                LastName = (string)scientist.Attribute("lastName"),
                FirstName = (string)scientist.Attribute("firstName"),
                MiddleName = (string)scientist.Attribute("middleName"),
                Faculty = (string)scientist.Parent.Parent.Parent.Attribute("name"),
                Department = (string)scientist.Parent.Parent.Attribute("name"),
                DegreeLevel = (string)scientist.Element("Degree").Attribute("name"),
                DegreeSpecialization = (string)scientist.Element("Degree").Attribute("specialization"),
                DegreeAwardDate = (string)scientist.Element("Degree").Attribute("awardDate"),
                TitleName = (string)scientist.Element("Title").Attribute("name"),
                TitleStartDate = (string)scientist.Element("Title").Attribute("startDate"),
                TitleEndDate = (string)scientist.Element("Title").Attribute("endDate")
            }).ToList();
        }
    }

}
