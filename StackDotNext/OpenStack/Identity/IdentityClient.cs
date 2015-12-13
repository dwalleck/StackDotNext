using Newtonsoft.Json;
using StackDotNext.OpenStack.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StackDotNext.OpenStack.Identity
{
    public class IdentityClient
    {
        public HttpClient Client { get; set; }
        public string BaseUrl { get; set; }

        public IdentityClient(string baseUrl)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            BaseUrl = baseUrl;
        }

        public async Task<Access> Authenticate(string username, string password, string tenantName)
        {

            var authRequest = new AuthRequest(username, password, tenantName);
            var content = JsonConvert.SerializeObject(authRequest);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/v2.0/tokens", request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<RootObject>(responseBody);
            return root.Access;

        }
    }
}
