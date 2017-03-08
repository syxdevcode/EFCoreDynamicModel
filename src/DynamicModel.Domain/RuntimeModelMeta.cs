﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicModel.Domain
{
    public class RuntimeModelMeta
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }//模型名称
        public string ClassName { get; set; }//类名称
        public ModelPropertyMeta[] Properties { get; set; }

        public class ModelPropertyMeta
        {
            public string Name { get; set; }//对应的中文名称
            public string PropertyName { get; set; } //类属性名称
            public int Length { get; set; }//数据长度，主要用于string类型

            public bool IsRequired { get; set; }//是否必须输入，用于数据验证
            public string ValueType { get; set; }//数据类型，可以是字符串，日期，bool等
        }
    }
}
