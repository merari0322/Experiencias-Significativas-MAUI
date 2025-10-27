using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Para guardar el token en Preferences
using Experiencias_Significativas_App.MAUI.Services; // Servicio para llamar al backend

namespace Experiencias_Significativas_App.MAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService;

        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService(); // Inicializa el servicio que conecta al backend
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor ingresa usuario y contraseña.", "Aceptar");
                return;
            }

            try
            {
                // 🔹 Mostrar indicador de carga si lo tienes en tu XAML
                if (LoadingIndicator != null)
                {
                    LoadingIndicator.IsVisible = true;
                    LoadingIndicator.IsRunning = true;
                }
                IsEnabled = false;

                // 🔹 Probar si el backend está accesible
                var isConnected = await _apiService.TestConnectionAsync();
                if (!isConnected)
                {
                    await DisplayAlert("Error", "No se puede conectar al servidor. Asegúrate de que el backend esté ejecutándose.", "Aceptar");
                    return;
                }

                // 🔹 Llamar al método de login del backend
                var token = await _apiService.LoginAsync(username, password);

                if (!string.IsNullOrEmpty(token))
                {
                    // ✅ Si el backend devuelve un token válido
                    await DisplayAlert("Éxito", "Inicio de sesión correcto.", "Continuar");

                    // 🔹 Guarda el token localmente
                    Preferences.Set("AuthToken", token);

                    // 🔹 Redirige a la pantalla principal (HomePage)
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    // ❌ Si no hay token, algo falló en la autenticación
                    System.Diagnostics.Debug.WriteLine("❌ Login fallido: token vacío o nulo.");
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en OnLoginClicked: {ex.Message}");
                await DisplayAlert("Error", $"Ocurrió un problema al intentar iniciar sesión: {ex.Message}", "Aceptar");
            }
            finally
            {
                // 🔹 Ocultar indicador de carga y reactivar la página
                if (LoadingIndicator != null)
                {
                    LoadingIndicator.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                }
                IsEnabled = true;
            }
        }

        // 🔹 Evento para redirigir a la pantalla de registro
        private async void OnRegisterLinkTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
