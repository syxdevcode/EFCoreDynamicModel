using DynamicModel.Domain;
using DynamicModel.Infrastructure;
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

        public HomeController(IRuntimeModelProvider privader, DynamicModelDbContext dbContext)
        {
            _privader = privader;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
           // List<RuntimeModelMeta.ModelPropertyMeta> modelmeta = new List<RuntimeModelMeta.ModelPropertyMeta>();

           // modelmeta.Add(new RuntimeModelMeta.ModelPropertyMeta()
           // {
           //     Id = 1,
           //     PropertyName = "Size",
           //     Name = "尺寸",
           //     ValueType = "string",
           //     Length = 20
           // });
           // modelmeta.Add(new RuntimeModelMeta.ModelPropertyMeta()
           // {
           //     Id = 1,
           //     PropertyName = "Color",
           //     Name = "颜色",
           //     ValueType = "string",
           //     Length = 20
           // });

           //var s= JsonConvert.SerializeObject(modelmeta);



            Type modelType = _privader.GetType(2);//获取id=1的模型类型
            object obj = Activator.CreateInstance(modelType);//创建实体
            var entity = obj as DynamicEntity;//类型转换，目的是进行赋值
            entity["Id"] = 1;
            entity["Name"] = "名称";
            entity["Size"] = "10*20";
            entity["Color"] = "red";

            //_dbContext.CreateModel();

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