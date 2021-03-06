using Minne.Services;
using Minne.Services.Implementations;
using Minne.ViewModels;
using Minne.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("MDL2.ttf", Alias = "FontIcons")]
[assembly: ExportFont("Lato-Bold.ttf", Alias = "LatoBold")]
[assembly: ExportFont("Lato-Semibold.ttf", Alias = "LatoSemiBold")]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Minne
{
    public partial class App
    {
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            MainPage = new AppShell();

            Routing.RegisterRoute("about", typeof(AboutPage));
            Routing.RegisterRoute("todolist/todocreate", typeof(ToDoCreatePage));

            Routing.RegisterRoute("todolist", typeof(ToDoListPage));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(IRestService), typeof(RestService));

            containerRegistry.RegisterForNavigation<AppShell>();
            containerRegistry.RegisterForNavigation<ToDoListPage, ToDoListPageViewModel>();
            containerRegistry.RegisterForNavigation<ToDoCreatePage, ToDoCreatePageViewModel>();
        }
    }
}
