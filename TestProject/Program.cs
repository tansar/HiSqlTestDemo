using System;
using System.Collections.Generic;
using System.Diagnostics;
using HiSql;
using SqlSugar;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetSqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeSqlClient();
            
            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarClient();

            int _count = 100000;

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));
           
            
            Console.WriteLine("初始化sqlsugar专用表成功!");

 
            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景  向表中插入{_count}条数据,都用各个ORM常用的插入试(不用bulkcopy因为这个方法是底层库提供的)");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");

            

           
            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add( new { SID=(i+1), UName=$"hisql{i}",Age=20+( i % 50), Salary=5000+(i%2000)+ random.Next(10), Descript=$"hisql初始创建" });
                
                //sqlsugar用匿句类报错用实体类
                lstobj2.Add(new Table.HTest02 { SID = (i + 1), UName = $"sqlsugar{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"sqlsugar初始创建" });
                lstobj3.Add(new Table.HTest03 { SID = (i + 1), UName = $"freesql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"freesql初始创建" });
            }

            //删除测试表中的数据
            sqlClient.TrunCate("HTest01").ExecCommand();
            sqlClient.TrunCate("HTest02").ExecCommand();
            sqlClient.TrunCate("HTest03").ExecCommand();

            Stopwatch sw = new Stopwatch();

            #region freesql
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------FreeSql 测试----------");
            Console.WriteLine($"FreeSql 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp3 = freeClient.Queryable<Table.HTest03>().Where(w => w.Age < 0).ToList();
            Console.WriteLine($"FreeSql  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            freeClient.Insert<Table.HTest03>(lstobj3).ExecuteAffrows();
            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion


            #region sqlsugar
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------SqlSugar 测试----------");
            Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp2 = sugarClient.Queryable<Table.HTest02>("HTest02").Where(w => w.Age < 1).ToList();
            Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            sw.Stop();
            Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion




            #region hisql
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------HiSql 测试----------");
            Console.WriteLine($"HiSql 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp1 = sqlClient.Query("HTest01").Field("*").Take(1).Skip(1).ToDynamic();
            Console.WriteLine($"HiSql  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            sqlClient.Insert("HTest01", lstobj).ExecCommand();
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion


            var s = Console.ReadLine();
        }
    }
}
