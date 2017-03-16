using DynamicModel.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DynamicModel.Infrastructure.Extensions
{
    public static class DynamicModelDbContextExtensions
    {
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="property"></param>
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

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="property"></param>
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

        /// <summary>
        /// 创建模型表(sql server)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        public static void CreateModel(this DynamicModelDbContext context, RuntimeModelMeta model)
        {
            DbConnection conn = context.Database.GetDbConnection();

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
                    case "timestamp":
                        createTableCmd.CommandText += "timestamp";
                        break;
                    case "string":
                        if(p.Length!=0)
                            createTableCmd.CommandText += "nvarchar("+ p.Length + ")";
                        else
                            createTableCmd.CommandText += "nvarchar(max)";
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