using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Toast.Services;

public class TestRunner
{
    public async Task<HttpResponseMessage> ExecuteUrlAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetAsync(url);
        }
    }

    public bool ValidateResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        if (response.Content.Headers.ContentType.MediaType != "application/json")
        {
            return false;
        }

        var content = response.Content.ReadAsStringAsync().Result;
        var jsonDocument = JsonDocument.Parse(content);

        if (!jsonDocument.RootElement.TryGetProperty("value", out _))
        {
            return false;
        }

        return true;
    }
}
