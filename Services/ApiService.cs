using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Experiencias_Significativas_App.MAUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            // ✅ Usa la IP especial para emulador Android
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5062/api/")
            };
        }

        // 🔹 Clase que representa la respuesta del backend
        private class LoginResponse
        {
            public string Token { get; set; } // asegúrate de que coincida con lo que devuelve tu AuthController
        }

        // 🔹 Método para probar la conexión con el backend
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Auth");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error de conexión: {ex.Message}");
                return false;
            }
        }

        // 🔹 Método para iniciar sesión
        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                // 📡 Enviamos los datos al endpoint del backend
                var response = await _httpClient.PostAsJsonAsync("Auth/Login", loginData);

                // ⚠️ Si el backend responde con error (400, 401, 500...)
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Error HTTP: {response.StatusCode} - {errorMessage}");
                    return null;
                }

                // 📦 Leer el JSON correctamente con tolerancia de mayúsculas/minúsculas
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(options);

                // ✅ Devuelve el token si existe
                return result?.Token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en LoginAsync: {ex.Message}");
                return null;
            }
        }
    }
}
