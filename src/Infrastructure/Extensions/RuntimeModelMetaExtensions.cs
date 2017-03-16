using DynamicModel.Domain;
using Newtonsoft.Json;

namespace DynamicModel.Infrastructure.Extensions
{
    public static class RuntimeModelMetaExtensions
    {
        /// <summary>
        /// 反序列化获得集合
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        public static RuntimeModelMeta.ModelPropertyMeta[] GetProperties(this RuntimeModelMeta meta)
        {
            if (string.IsNullOrEmpty(meta.Properties))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<RuntimeModelMeta.ModelPropertyMeta[]>(meta.Properties);
        }

        /// <summary>
        /// 把集合序列化成字符串，用于保存
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="properties"></param>
        public static void SetProperties(this RuntimeModelMeta meta, RuntimeModelMeta.ModelPropertyMeta[] properties)
        {
            meta.Properties = JsonConvert.SerializeObject(properties);
        }
    }
}