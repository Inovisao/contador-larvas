using AppContagem.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppContagem.Services
{
    public class ContagemService
    {
        public ContagemService() { }

        public async Task<ImageReceiveRequest> Post(List<ImageSendRequest> images)
        {
            var Url = Config.Url + "contador-alevinos";
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(images);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(Url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ImageReceiveRequest>(responseContent);
                }
            }
            catch (Exception ex) { }

            return null;
        }
    }
}
