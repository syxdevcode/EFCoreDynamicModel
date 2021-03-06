﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelLib
{
    public interface IRuntimeModelProvider
    {
        Type GetType(int modelId);

        Type[] GetTypes();

        Type[] GetTypes(string modelName);
    }
}
