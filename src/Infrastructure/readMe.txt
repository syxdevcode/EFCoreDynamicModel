数据迁移
首先设置mvc(站点项目为启动项目)
迁移时候选择项目为DBContext实例项目
1，自动创建数据库迁移文件
   Add-Migration Init
2，数据库更新操作
   Update-Database



多个Context需要使用 -Context(dotnet 命令) context名称，
例如：


Add-Migration Init -Context ModelDbContext
Update-Database -Context ModelDbContext

Add-Migration Init -Context DynamicModelDbContext
Update-Database -Context DynamicModelDbContext
