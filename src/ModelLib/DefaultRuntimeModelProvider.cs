using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using ModelLib.Extensions;

namespace ModelLib
{
    public class DefaultRuntimeModelProvider: IRuntimeModelProvider
    {
        private Dictionary<int, Type> _resultMap;
        private readonly IOptions<RuntimeModelMetaConfig> _config;
        private object _lock = new object();
        public DefaultRuntimeModelProvider(IOptions<RuntimeModelMetaConfig> config)
        {
            //通过依赖注入方式获取到模型配置信息
            _config = config;
        }
        //动态编译结果的缓存，这样在获取动态类型时不用每次都编译一次
        public Dictionary<int, Type> Map
        {
            get
            {
                if (_resultMap == null)
                {
                    lock (_lock)
                    {
                        _resultMap = new Dictionary<int, Type>();

                        foreach (var item in _config.Value.Metas)
                        {
                            //根据RuntimeModelMeta编译成类，具体实现看后面内容
                            var result = RuntimeTypeBuilder.Build(GetTypeMetaFromModelMeta(item));
                            //编译结果放到缓存中，方便下次使用
                            _resultMap.Add(item.ModelId, result);
                        }
                    }
                }
                return _resultMap;
            }
        }

        public Type GetType(int modelId)
        {
            Dictionary<int, Type> map = Map;
            Type result = null;
            if (!map.TryGetValue(modelId, out result))
            {
                throw new NotSupportedException("dynamic model not supported:" + modelId);
            }
            return result;
        }

        public Type[] GetTypes()
        {
            int[] modelIds = _config.Value.Metas.Select(m => m.ModelId).ToArray();
            return Map.Where(m => modelIds.Contains(m.Key)).Select(m => m.Value).ToArray();
        }

        public Type[] GetTypes(string modelName)
        {
            int[] modelIds = _config.Value.Metas.Where(m => m.ModelName==modelName).Select(o=>o.ModelId).ToArray();
            return Map.Where(m => modelIds.Contains(m.Key)).Select(m => m.Value).ToArray();
        }

        //这个方法就是把一个RuntimeModelMeta转换成更接近类结构的TypeMeta对象
        private TypeMeta GetTypeMetaFromModelMeta(RuntimeModelMeta meta)
        {
            TypeMeta typeMeta = new TypeMeta();
            //我们让所有的动态类型都继承自DynamicEntity类，这个类主要是为了方便属性数据的读取，具体代码看后面
            typeMeta.BaseType = typeof(DynamicEntity);
            typeMeta.TypeName = meta.ClassName;

            foreach (var item in meta.Properties)
            {
                TypeMeta.TypePropertyMeta pmeta = new TypeMeta.TypePropertyMeta();
                pmeta.PropertyName = item.PropertyName;
                //如果必须输入数据，我们在属性上增加RequireAttribute特性，这样方便我们进行数据验证
                if (item.IsRequired)
                {
                    TypeMeta.AttributeMeta am = new TypeMeta.AttributeMeta();
                    am.AttributeType = typeof(RequiredAttribute);
                    am.Properties = new string[] { "ErrorMessage" };
                    am.PropertyValues = new object[] { "请输入" + item.Name };
                    pmeta.AttributeMetas.Add(am);
                }

                if (item.ValueType == "string")
                {
                    pmeta.PropertyType = typeof(string);
                    TypeMeta.AttributeMeta am = new TypeMeta.AttributeMeta();
                    //增加长度验证特性
                    am.AttributeType = typeof(StringLengthAttribute);
                    am.ConstructorArgTypes = new Type[] { typeof(int) };
                    am.ConstructorArgValues = new object[] { item.Length };
                    am.Properties = new string[] { "ErrorMessage" };
                    am.PropertyValues = new object[] { item.Name + "长度不能超过" + item.Length.ToString() + "个字符" };

                    pmeta.AttributeMetas.Add(am);
                }
                else if (item.ValueType == "int")
                {
                    if (!item.IsRequired)
                    {
                        pmeta.PropertyType = typeof(int?);
                    }
                    else
                    {
                        pmeta.PropertyType = typeof(int);
                    }
                }
                else if (item.ValueType == "datetime")
                {
                    if (!item.IsRequired)
                    {
                        pmeta.PropertyType = typeof(DateTime?);
                    }
                    else
                    {
                        pmeta.PropertyType = typeof(DateTime);
                    }
                }
                else if (item.ValueType == "bool")
                {
                    if (!item.IsRequired)
                    {
                        pmeta.PropertyType = typeof(bool?);
                    }
                    else
                    {
                        pmeta.PropertyType = typeof(bool);
                    }
                }
                typeMeta.PropertyMetas.Add(pmeta);
            }
            return typeMeta;
        }
    }
}
