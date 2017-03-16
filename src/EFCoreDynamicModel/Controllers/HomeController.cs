using DynamicModel.Domain;
using DynamicModel.Infrastructure;
using DynamicModel.Infrastructure.Extensions;
using DynamicModel.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DynamicModel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRuntimeModelProvider _privader;
        private readonly DynamicModelDbContext _dbContext;
        private readonly ModelDbContext _modeldbContext;

        public HomeController(IRuntimeModelProvider privader, DynamicModelDbContext dbContext, ModelDbContext modeldbContext)
        {
            _privader = privader;
            _dbContext = dbContext;
            _modeldbContext = modeldbContext;
        }

        public IActionResult Index()
        {
            List<RuntimeModelMeta.ModelPropertyMeta> modelmeta = new List<RuntimeModelMeta.ModelPropertyMeta>();

            modelmeta.Add(new RuntimeModelMeta.ModelPropertyMeta()
            {
                Id = 1,
                PropertyName = "Size",
                Name = "尺寸",
                ValueType = "string",
                Length = 20
            });
            modelmeta.Add(new RuntimeModelMeta.ModelPropertyMeta()
            {
                Id = 2,
                PropertyName = "Name",
                Name = "名称",
                ValueType = "string",
                Length = 20
            });
            modelmeta.Add(new RuntimeModelMeta.ModelPropertyMeta()
            {
                Id = 3,
                PropertyName = "Color",
                Name = "颜色",
                ValueType = "string",
                Length = 20
            });
            modelmeta.Add(new RuntimeModelMeta.ModelPropertyMeta()
            {
                Id = 4,
                PropertyName = "Version",
                Name = "版本",
                ValueType = "timestamp"
            });

            var s = JsonConvert.SerializeObject(modelmeta);

            RuntimeModelMeta meta = new RuntimeModelMeta();
            meta.ClassName = "BareDiamond";
            meta.ModelName = "BareDiamond";
            meta.Properties = s;
            _modeldbContext.Add(meta);
            _modeldbContext.SaveChanges();


            foreach (var item in _modeldbContext.Metas)
            {
                _dbContext.CreateModel(item);
            }

            Type modelType = _privader.GetType(1);//获取id=1的模型类型
            object obj = Activator.CreateInstance(modelType);//创建实体
            var entity = obj as DynamicEntity;//类型转换，目的是进行赋值
            entity["Id"] = 1;
            entity["Name"] = "名称";
            entity["Size"] = "10*20";
            entity["Color"] = "red";

            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}