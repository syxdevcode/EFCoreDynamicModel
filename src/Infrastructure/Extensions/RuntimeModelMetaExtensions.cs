using DynamicModel.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicModel.Infrastructure.Extensions
{
    public static class RuntimeModelMetaExtensions
    {
        //反序列化获得集合
        public static RuntimeModelMeta.ModelPropertyMeta[] GetProperties(this RuntimeModelMeta meta)
        {
            if (string.IsNullOrEmpty(meta.Properties))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<RuntimeModelMeta.ModelPropertyMeta[]>(meta.Properties);
        }

        //把集合序列化成字符串，用于保存
        public static void SetProperties(this RuntimeModelMeta meta, RuntimeModelMeta.ModelPropertyMeta[] properties)
        {
            meta.Properties = JsonConvert.SerializeObject(properties);
        }
    }
}
