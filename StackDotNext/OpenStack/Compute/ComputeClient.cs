using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackDotNext.OpenStack.Compute.Models;
using StackDotNext.OpenStack.Compute.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StackDotNext.OpenStack.Compute
{
    class ComputeClient : IComputeClient
    {

        public HttpClient Client { get; set; }
        public string BaseUrl { get; set; }
        public JsonSerializer Serializer { get; set; }

        public ComputeClient(string baseUrl, string token)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("X-Auth-Token", token);
            BaseUrl = baseUrl;
        }


        public async Task ChangeServerPasswordAsync(string serverId, string newPassword)
        {
            var requestBody = new ChangePasswordRequest(newPassword);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);

        }

        public async Task ConfirmServerResizeAsync(string serverId)
        {
            var requestBody = new ConfirmResizeRequest();
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);

        }

        public async Task<string> CreateImageAsync(string serverId, string name)
        {
            var requestBody = new CreateImageRequest(name);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);

            var imageLocation = response.Headers.GetValues("location").FirstOrDefault();
            var imageId = imageLocation.Split('/').Last();
            return imageId;

        }

        public async Task<Server> CreateServerAsync(string name, string flavorId, string imageId = null, BlockDeviceMapping blockDevice = null, bool configDriveEnabled = false)
        {
            var requestBody = new CreateServerRequest(name, flavorId, imageId: imageId, blockDeviceMapping: blockDevice, isConfigDriveEnabled: configDriveEnabled);
            var content = JsonConvert.SerializeObject(
                requestBody, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers", request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var serverResponse = JsonConvert.DeserializeObject<ServerResponse>(responseBody);
            return serverResponse.Server;

        }

        public async Task DeleteImageAsync(string imageId)
        {
            var response = await Client.DeleteAsync($"{BaseUrl}/images/{imageId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteImageMetadataItemAsync(string imageId, string key)
        {
            var response = await Client.DeleteAsync($"{BaseUrl}/images/{imageId}/metadata/{key}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteServerAsync(string serverId)
        {
            var response = await Client.DeleteAsync($"{BaseUrl}/servers/{serverId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteServerMetadataItemAsync(string serverId, string key)
        {
            var response = await Client.DeleteAsync($"{BaseUrl}/servers/{serverId}/metadata/{key}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Flavor> GetFlavorAsync(string flavorId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/flavors/{flavorId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var flavorResponse = JsonConvert.DeserializeObject<GetFlavorResponse>(responseBody);
            return flavorResponse.Flavor;
        }

        public async Task<Image> GetImageAsync(string imageId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/images/{imageId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var imageResponse = JsonConvert.DeserializeObject<GetImageResponse>(responseBody);
            return imageResponse.Image;
        }

        public async Task<Metadata> GetImageMetadataAsync(string imageId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/images/{imageId}/metadata");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;
        }

        public async Task<Metadata> GetImageMetadataItemAsync(string imageId, string key)
        {
            var response = await Client.GetAsync($"{BaseUrl}/images/{imageId}/metadata/{key}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;
        }

        public async Task<Server> GetServerAsync(string serverId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers/{serverId}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var serverResponse = JsonConvert.DeserializeObject<ServerResponse>(responseBody);
            return serverResponse.Server;
        }

        public async Task<Metadata> GetServerMetadataAsync(string serverId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers/{serverId}/metadata");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;
        }

        public async Task<Metadata> GetServerMetadataItemAsync(string serverId, string key)
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers/{serverId}/metadata/{key}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;
        }

        public async Task<List<Flavor>> ListFlavorsAsync()
        {
            var response = await Client.GetAsync($"{BaseUrl}/flavors");
            var responseBody = await response.Content.ReadAsStringAsync();
            var flavorsResponse = JsonConvert.DeserializeObject<ListFlavorsResponse>(responseBody);
            return flavorsResponse.Flavors;
        }

        public async Task<List<Flavor>> ListFlavorsDetailedAsync()
        {
            var response = await Client.GetAsync($"{BaseUrl}/flavors/detail");
            var responseBody = await response.Content.ReadAsStringAsync();
            var flavorsResponse = JsonConvert.DeserializeObject<ListFlavorsResponse>(responseBody);
            return flavorsResponse.Flavors;
        }

        public async Task<List<Image>> ListImagesAsync()
        {
            var response = await Client.GetAsync($"{BaseUrl}/images");
            var responseBody = await response.Content.ReadAsStringAsync();
            var imagesResponse = JsonConvert.DeserializeObject<ListImagesResponse>(responseBody);
            return imagesResponse.Images;
        }

        public async Task<List<Image>> ListImagesDetailedAsync()
        {
            var response = await Client.GetAsync($"{BaseUrl}/images/detail");
            var responseBody = await response.Content.ReadAsStringAsync();
            var imagesResponse = JsonConvert.DeserializeObject<ListImagesResponse>(
                responseBody,
                new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        args.ErrorContext.Handled = true;
                    }
                });
            return imagesResponse.Images;

        }

        public async Task<List<InstanceAction>> ListInstanceActionsAsync(string serverId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers/{serverId}/os-instance-actions");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var actionsResponse = JsonConvert.DeserializeObject<ListInstanceActionsResponse>(responseBody);
            return actionsResponse.InstanceActions;
        }

        public async Task<Addresses> ListServerAddressesAsync(string serverId)
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers/{serverId}/ips");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var addressesResponse = JsonConvert.DeserializeObject<ListAddressesResponse>(responseBody);
            return addressesResponse.Addresses;
        }

        public async Task<List<Server>> ListServersAsync()
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var serversResponse = JsonConvert.DeserializeObject<ListServersResponse>(responseBody);
            return serversResponse.Servers;

        }

        public async Task<List<Server>> ListServersDetailedAsync()
        {
            var response = await Client.GetAsync($"{BaseUrl}/servers/detail");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var serversResponse = JsonConvert.DeserializeObject<ListServersResponse>(responseBody);
            return serversResponse.Servers;

        }

        public async Task RebootServerAsync(string serverId, string type)
        {
            var requestBody = new RebootServerRequest(type);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);

        }

        public async Task<Server> RebuildServerAsync(string serverId, string name, string imageId)
        {
            var requestBody = new RebuildServerRequest(name, imageId);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var serverResponse = JsonConvert.DeserializeObject<ServerResponse>(responseBody);
            return serverResponse.Server;

        }

        public async Task ResizeServerAsync(string serverId, string newFlavor)
        {
            var requestBody = new ResizeServerRequest(newFlavor);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);

        }

        public async Task RevertServerResizeAsync(string serverId)
        {
            var requestBody = new RevertResizeRequest();
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/action", request);
        }

        public async Task<Metadata> SetImageMetadataAsync(string imageId, Metadata metadata)
        {
            var requestBody = new MetadataTransaction(metadata);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/images/{imageId}/metadata", request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;

        }

        public async Task<Metadata> SetImageMetadataItemAsync(string imageId, string key, string value)
        {
            Metadata requestBody = new Metadata()
            {
                {key, value}
            };
            var content = JsonConvert.SerializeObject(requestBody);
            var serializedContent = new StringContent(content, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/images/{imageId}/metadata/{key}");
            request.Content = serializedContent;
            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;

        }

        public async Task<Metadata> SetServerMetadataAsync(string serverId, Metadata metadata)
        {
            var requestBody = new MetadataTransaction(metadata);
            var content = JsonConvert.SerializeObject(requestBody);
            var request = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{BaseUrl}/servers/{serverId}/metadata", request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;

        }

        public async Task<Metadata> SetServerMetadataItemAsync(string serverId, string key, string value)
        {
            Metadata requestBody = new Metadata()
            {
                {key, value}
            };
            var content = JsonConvert.SerializeObject(requestBody);
            var serialized_content = new StringContent(content, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/servers/{serverId}/metadata/{key}");
            request.Content = serialized_content;
            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;

        }

        public async Task<Metadata> UpdateImageMetadataAsync(string imageId, Metadata metadata)
        {
            var requestBody = new MetadataTransaction(metadata);
            var content = JsonConvert.SerializeObject(requestBody);
            var serializedContent = new StringContent(content, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/images/{imageId}/metadata");
            request.Content = serializedContent;
            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;

        }

        public async Task<Server> UpdateServerAsync(string serverId, string name, string accessIPv4, string accessIPv6)
        {
            var requestBody = new UpdateServerRequest(name, accessIPv4, accessIPv6);
            var content = JsonConvert.SerializeObject(requestBody);
            var serialized_content = new StringContent(content, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/servers/{serverId}");
            request.Content = serialized_content;
            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var serverResponse = JsonConvert.DeserializeObject<ServerResponse>(responseBody);
            return serverResponse.Server;

        }

        public async Task<Metadata> UpdateServerMetadataAsync(string serverId, Metadata metadata)
        {

            var requestBody = new MetadataTransaction(metadata);
            var content = JsonConvert.SerializeObject(requestBody);
            var serialized_content = new StringContent(content, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/servers/{serverId}/metadata");
            request.Content = serialized_content;
            var response = await Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var metadataResponse = JsonConvert.DeserializeObject<MetadataTransaction>(responseBody);
            return metadataResponse.Metadata;

        }
    }
}
