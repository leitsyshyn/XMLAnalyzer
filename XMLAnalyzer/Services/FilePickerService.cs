using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLAnalyzer.Services
{
    public class FilePickerService
    {
        public async Task<string?> PickFileAsync()
        {
            try
            {
                var customXMLFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.xml" } },
                    { DevicePlatform.Android, new[] { "application/xml" } },
                    { DevicePlatform.WinUI, new[] { ".xml" } }, 
                    { DevicePlatform.Tizen, new[] { "application/xml" } },
                    { DevicePlatform.macOS, new[] { "public.xml" } } 
                });

                FileResult? result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customXMLFileType,
                    PickerTitle = "Select an XML file"
                });

                if (result != null)
                {
                    Console.WriteLine(result.FullPath);
                    return result.FullPath;
                }
                else
                {
                    Console.WriteLine("No file selected");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File picking failed: {ex.Message}");
                return null;
            }
        }
    }
}
