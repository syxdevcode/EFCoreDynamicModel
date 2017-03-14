using DynamicModel.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicModel.Infrastructure.Extensions
{
    public static class ModelDbContextExtensions
    {
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="property"></param>
        public static void AddField(this ModelDbContext context, RuntimeModelMeta model, RuntimeModelMeta.ModelPropertyMeta property)
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

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="property"></param>
        public static void RemoveField(this ModelDbContext context, RuntimeModelMeta model, string property)
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

        /// <summary>
        /// 创建模型表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        public static void CreateModel(this ModelDbContext context, RuntimeModelMeta model)
        {
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                DbCommand createTableCmd = conn.CreateCommand();
                createTableCmd.CommandText = $"create table {model.ClassName}";
                createTableCmd.CommandText += "(id int identity(1,1)";
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

                createTableCmd.CommandText += ")";
                createTableCmd.ExecuteNonQuery();
            }

        }
    }
}
