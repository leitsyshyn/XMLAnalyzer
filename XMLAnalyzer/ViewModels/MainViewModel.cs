using System.Collections.ObjectModel;
using XMLAnalyzer.Models;
using XMLAnalyzer.Services;
using System.Windows.Input;
using System.Diagnostics;
using System.Xml.Linq;

namespace XMLAnalyzer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IXmlProcessorService _xmlProcessor;

        private readonly HtmlTransformerService _htmlTransformer;

        private bool _isFileSelected;
        public bool IsFileSelected
        {
            get => _isFileSelected;
            set => SetProperty(ref _isFileSelected, value);
        }

        private string _selectedFilePath;
        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (SetProperty(ref _selectedFilePath, value))
                {
                    Console.WriteLine($"SelectedFilePath updated to: {_selectedFilePath}");
                }
            }
        }

        public ICommand SelectFileCommand { get; }

        private async void OnSelectFile()
        {
            FilePickerService filePicker = new FilePickerService();
            string? xmlFilePath = await filePicker.PickFileAsync();

            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                SelectedFilePath = xmlFilePath;
                IsFileSelected = true;
                PopulatePickers();
            }
            else
            {
                Debug.WriteLine("No file selected or file path is null/empty.");
            }
        }

        public ObservableCollection<string> Faculties { get; set; }
        public ObservableCollection<string> Departments { get; set; }
        public ObservableCollection<string> Degrees { get; set; }
        public ObservableCollection<string> Titles { get; set; }

        private string _selectedFaculty;
        public string SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                if (SetProperty(ref _selectedFaculty, value))
                {
                    Departments.Clear();
                    if (!string.IsNullOrEmpty(value))
                    {
                        foreach (var department in _xmlProcessor.GetPickerItemsByParent(_selectedFilePath, "Department", "Faculty", value))
                        {
                            Departments.Add(department);
                        }
                    }
                    else
                    {
                        foreach (var department in _xmlProcessor.GetPickerItems(_selectedFilePath, "Department"))
                        {
                            Departments.Add(department);
                        }
                    }
                }
            }
        }

        private string _selectedDepartment;
        public string SelectedDepartment
        {
            get => _selectedDepartment;
            set => SetProperty(ref _selectedDepartment, value);
        }

        private string _selectedDegree;
        public string SelectedDegree
        {
            get => _selectedDegree;
            set => SetProperty(ref _selectedDegree, value);
        }

        private string _selectedTitle;
        public string SelectedTitle
        {
            get => _selectedTitle;
            set => SetProperty(ref _selectedTitle, value);
        }

        public ObservableCollection<string> ParsingStrategies { get; set; }

        private string _selectedStrategy;
        public string SelectedStrategy
        {
            get => _selectedStrategy;
            set
            {
                if (SetProperty(ref _selectedStrategy, value))
                {
                    // Update parsing strategy in XmlProcessorService
                    _xmlProcessor.SetParsingStrategy(value);
                }
            }
        }

        public ObservableCollection<StaffMember> FilteredStaff { get; set; }

        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand TransformCommand { get; }

        public MainViewModel(IXmlProcessorService xmlProcessor)
        {
            _xmlProcessor = xmlProcessor;
            _htmlTransformer = new HtmlTransformerService();

            Faculties = new ObservableCollection<string>();
            Departments = new ObservableCollection<string>();
            Degrees = new ObservableCollection<string>();
            Titles = new ObservableCollection<string>();
            FilteredStaff = new ObservableCollection<StaffMember>();
            ParsingStrategies = new ObservableCollection<string> { "SAX", "DOM", "LINQ" };

            SelectFileCommand = new Command(OnSelectFile);
            SearchCommand = new Command(OnSearch);
            ClearCommand = new Command(OnClear);
            AboutCommand = new Command(async () => await OnAbout());
            ExitCommand = new Command(async () => await OnExit());
            TransformCommand = new Command(OnTransform);

            IsFileSelected = false;
        }
        private void PopulatePickers()
        {
            Faculties.Clear();
            foreach (var faculty in _xmlProcessor.GetPickerItems(_selectedFilePath, "Faculty"))
            {
                Faculties.Add(faculty);
            }

            Departments.Clear();
            foreach (var department in _xmlProcessor.GetPickerItems(_selectedFilePath, "Department"))
            {
                Departments.Add(department);
            }

            Degrees.Clear();
            foreach (var degree in _xmlProcessor.GetPickerItems(_selectedFilePath, "Degree"))
            {
                Degrees.Add(degree);
            }

            Titles.Clear();
            foreach (var title in _xmlProcessor.GetPickerItems(_selectedFilePath, "Title"))
            {
                Titles.Add(title);
            }

        }

        private async Task OnAbout()
        {
            await Application.Current.MainPage.DisplayAlert(
                "About",
                "OOP Lab #2: \"XML Analyzer\"\nAuthor: Leitsyshyn Tymofii K-24\nVariant: 16",
                "OK");
        }

        private async Task OnExit()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Exit",
                "Are you sure you want to exit the app?",
                "Yes",
                "No");

            if (confirm)
            {
                Application.Current.Quit();
            }
        }

        private void OnSearch()
        {
            if (SelectedStrategy == "SAX")
            {
                var filter = new StaffFilter
                {
                    Faculty = SelectedFaculty,
                    Department = SelectedDepartment,
                    Degree = SelectedDegree,
                    Title = SelectedTitle
                };

                var results = _xmlProcessor.Parse(_selectedFilePath, filter: filter);
                UpdateFilteredStaff(results);
            }
            else
            {
                var xpathQuery = BuildXPathQuery(SelectedFaculty, SelectedDepartment, SelectedDegree, SelectedTitle);

                var results = _xmlProcessor.Parse(_selectedFilePath, xpathQuery: xpathQuery);
                UpdateFilteredStaff(results);
            }
        }

        private void OnClear()
        {
            SelectedFaculty = null;
            SelectedDepartment = null;
            SelectedDegree = null;
            SelectedTitle = null;
            SelectedStrategy = null;

            FilteredStaff.Clear();
        }

        private async void OnTransform()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedFilePath) || FilteredStaff.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No XML file selected or no results to transform.", "OK");
                    return;
                }

                string filteredXmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FilteredResults.xml");
                string xsltFilePath = "C:\\Users\\Leitsyshyn\\source\\repos\\XMLAnalyzer\\XMLAnalyzer\\Resources\\Data\\StaffTemplate.xslt"; 
                string outputHtmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FilteredResults.html");

                GenerateFilteredXml(filteredXmlFilePath);

                _htmlTransformer.Transform(filteredXmlFilePath, xsltFilePath, outputHtmlFilePath);

                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    $"HTML file successfully generated at:\n{outputHtmlFilePath}",
                    "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private string BuildXPathQuery(string faculty, string department, string degree, string title)
        {
            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(faculty))
                conditions.Add($"parent::Scientists[parent::Department[parent::Faculty[@name='{faculty}']]]");

            if (!string.IsNullOrEmpty(department))
                conditions.Add($"parent::Scientists[parent::Department[@name='{department}']]");

            if (!string.IsNullOrEmpty(degree))
                conditions.Add($"Degree[@name='{degree}']");

            if (!string.IsNullOrEmpty(title))
                conditions.Add($"Title[@name='{title}']");

            return conditions.Count > 0
                ? $"//Scientist[{string.Join(" and ", conditions)}]"
                : "//Scientist";
        }

        private void UpdateFilteredStaff(IEnumerable<StaffMember> results)
        {
            FilteredStaff.Clear();

            foreach (var staff in results)
            {
                FilteredStaff.Add(staff);
            }
        }

        private void GenerateFilteredXml(string outputFilePath)
        {
            try
            {
                var xmlDoc = new XDocument(
                    new XElement("Scientists",
                        FilteredStaff.Select(staff => new XElement("Scientist",
                            new XElement("FirstName", staff.FirstName),
                            new XElement("MiddleName", staff.MiddleName),
                            new XElement("LastName", staff.LastName),
                            new XElement("Faculty", staff.Faculty),
                            new XElement("Department", staff.Department),
                            new XElement("DegreeLevel", staff.DegreeLevel),
                            new XElement("DegreeSpecialization", staff.DegreeSpecialization),
                            new XElement("DegreeAwardDate", staff.DegreeAwardDate),
                            new XElement("TitleName", staff.TitleName),
                            new XElement("TitleStartDate", staff.TitleStartDate),
                            new XElement("TitleEndDate", staff.TitleEndDate)
                        ))
                    )
                );

                xmlDoc.Save(outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while generating filtered XML: {ex.Message}");
                throw;
            }
        }
    }

}
