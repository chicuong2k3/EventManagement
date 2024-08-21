﻿

using Newtonsoft.Json;

namespace EventManagement.Common.Infrastructure.Serialization
{
    public static class SerializerSettings
    {
        public static readonly JsonSerializerSettings Instance = new()
        { 
            TypeNameHandling = TypeNameHandling.All,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
        };
    }
}