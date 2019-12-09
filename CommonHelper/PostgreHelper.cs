using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;

namespace Advantech.IFactory.CommonHelper
{
    /// <summary>
    /// 数据库连接属性
    /// </summary>
    public class DbConnectionString
    {
        public string DbType { set; get; }
        public string Database { set; get; }
        public string Port { set; get; }
        public string Host { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
    }

    public abstract class PostgreBase
    {
        public static string connString = "";

        #region 基础操作
        /// <summary>
        /// 执行增删改方法
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns>0的执行失败</returns>
        public static int ExecuteNonQuery(string sql)
        {
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand SqlCommand = new NpgsqlCommand(sql, SqlConn))
                {
                    try
                    {
                        SqlConn.Open();
                        int count = SqlCommand.ExecuteNonQuery();  //执行查询并返回受影响的行数
                        SqlConn.Close();
                        return count;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ExecuteNonQuery error=" + sql + ex.Message);
                    }
                    finally
                    {
                        SqlConn.Close();
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 执行查询方法
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns>返回DbDataReader</returns>
        public static DbDataReader GetReader(string sql)
        {
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand SqlCommand = new NpgsqlCommand(sql, SqlConn))
                {
                    try
                    {
                        SqlConn.Open();
                        DbDataReader reader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        return reader;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("GetReader error=" + sql + ex.Message);
                        SqlConn.Close();
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql">执行的sql</param>
        /// <returns>如果为空，则返回DBNull.Value</returns>
        public static object ExecuteScalar(string sql)
        {
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand SqlCommand = new NpgsqlCommand(sql, SqlConn))
                {
                    try
                    {
                        SqlConn.Open();
                        object s = SqlCommand.ExecuteScalar();
                        SqlConn.Close();
                        return s;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ExecuteScalar error=" + sql + ex.Message);
                    }
                    finally
                    {
                        SqlConn.Close();
                    }
                }
            } 
            return null;
        }
        #endregion
    }
    /// <summary>
    /// 数据库操作基类(for PostgreSQL)
    /// </summary>
    public class PostgreHelper: PostgreBase
    {
        /// <summary>
        /// 将对象实体插入数据库
        /// </summary>
        /// <param name="Obj">对象</param>
        /// <returns>默认为返回新增行id</returns> 
        public static int InsertSingleEntity<T>(T Obj) where T : class
        {
            long id = -1;
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                try
                {
                    SqlConn.Open();
                    id = SqlConn.Insert<T>(Obj);
                    SqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("InsertSingleEntity error " + ex.Message);
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                    }
                }
            }
            return (int)id;
        }
        
        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName">表名称</param>
        /// <param name="id">主键id值</param>
        /// <param name="UpdateDic">需要更新的键值对字典</param>
        /// <returns></returns>
        public static int UpdateSingleEntity<T>(string TableName, int id, Dictionary<string,object>UpdateDic) where T : new()
        {
            int count = 0;
            string CommandString=string.Empty;
            string KeysAndValues = string.Empty;

            foreach (var item in UpdateDic)//反射类型与值并拼接命令字
            {
                if (KeysAndValues.Length > 0) KeysAndValues = KeysAndValues + ",";
                KeysAndValues += string.Format("{0}='{1}'", item.Key, item.Value);
            }
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                try
                {
                    CommandString = string.Format("Update {0} Set {1} where id={2}", TableName, KeysAndValues, id);//update tabe1 set username='youname' where userid=5
                    SqlConn.Open();
                    count = SqlConn.Execute(CommandString);
                    SqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UpdateSingleEntity error " + CommandString + " " + ex.Message);
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="UpdateEntity">实体对象</param>
        /// <returns>执行成功返回1，失败返回0</returns>
        public static int UpdateSingleEntity<T>(T UpdateEntity) where T : class
        {
            bool success = false;
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                try
                {
                    SqlConn.Open();
                    success = SqlConn.Update<T>(UpdateEntity);
                    SqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UpdateSingleEntity error " + ex.Message);
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                    }
                }
            }
            if(success)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取一个对象后返回
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <returns>T对象</returns> 
        public static T GetSingleEntity<T>(string cmdText) where T : new()
        {  
            T obj = default;
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                try
                {
                    SqlConn.Open();
                    obj = SqlConn.QueryFirstOrDefault<T>(cmdText);
                    SqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(cmdText + " " + ex.Message);
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 读取队列后返回
        /// </summary>
        /// <param name="cmdText">命令语句</param>
        /// <returns>list对象</returns> 
        public static List<T> GetEntityList<T>(string cmdText) where T : new()
        {
            List<T> listT = new List<T>();
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(connString))
            {
                try
                {
                    SqlConn.Open();
                    listT = SqlConn.Query<T>(cmdText).AsList<T>();
                    SqlConn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(cmdText + " " + ex.Message);
                    if (SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                    }
                }
            }
            return listT;
        }
    }
}
