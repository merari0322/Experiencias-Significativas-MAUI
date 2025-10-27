using Experiencias_Significativas_App.MAUI.Views;

namespace Experiencias_Significativas_App.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Iniciamos la app en la pantalla de Login
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
