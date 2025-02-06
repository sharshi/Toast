using System.Net.Http;
using System.Threading.Tasks;

namespace Toast.Services
{
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
            // Validate the response based on expected criteria
            return response.IsSuccessStatusCode;
        }
    }
}
