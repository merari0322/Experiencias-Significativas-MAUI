using System.Threading.Tasks;
using System.Windows.Input;
using Experiencias_Significativas_App.MAUI.Services;
using Experiencias_Significativas_App.MAUI.Models;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace Experiencias_Significativas_App.MAUI.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        private readonly ApiService _apiService;
        private string _username;
        private string _password;
        private bool _isBusy;

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public ICommand LoginCommand { get; }

        private async Task LoginAsync()
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(Username))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingresa tu usuario", "Aceptar");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingresa tu contraseña", "Aceptar");
                return;
            }

            IsBusy = true;

            try
            {
                var user = new UserDto
                {
                    username = Username,
                    password = Password
                };

                var token = await _apiService.LoginAsync(user);

                if (!string.IsNullOrEmpty(token))
                {
                    // ✅ Guardamos el token
                    Preferences.Set("AuthToken", token);

                    await Application.Current.MainPage.DisplayAlert(
                        "Éxito",
                        "Inicio de sesión correcto.",
                        "Continuar"
                    );

                    // Navegar a la página principal
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Usuario o contraseña incorrectos.",
                        "Aceptar"
                    );
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error de conexión",
                    $"No se pudo conectar con el servidor:\n{ex.Message}",
                    "Aceptar"
                );
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}