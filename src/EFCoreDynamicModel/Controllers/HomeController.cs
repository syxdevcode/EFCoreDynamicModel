using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ModelLib;
using System;

namespace EFCoreDynamicModel.Controllers
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
            Type modelType = _privader.GetType(1);//获取id=1的模型类型
            object obj = Activator.CreateInstance(modelType);//创建实体
            var entity = obj as DynamicEntity;//类型转换，目的是进行赋值
            entity["Id"] = 1;
            entity["Name"] = "名称";
            entity["Size"] = "10*20";
            entity["Color"] = "red";
            _dbContext.Add(entity);
            _dbContext.SaveChanges();


            DynamicModelDbContext.Add(entity);

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}