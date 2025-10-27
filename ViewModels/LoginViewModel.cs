using System.Threading.Tasks;
using System.Windows.Input;
using Experiencias_Significativas_App.MAUI.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

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
            LoginCommand = new Command(async () => await LoginAsync());
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var token = await _apiService.LoginAsync(Username, Password);

            if (token != null)
            {
                await SecureStorage.SetAsync("jwt_token", token);
                await Application.Current.MainPage.Navigation.PushAsync(new Views.RegisterPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Credenciales inválidas", "OK");
            }

            IsBusy = false;
        }
    }
}
