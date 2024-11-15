using System.Xml;
using System.Xml.Xsl;

namespace XMLAnalyzer.Services
{
    public class HtmlTransformerService
    {
        public void Transform(string xmlFilePath, string xsltFilePath, string outputHtmlFilePath)
        {
            try
            {
                // Load the XSLT
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltFilePath);

                // Transform the XML file to HTML
                using XmlWriter writer = XmlWriter.Create(outputHtmlFilePath);
                xslt.Transform(xmlFilePath, writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during transformation: {ex.Message}");
                throw;
            }
        }
    }
}
