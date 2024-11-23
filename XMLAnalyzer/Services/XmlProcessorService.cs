using System.Xml.Schema;
using XMLAnalyzer.Strategies;
using XMLAnalyzer.Models;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text;


namespace XMLAnalyzer.Services
{
    public class XmlProcessorService : IXmlProcessorService
    {
        private IParsingStrategy _strategy;

        public XmlProcessorService(IParsingStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetParsingStrategy(string strategy)
        {
            if (strategy != null)
            {
                switch (strategy)
                {
                    case "SAX":
                        _strategy = new SaxParsingStrategy();
                        break;
                    case "DOM":
                        _strategy = new DomParsingStrategy();
                        break;
                    case "LINQ":
                        _strategy = new LinqParsingStrategy();
                        break;
                    default:
                        throw new ArgumentException("Invalid parsing strategy.");
                }
            }
            
        }


        public bool ValidateXml(string xmlFilePath, out string validationErrors)
        {
            validationErrors = string.Empty;
            var localErrors = new StringBuilder();

            try
            {
                XDocument doc = XDocument.Load(xmlFilePath);

                var schemaLocation = doc.Root?.Attribute(XNamespace.Xmlns + "xsi")?.Value;
                if (string.IsNullOrEmpty(schemaLocation))
                {
                    validationErrors = "No schema reference found in the XML.";
                    return false;
                }

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, schemaLocation); 

                doc.Validate(schemaSet, (sender, e) =>
                {
                    localErrors.AppendLine(e.Message);
                });

                validationErrors = localErrors.ToString();
                return string.IsNullOrEmpty(validationErrors);
            }
            catch (Exception ex)
            {
                validationErrors = $"Error during validation: {ex.Message}";
                return false;
            }
        }



        public IEnumerable<string> GetPickerItems(string filePath, string elementName)
        {
            try
            {
                var doc = XDocument.Load(filePath);

                var values = doc.Descendants(elementName)
                                .Select(e => e.Attribute("name")?.Value)
                                .Where(v => !string.IsNullOrEmpty(v))
                                .Distinct()
                                .ToList();

                return values;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetUniqueValues: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> GetPickerItemsByParent(string filePath, string childElement, string parentElement, string parentValue)
        {
            var xml = XDocument.Load(filePath);

            return xml.Descendants(childElement)
                      .Where(e => e.Parent.Name == parentElement && e.Parent.Attribute("name")?.Value == parentValue)
                      .Select(e => e.Attribute("name")?.Value)
                      .Where(name => !string.IsNullOrEmpty(name))
                      .Distinct()
                      .ToList();
        }

        public string? GetParentAttribute(string filePath, string childElement, string childValue, string parentElement)
        {
            var xml = XDocument.Load(filePath);

            return xml.Descendants(childElement)
                      .FirstOrDefault(e => e.Attribute("name")?.Value == childValue)?
                      .Parent?.Attribute("name")?.Value;
        }



        public List<StaffMember> Parse(string xmlFilePath, string? xpathQuery = null, StaffFilter? filter = null)
        {
            return _strategy.Parse(xmlFilePath, xpathQuery, filter);
        }

    }


}
