using HiSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace TestProject
{
    public class Demo_Init
    {
        public static HiSqlClient GetSqlClient()
        {

            HiSqlClient sqlclient = new HiSqlClient(
                     new HiSql.ConnectionConfig()
                     {
                         DbType = DBType.SqlServer,
                         DbServer = "local-HoneBI",
                         ConnectionString = "server=(local);uid=sa;pwd=Hone@123;database=HiSql;",//; MultipleActiveResultSets = true;
                                                                                                 //User="tansar",//可以指定登陆用户的帐号

                        

                         Schema = "dbo",
                         IsEncrypt = true,
                         IsAutoClose = false,
                         SqlExecTimeOut = 60000,

                         AppEvents = new AopEvent()
                         {
                             OnDbDecryptEvent = (connstr) =>
                             {
                                 //解密连接字段
                                 //Console.WriteLine($"数据库连接:{connstr}");

                                 return connstr;
                             },
                             OnLogSqlExecuting = (sql, param) =>
                             {
                                 //sql执行前 日志记录 (异步)

                                 //Console.WriteLine($"sql执行前记录{sql} time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");
                             },
                             OnLogSqlExecuted = (sql, param) =>
                             {
                                 //sql执行后 日志记录 (异步)
                                 //Console.WriteLine($"sql执行后记录{sql} time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");
                             },
                             OnSqlError = (sqlEx) =>
                             {
                                 //sql执行错误后 日志记录 (异步)
                                 Console.WriteLine(sqlEx.Message.ToString());
                             },
                             OnTimeOut = (int timer) =>
                             {
                                 //Console.WriteLine($"执行SQL语句超过[{timer.ToString()}]毫秒...");
                             }
                         }
                     }
                     );


            //sqlclient.CodeFirst.InstallHisql();

            return sqlclient;
        }

        public static SqlSugarClient GetSugarClient()
        {
            SqlSugarClient db = new SqlSugarClient(new SqlSugar.ConnectionConfig()
            {
                ConnectionString = "server=(local);uid=sa;pwd=Hone@123;database=HiSql;",//连接符字串
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true
            });
            return db;
        }


        public static IFreeSql GetFreeSqlClient()
        {

            IFreeSql fsql = new FreeSql.FreeSqlBuilder()
               .UseConnectionString(FreeSql.DataType.SqlServer, "server=(local);uid=sa;pwd=Hone@123;database=HiSql;")
               .UseAutoSyncStructure(true) //自动同步实体结构到数据库
               .Build();
            return fsql;
        }
    }
}
