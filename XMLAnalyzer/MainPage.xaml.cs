using XMLAnalyzer.ViewModels;
using Microsoft.Maui.Controls;
using XMLAnalyzer.Services;
using XMLAnalyzer.Strategies;

namespace XMLAnalyzer;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        IParsingStrategy strategy = new LinqParsingStrategy();
        IXmlProcessorService xmlProcessor = new XmlProcessorService(strategy);

        BindingContext = new MainViewModel(xmlProcessor);
    }
}

