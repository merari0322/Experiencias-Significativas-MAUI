using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Experiencias_Significativas_App.MAUI.Services;
using Experiencias_Significativas_App.MAUI.Models;

namespace Experiencias_Significativas_App.MAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService;

        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
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
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                IsEnabled = false;

                var user = new UserDto
                {
                    username = username,
                    password = password
                };

                var token = await _apiService.LoginAsync(user);

                if (!string.IsNullOrEmpty(token))
                {
                    await DisplayAlert("Éxito", "Inicio de sesión correcto.", "Continuar");
                    Preferences.Set("AuthToken", token);

                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos o servidor no disponible.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un problema al iniciar sesión: {ex.Message}", "Aceptar");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                IsEnabled = true;
            }
        }

        private async void OnRegisterLinkTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
