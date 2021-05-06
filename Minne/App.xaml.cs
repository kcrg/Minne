using Minne.Services;
using Minne.Services.Implementations;
using Minne.ViewModels;
using Minne.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Navigation;
using Prism.DryIoc;

[assembly: ExportFont("SegoeFluentIcons.ttf", Alias = "FontIcons")]
[assembly: ExportFont("Lato-Bold.ttf", Alias = "LatoBold")]
[assembly: ExportFont("Lato-Semibold.ttf", Alias = "LatoSemiBold")]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Minne
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            //MainPage = new Views.TabbedPage();

            await NavigationService.NavigateAsync($"TabbedPage"); //?{KnownNavigationParameters.SelectedTab}=ToDoListPage&{KnownNavigationParameters.CreateTab}=AboutPage

            //Routing.RegisterRoute("about", typeof(AboutPage));
            //Routing.RegisterRoute("todolist/todocreate", typeof(ToDoCreatePage));
            //Routing.RegisterRoute("todolist", typeof(ToDoListPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register(typeof(IRestService), typeof(RestService));

            //containerRegistry.RegisterForNavigation<AppShell>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.TabbedPage>();
            containerRegistry.RegisterForNavigation<ToDoListPage, ToDoListPageViewModel>();
            containerRegistry.RegisterForNavigation<ToDoCreatePage, ToDoCreatePageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
        }
    }
}
