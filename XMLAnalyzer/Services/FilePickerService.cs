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
                // Define custom XML file type
                var customXMLFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.xml" } }, // iOS
                    { DevicePlatform.Android, new[] { "application/xml" } }, // Android
                    { DevicePlatform.WinUI, new[] { ".xml" } }, // Windows
                    { DevicePlatform.Tizen, new[] { "application/xml" } }, // Tizen
                    { DevicePlatform.macOS, new[] { "public.xml" } } // macOS
                });

                // Show the file picker dialog
                FileResult? result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customXMLFileType, // Filter by XML files
                    PickerTitle = "Select an XML file"
                });

                // If a file is selected, return its file path
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
