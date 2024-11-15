using XMLAnalyzer.Models;

namespace XMLAnalyzer.Services
{
    public interface IXmlProcessorService
    {
        void SetParsingStrategy(string strategy);

        bool ValidateXml(string xmlFilePath, out string validationErrors);
        IEnumerable<string> GetPickerItems(string xmlFilePath, string elementName);
        IEnumerable<string> GetPickerItemsByParent(string xmlFilePath, string childElement, string parentElement, string parentValue);
        string? GetParentAttribute(string xmlFilePath, string childElement, string childValue, string parentElement);

        List<StaffMember> Parse(string xmlFilePath, string? xpathQuery = null, StaffFilter? filter = null);
    }

}
