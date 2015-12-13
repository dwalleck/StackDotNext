﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackDotNext.OpenStack.Compute.Models
{
    public class RebuildServerRequest
    {
        [JsonProperty(PropertyName = "rebuild")]
        public RebuildServerContent RequestContent { get; set; }

        public RebuildServerRequest(string name, string imageId)
        {
            RequestContent = new RebuildServerContent
            {
                Name = name,
                ImageRef = imageId,
            };
        }
    }

    public class RebuildServerContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "imageRef")]
        public string ImageRef { get; set; }

    }
}
