using StackDotNext.OpenStack.Compute.Models;
using StackDotNext.OpenStack.Compute.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackDotNext.OpenStack.Compute
{
    interface IComputeClient
    {
        #region Servers

        Task<List<Server>> ListServersAsync();

        Task<List<Server>> ListServersDetailedAsync();

        Task<Server> GetServerAsync(string serverId);

        Task DeleteServerAsync(string serverId);

        Task<Addresses> ListServerAddressesAsync(string serverId);

        Task<Server> CreateServerAsync(string name, string flavorId, string imageId = null, BlockDeviceMapping blockDevice = null, bool configDriveEnabled = false);

        Task<Server> UpdateServerAsync(string serverId, string name, string accessIPv4, string accessIPv6);

        Task<Metadata> GetServerMetadataAsync(string serverId);

        Task<Metadata> SetServerMetadataAsync(string serverId, Metadata metadata);

        Task<Metadata> UpdateServerMetadataAsync(string serverId, Metadata metadata);

        Task<Metadata> GetServerMetadataItemAsync(string serverId, string key);

        Task<Metadata> SetServerMetadataItemAsync(string serverId, string key, string value);

        Task DeleteServerMetadataItemAsync(string serverId, string key);

        Task ChangeServerPasswordAsync(string serverId, string newPassword);

        Task RebootServerAsync(string serverId, string type);

        Task<Server> RebuildServerAsync(string serverId, string name, string imageId);

        Task ResizeServerAsync(string serverId, string newFlavor);

        Task ConfirmServerResizeAsync(string serverId);

        Task RevertServerResizeAsync(string serverId);

        Task<string> CreateImageAsync(string serverId, string name);

        Task<List<InstanceAction>> ListInstanceActionsAsync(string serverId);

        #endregion

        #region Flavors

        Task<List<Flavor>> ListFlavorsAsync();

        Task<List<Flavor>> ListFlavorsDetailedAsync();

        Task<Flavor> GetFlavorAsync(string flavorId);

        #endregion

        #region Images

        Task<List<Image>> ListImagesAsync();

        Task<List<Image>> ListImagesDetailedAsync();

        Task DeleteImageAsync(string imageId);

        Task<Image> GetImageAsync(string imageId);

        Task<Metadata> GetImageMetadataAsync(string imageId);

        Task<Metadata> SetImageMetadataAsync(string imageId, Metadata metadata);

        Task<Metadata> UpdateImageMetadataAsync(string imageId, Metadata metadata);

        Task<Metadata> GetImageMetadataItemAsync(string imageId, string key);

        Task<Metadata> SetImageMetadataItemAsync(string imageId, string key, string value);

        Task DeleteImageMetadataItemAsync(string imageId, string key);

        #endregion

    }
}
