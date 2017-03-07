using ModelLib;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using ModelLib.Extensions;

namespace EFCoreDynamicModel.Extensions
{
    public static class ModelDbContextExtensions
    {
        //添加字段
        public static void AddField(this DynamicModelDbContext context, RuntimeModelMeta model, RuntimeModelMeta.ModelPropertyMeta property)
        {
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                DbCommand addFieldCmd = conn.CreateCommand();
                addFieldCmd.CommandText = $"alert table {model.ClassName} add {property.PropertyName} ";

                switch (property.ValueType)
                {
                    case "int":
                        addFieldCmd.CommandText += "int";
                        break;
                    case "datetime":
                        addFieldCmd.CommandText += "datetime";
                        break;
                    case "bool":
                        addFieldCmd.CommandText += "bit";
                        break;
                    default:
                        addFieldCmd.CommandText += "nvarchar(max)";
                        break;
                }

                addFieldCmd.ExecuteNonQuery();
            }
        }
        //删除字段
        public static void RemoveField(this DynamicModelDbContext context, RuntimeModelMeta model, string property)
        {
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                DbCommand removeFieldCmd = conn.CreateCommand();
                removeFieldCmd.CommandText = $"alert table {model.ClassName} DROP COLUMN  {property}";

                removeFieldCmd.ExecuteNonQuery();
            }
        }
        //创建模型表
        public static void CreateModel(this DynamicModelDbContext context, RuntimeModelMeta model)
        {
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                DbCommand createTableCmd = conn.CreateCommand();
                createTableCmd.CommandText = $"create table {model.ClassName}";
                createTableCmd.CommandText += "{id int identity(1,1)";
                foreach (var p in model.GetProperties())
                {
                    createTableCmd.CommandText += $",{p.PropertyName} ";
                    switch (p.ValueType)
                    {
                        case "int":
                            createTableCmd.CommandText += "int";
                            break;
                        case "datetime":
                            createTableCmd.CommandText += "datetime";
                            break;
                        case "bool":
                            createTableCmd.CommandText += "bit";
                            break;
                        default:
                            createTableCmd.CommandText += "nvarchar(max)";
                            break;
                    }
                }

                createTableCmd.CommandText += "}";
                createTableCmd.ExecuteNonQuery();
            }

        }
    }
}
