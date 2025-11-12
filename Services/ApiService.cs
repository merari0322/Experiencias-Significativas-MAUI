using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Experiencias_Significativas_App.MAUI.Models;

namespace Experiencias_Significativas_App.MAUI.Services
{
    public class ApiService
    {
        private static HttpClient _httpClient; // 👈 Ahora es static
        private const string BaseUrl = "http://10.0.2.2:5062/api/";

        public ApiService()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(BaseUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };
            }
        }

        public async Task<string?> LoginAsync(UserDto user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 👇 Solo usa la ruta relativa, no la URL completa
                var url = "Auth/Login";

                System.Diagnostics.Debug.WriteLine($"➡️ Enviando POST a: {BaseUrl}{url}");
                System.Diagnostics.Debug.WriteLine($"📤 Body: {json}");

                var response = await _httpClient.PostAsync(url, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"⬅️ HTTP: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"📦 Body respuesta: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Error del servidor: {response.StatusCode}");
                    return null;
                }

                // Analizamos el JSON
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

                // Intentamos obtener el token de diferentes estructuras posibles
                if (root.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("token", out var tokenFromData))
                {
                    return tokenFromData.GetString();
                }

                if (root.TryGetProperty("token", out var token))
                {
                    return token.GetString();
                }

                System.Diagnostics.Debug.WriteLine("⚠️ No se encontró token en la respuesta");
                return null;
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error HTTP: {httpEx.Message}");
                throw new Exception($"Error de conexión: {httpEx.Message}");
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("❌ Timeout");
                throw new Exception("La solicitud tardó demasiado tiempo");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error general: {ex.Message}");
                throw;
            }
        }
    }
}