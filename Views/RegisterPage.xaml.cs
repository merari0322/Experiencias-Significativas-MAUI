using Experiencias_Significativas_App.MAUI.Views;

namespace Experiencias_Significativas_App.MAUI.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Registro", "Cuenta creada correctamente.", "OK");

            // Regresar al login después del registro
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnLoginLinkTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
