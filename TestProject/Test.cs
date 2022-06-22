using HiSql;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class Test
    {



        #region sqlserver 测试 5字段插入测试
        public static void TestSqlServerInsert(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetSqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarClient();

            

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 Sqlserver  向表中插入{_count}条数据 常规数据插入)");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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
        }


        public static void TestSqlServerBulkCopy(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetSqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 Sqlserver  向表中插入{_count}条数据 BulkCopy方式插入");
            Console.WriteLine($"适用于大量数据导入场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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
            //freeClient.Insert<Table.HTest03>(lstobj3).ExecuteAffrows();
            freeClient.Insert<Table.HTest03>(lstobj3).ExecuteSqlBulkCopy();
            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
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
            //sqlClient.Insert("HTest01", lstobj).ExecCommand();

            sqlClient.BulkCopyExecCommand("HTest01", lstobj);
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
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
            //sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            sugarClient.Fastest<Table.HTest02>().BulkCopy(lstobj2);
            sw.Stop();
            Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion
        }

        #endregion

        #region  sqlserver 测试 50字段插入测试
        public static void TestSqlServer50ColInsert(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetSqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.H_Test50C01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.H_Test50C02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.H_Test50C03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 Sqlserver  向表中插入{_count}条数据 50列 常规数据插入)");
            Console.WriteLine($"适用于大量数据导入场景");




            List<object> lstobj = new List<object>();
            List<Table.H_Test50C02> lstobj2 = new List<Table.H_Test50C02>();
            List<Table.H_Test50C03> lstobj3 = new List<Table.H_Test50C03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.H_Test50C01 { 
                    Material=(900000+i).ToString(),
                    Batch=(30000000+i).ToString(),
                    TestNum1= random.Next(10,100),
                    TestNum2 = random.Next(10, 100),
                    TestNum3 = random.Next(10, 100),
                    TestNum4 = random.Next(10, 100),
                    TestNum5 = random.Next(10, 100),
                    TestNum6 = random.Next(10, 100),
                    TestNum7 = random.Next(10, 100),
                    TestNum8 = random.Next(10, 100),
                    TestNum9 = random.Next(10, 100),
                    TestNum10 = random.Next(10, 100),
                    TestNum11 = random.Next(10, 100),
                    TestNum12 = random.Next(10, 100),
                    TestNum13= random.Next(10, 100),
                    TestNum14= random.Next(10, 100),
                    TestNum15= random.Next(10, 100),
                    TestStr1 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr2 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr3 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr4 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr5 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr6 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr7 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr8 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr9 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr10 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr11 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr12 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr13 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr14 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr15 = $"hisql{random.Next(1, 100).ToString()}",
                    TestDec1 = i+ random.Next(1, 10000)/3,
                    TestDec2 = i + random.Next(1, 10000) / 3,
                    TestDec3 = i + random.Next(1, 10000) / 3,
                    TestDec4 = i + random.Next(1, 10000) / 3,
                    TestDec5 = i + random.Next(1, 10000) / 3,
                    TestDec6 = i + random.Next(1, 10000) / 3,
                    TestDec7 = i + random.Next(1, 10000) / 3,
                    TestDec8 = i + random.Next(1, 10000) / 3,
                    TestDec9 = i + random.Next(1, 10000) / 3,
                    TestDec10 = i + random.Next(1, 10000) / 3,
                    TestDec11 = i + random.Next(1, 10000) / 3,
                    TestDec12 = i + random.Next(1, 10000) / 3,
                    TestDec13 = i + random.Next(1, 10000) / 3,
                    TestDec14 = i + random.Next(1, 10000) / 3,
                    TestDec15 = i + random.Next(1, 10000) / 3,
                    Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

                //sqlsugar用匿句类报错用实体类
                lstobj2.Add(new Table.H_Test50C02
                {
                    Material = (900000 + i).ToString(),
                    Batch = (30000000 + i).ToString(),
                    TestNum1 = random.Next(10, 100),
                    TestNum2 = random.Next(10, 100),
                    TestNum3 = random.Next(10, 100),
                    TestNum4 = random.Next(10, 100),
                    TestNum5 = random.Next(10, 100),
                    TestNum6 = random.Next(10, 100),
                    TestNum7 = random.Next(10, 100),
                    TestNum8 = random.Next(10, 100),
                    TestNum9 = random.Next(10, 100),
                    TestNum10 = random.Next(10, 100),
                    TestNum11 = random.Next(10, 100),
                    TestNum12 = random.Next(10, 100),
                    TestNum13 = random.Next(10, 100),
                    TestNum14 = random.Next(10, 100),
                    TestNum15 = random.Next(10, 100),
                    TestStr1 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr2 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr3 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr4 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr5 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr6 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr7 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr8 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr9 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr10 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr11 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr12 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr13 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr14 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr15 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestDec1 = i + random.Next(1, 10000) / 3,
                    TestDec2 = i + random.Next(1, 10000) / 3,
                    TestDec3 = i + random.Next(1, 10000) / 3,
                    TestDec4 = i + random.Next(1, 10000) / 3,
                    TestDec5 = i + random.Next(1, 10000) / 3,
                    TestDec6 = i + random.Next(1, 10000) / 3,
                    TestDec7 = i + random.Next(1, 10000) / 3,
                    TestDec8 = i + random.Next(1, 10000) / 3,
                    TestDec9 = i + random.Next(1, 10000) / 3,
                    TestDec10 = i + random.Next(1, 10000) / 3,
                    TestDec11 = i + random.Next(1, 10000) / 3,
                    TestDec12 = i + random.Next(1, 10000) / 3,
                    TestDec13 = i + random.Next(1, 10000) / 3,
                    TestDec14 = i + random.Next(1, 10000) / 3,
                    TestDec15 = i + random.Next(1, 10000) / 3,
                    Salary = 5000 + (i % 2000) + random.Next(10),
                    Descript = $"sqlsugar初始创建"
                });
                lstobj3.Add(new Table.H_Test50C03
                {
                    Material = (900000 + i).ToString(),
                    Batch = (30000000 + i).ToString(),
                    TestNum1 = random.Next(10, 100),
                    TestNum2 = random.Next(10, 100),
                    TestNum3 = random.Next(10, 100),
                    TestNum4 = random.Next(10, 100),
                    TestNum5 = random.Next(10, 100),
                    TestNum6 = random.Next(10, 100),
                    TestNum7 = random.Next(10, 100),
                    TestNum8 = random.Next(10, 100),
                    TestNum9 = random.Next(10, 100),
                    TestNum10 = random.Next(10, 100),
                    TestNum11 = random.Next(10, 100),
                    TestNum12 = random.Next(10, 100),
                    TestNum13 = random.Next(10, 100),
                    TestNum14 = random.Next(10, 100),
                    TestNum15 = random.Next(10, 100),
                    TestStr1 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr2 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr3 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr4 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr5 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr6 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr7 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr8 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr9 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr10 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr11 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr12 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr13 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr14 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr15 = $"freesql{random.Next(1, 100).ToString()}",
                    TestDec1 = i + random.Next(1, 10000) / 3,
                    TestDec2 = i + random.Next(1, 10000) / 3,
                    TestDec3 = i + random.Next(1, 10000) / 3,
                    TestDec4 = i + random.Next(1, 10000) / 3,
                    TestDec5 = i + random.Next(1, 10000) / 3,
                    TestDec6 = i + random.Next(1, 10000) / 3,
                    TestDec7 = i + random.Next(1, 10000) / 3,
                    TestDec8 = i + random.Next(1, 10000) / 3,
                    TestDec9 = i + random.Next(1, 10000) / 3,
                    TestDec10 = i + random.Next(1, 10000) / 3,
                    TestDec11 = i + random.Next(1, 10000) / 3,
                    TestDec12 = i + random.Next(1, 10000) / 3,
                    TestDec13 = i + random.Next(1, 10000) / 3,
                    TestDec14 = i + random.Next(1, 10000) / 3,
                    TestDec15 = i + random.Next(1, 10000) / 3,
                    Salary = 5000 + (i % 2000) + random.Next(10),
                    Descript = $"freesql初始创建"
                });
            }

            //删除测试表中的数据
            sqlClient.TrunCate("H_Test50C01").ExecCommand();
            sqlClient.TrunCate("H_Test50C02").ExecCommand();
            sqlClient.TrunCate("H_Test50C03").ExecCommand();

            Stopwatch sw = new Stopwatch();



            #region freesql
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------FreeSql 测试----------");
            Console.WriteLine($"FreeSql 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp3 = freeClient.Queryable<Table.H_Test50C03>().Where(w => w.TestDec1 < 0).ToList();
            Console.WriteLine($"FreeSql  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            freeClient.Insert<Table.H_Test50C03>(lstobj3).ExecuteAffrows();

            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion


            #region hisql
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------HiSql 测试----------");
            Console.WriteLine($"HiSql 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp1 = sqlClient.Query("H_Test50C01").Field("*").Take(1).Skip(1).ToDynamic();
            Console.WriteLine($"HiSql  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            sqlClient.Insert("H_Test50C01", lstobj).ExecCommand();
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion






            #region sqlsugar
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------SqlSugar 测试----------");
            Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp2 = sugarClient.Queryable<Table.H_Test50C03>("H_Test50C03").Where(w => w.TestDec1 < 1).ToList();
            Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            sugarClient.Insertable(lstobj2).AS("H_Test50C03").ExecuteCommand();
            sw.Stop();
            Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion
        }

        public static void TestSqlServer50ColBulkCopy(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetSqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.H_Test50C01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.H_Test50C02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.H_Test50C03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 Sqlserver  向表中插入{_count}条数据 50列 BulkCopy方式插入");
            Console.WriteLine($"适用于大量数据导入场景");




            List<Table.H_Test50C01> lstobj = new List<Table.H_Test50C01>();
            List<Table.H_Test50C02> lstobj2 = new List<Table.H_Test50C02>();
            List<Table.H_Test50C03> lstobj3 = new List<Table.H_Test50C03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.H_Test50C01
                {
                    Material = (900000 + i).ToString(),
                    Batch = (30000000 + i).ToString(),
                    TestNum1 = random.Next(10, 100),
                    TestNum2 = random.Next(10, 100),
                    TestNum3 = random.Next(10, 100),
                    TestNum4 = random.Next(10, 100),
                    TestNum5 = random.Next(10, 100),
                    TestNum6 = random.Next(10, 100),
                    TestNum7 = random.Next(10, 100),
                    TestNum8 = random.Next(10, 100),
                    TestNum9 = random.Next(10, 100),
                    TestNum10 = random.Next(10, 100),
                    TestNum11 = random.Next(10, 100),
                    TestNum12 = random.Next(10, 100),
                    TestNum13 = random.Next(10, 100),
                    TestNum14 = random.Next(10, 100),
                    TestNum15 = random.Next(10, 100),
                    TestStr1 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr2 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr3 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr4 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr5 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr6 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr7 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr8 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr9 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr10 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr11 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr12 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr13 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr14 = $"hisql{random.Next(1, 100).ToString()}",
                    TestStr15 = $"hisql{random.Next(1, 100).ToString()}",
                    TestDec1 = i + random.Next(1, 10000) / 3,
                    TestDec2 = i + random.Next(1, 10000) / 3,
                    TestDec3 = i + random.Next(1, 10000) / 3,
                    TestDec4 = i + random.Next(1, 10000) / 3,
                    TestDec5 = i + random.Next(1, 10000) / 3,
                    TestDec6 = i + random.Next(1, 10000) / 3,
                    TestDec7 = i + random.Next(1, 10000) / 3,
                    TestDec8 = i + random.Next(1, 10000) / 3,
                    TestDec9 = i + random.Next(1, 10000) / 3,
                    TestDec10 = i + random.Next(1, 10000) / 3,
                    TestDec11 = i + random.Next(1, 10000) / 3,
                    TestDec12 = i + random.Next(1, 10000) / 3,
                    TestDec13 = i + random.Next(1, 10000) / 3,
                    TestDec14 = i + random.Next(1, 10000) / 3,
                    TestDec15 = i + random.Next(1, 10000) / 3,
                    Salary = 5000 + (i % 2000) + random.Next(10),
                    Descript = $"hisql初始创建"
                });

                //sqlsugar用匿句类报错用实体类
                lstobj2.Add(new Table.H_Test50C02
                {
                    Material = (900000 + i).ToString(),
                    Batch = (30000000 + i).ToString(),
                    TestNum1 = random.Next(10, 100),
                    TestNum2 = random.Next(10, 100),
                    TestNum3 = random.Next(10, 100),
                    TestNum4 = random.Next(10, 100),
                    TestNum5 = random.Next(10, 100),
                    TestNum6 = random.Next(10, 100),
                    TestNum7 = random.Next(10, 100),
                    TestNum8 = random.Next(10, 100),
                    TestNum9 = random.Next(10, 100),
                    TestNum10 = random.Next(10, 100),
                    TestNum11 = random.Next(10, 100),
                    TestNum12 = random.Next(10, 100),
                    TestNum13 = random.Next(10, 100),
                    TestNum14 = random.Next(10, 100),
                    TestNum15 = random.Next(10, 100),
                    TestStr1 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr2 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr3 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr4 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr5 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr6 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr7 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr8 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr9 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr10 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr11 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr12 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr13 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr14 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestStr15 = $"sqlsugar{random.Next(1, 100).ToString()}",
                    TestDec1 = i + random.Next(1, 10000) / 3,
                    TestDec2 = i + random.Next(1, 10000) / 3,
                    TestDec3 = i + random.Next(1, 10000) / 3,
                    TestDec4 = i + random.Next(1, 10000) / 3,
                    TestDec5 = i + random.Next(1, 10000) / 3,
                    TestDec6 = i + random.Next(1, 10000) / 3,
                    TestDec7 = i + random.Next(1, 10000) / 3,
                    TestDec8 = i + random.Next(1, 10000) / 3,
                    TestDec9 = i + random.Next(1, 10000) / 3,
                    TestDec10 = i + random.Next(1, 10000) / 3,
                    TestDec11 = i + random.Next(1, 10000) / 3,
                    TestDec12 = i + random.Next(1, 10000) / 3,
                    TestDec13 = i + random.Next(1, 10000) / 3,
                    TestDec14 = i + random.Next(1, 10000) / 3,
                    TestDec15 = i + random.Next(1, 10000) / 3,
                    Salary = 5000 + (i % 2000) + random.Next(10),
                    Descript = $"sqlsugar初始创建"
                });
                lstobj3.Add(new Table.H_Test50C03
                {
                    Material = (900000 + i).ToString(),
                    Batch = (30000000 + i).ToString(),
                    TestNum1 = random.Next(10, 100),
                    TestNum2 = random.Next(10, 100),
                    TestNum3 = random.Next(10, 100),
                    TestNum4 = random.Next(10, 100),
                    TestNum5 = random.Next(10, 100),
                    TestNum6 = random.Next(10, 100),
                    TestNum7 = random.Next(10, 100),
                    TestNum8 = random.Next(10, 100),
                    TestNum9 = random.Next(10, 100),
                    TestNum10 = random.Next(10, 100),
                    TestNum11 = random.Next(10, 100),
                    TestNum12 = random.Next(10, 100),
                    TestNum13 = random.Next(10, 100),
                    TestNum14 = random.Next(10, 100),
                    TestNum15 = random.Next(10, 100),
                    TestStr1 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr2 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr3 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr4 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr5 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr6 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr7 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr8 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr9 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr10 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr11 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr12 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr13 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr14 = $"freesql{random.Next(1, 100).ToString()}",
                    TestStr15 = $"freesql{random.Next(1, 100).ToString()}",
                    TestDec1 = i + random.Next(1, 10000) / 3,
                    TestDec2 = i + random.Next(1, 10000) / 3,
                    TestDec3 = i + random.Next(1, 10000) / 3,
                    TestDec4 = i + random.Next(1, 10000) / 3,
                    TestDec5 = i + random.Next(1, 10000) / 3,
                    TestDec6 = i + random.Next(1, 10000) / 3,
                    TestDec7 = i + random.Next(1, 10000) / 3,
                    TestDec8 = i + random.Next(1, 10000) / 3,
                    TestDec9 = i + random.Next(1, 10000) / 3,
                    TestDec10 = i + random.Next(1, 10000) / 3,
                    TestDec11 = i + random.Next(1, 10000) / 3,
                    TestDec12 = i + random.Next(1, 10000) / 3,
                    TestDec13 = i + random.Next(1, 10000) / 3,
                    TestDec14 = i + random.Next(1, 10000) / 3,
                    TestDec15 = i + random.Next(1, 10000) / 3,
                    Salary = 5000 + (i % 2000) + random.Next(10),
                    Descript = $"freesql初始创建"
                });
            }

            //删除测试表中的数据
            sqlClient.TrunCate("H_Test50C01").ExecCommand();
            sqlClient.TrunCate("H_Test50C02").ExecCommand();
            sqlClient.TrunCate("H_Test50C03").ExecCommand();

            Stopwatch sw = new Stopwatch();



            #region freesql
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------FreeSql 测试----------");
            Console.WriteLine($"FreeSql 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp3 = freeClient.Queryable<Table.H_Test50C03>().Where(w => w.TestDec1 < 0).ToList();
            Console.WriteLine($"FreeSql  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            //freeClient.Insert<Table.HTest03>(lstobj3).ExecuteAffrows();
            freeClient.Insert<Table.H_Test50C03>(lstobj3).ExecuteSqlBulkCopy();
            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion


            #region hisql
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------HiSql 测试----------");
            Console.WriteLine($"HiSql 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp1 = sqlClient.Query("H_Test50C01").Field("*").Take(1).Skip(1).ToDynamic();
            Console.WriteLine($"HiSql  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            //sqlClient.Insert("HTest01", lstobj).ExecCommand();

            sqlClient.BulkCopyExecCommand("H_Test50C01", lstobj);
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion






            #region sqlsugar
            sw.Reset();
            Console.WriteLine("------------------------------");
            Console.WriteLine("----------SqlSugar 测试----------");
            Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var temp2 = sugarClient.Queryable<Table.H_Test50C02>("H_Test50C02").Where(w => w.TestDec1 < 1).ToList();
            Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            sw.Start();
            //sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            sugarClient.Fastest<Table.H_Test50C02>().BulkCopy(lstobj2);
            sw.Stop();
            Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion
        }

        #endregion




        #region mysql 测试 5字段插入测试
        public static void TestMySqlInsert(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetMySqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeMySqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarMySqlClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 MySql  向表中插入{_count}条数据 常规数据插入)");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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
        }


        public static void TestMySqlBulkCopy(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetSqlClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 MySql  向表中插入{_count}条数据 BulkCopy方式插入");
            Console.WriteLine($"适用于批量数据导入场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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
            //freeClient.Insert<Table.HTest03>(lstobj3).ExecuteAffrows();
            freeClient.Insert<Table.HTest03>(lstobj3).ExecuteSqlBulkCopy();
            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
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
            //sqlClient.Insert("HTest01", lstobj).ExecCommand();

            sqlClient.BulkCopyExecCommand("HTest01", lstobj);
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
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
            //sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            sugarClient.Fastest<Table.HTest02>().BulkCopy(lstobj2);
            sw.Stop();
            Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion
        }

        #endregion

        #region posgresql 5字段插入测试
        public static void TestPosgreSqlInsert(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetPosegreClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreePosGreSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarPoseGreSqlClient();


            //sqlClient.DbFirst.CheckTabExists("HTest01");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 PosGreSql  向表中插入{_count}条数据 常规数据插入)");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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





            //sqlsugar 在postgresql下报错
            #region sqlsugar
            //sw.Reset();
            //Console.WriteLine("------------------------------");
            //Console.WriteLine("----------SqlSugar 测试----------");
            //Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //var temp2 = sugarClient.Queryable<Table.HTest02>("HTest02").Where(w => w.Age < 1).ToList();
            //Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //sw.Start();
            //sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            //sw.Stop();
            //Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            //sw.Reset();
            #endregion
        }

        public static void TestPosgreSqlBulkCopy(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetPosegreClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreePosGreSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarPoseGreSqlClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 PosGreSql  向表中插入{_count}条数据 BulkCopy方式插入");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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
            //freeClient.Insert<Table.HTest03>(lstobj3).ExecuteAffrows();
            freeClient.Insert<Table.HTest03>(lstobj3).ExecutePgCopy();
            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
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
            //sqlClient.Insert("HTest01", lstobj).ExecCommand();

            sqlClient.BulkCopyExecCommand("HTest01", lstobj);
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion






            #region sqlsugar
            //sw.Reset();
            //Console.WriteLine("------------------------------");
            //Console.WriteLine("----------SqlSugar 测试----------");
            //Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //var temp2 = sugarClient.Queryable<Table.HTest02>("HTest02").Where(w => w.Age < 1).ToList();
            //Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //sw.Start();
            ////sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            //sugarClient.Fastest<Table.HTest02>().BulkCopy(lstobj2);
            //sw.Stop();
            //Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            //sw.Reset();
            #endregion
        }

        #endregion




        #region oracle测试 5字段插入测试
        public static void TestOracleInsert(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetOracleClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreeOraclelClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarOralceClient();


            //sqlClient.DbFirst.CheckTabExists("HTest01");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 PosGreSql  向表中插入{_count}条数据 常规数据插入)");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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





            #region sqlsugar
            //sw.Reset();
            //Console.WriteLine("------------------------------");
            //Console.WriteLine("----------SqlSugar 测试----------");
            //Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //var temp2 = sugarClient.Queryable<Table.HTest02>("HTest02").Where(w => w.Age < 1).ToList();
            //Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //sw.Start();
            //sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            //sw.Stop();
            //Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            //sw.Reset();
            #endregion
        }

        public static void TestOralceBulkCopy(int _count)
        {

            //hisql连接 请先配置好数据库连接
            HiSqlClient sqlClient = Demo_Init.GetPosegreClient();
            //hisql需要初始货安装 只需要执行一次
            sqlClient.CodeFirst.InstallHisql();


            //freesql连接
            IFreeSql freeClient = Demo_Init.GetFreePosGreSqlClient();

            //sqlsugar连接
            SqlSugarClient sugarClient = Demo_Init.GetSugarPoseGreSqlClient();



            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest01));

            Console.WriteLine("初始化hisql专用表成功!");

            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest02));


            Console.WriteLine("初始化sqlsugar专用表成功!");


            sqlClient.CodeFirst.CreateTable(typeof(Table.HTest03));
            Console.WriteLine("初始化freesql专用表成功!");


            Console.WriteLine($"测试场景 PosGreSql  向表中插入{_count}条数据 BulkCopy方式插入");
            Console.WriteLine($"用常规数据插入最适应日常应用场景");




            List<object> lstobj = new List<object>();
            List<Table.HTest02> lstobj2 = new List<Table.HTest02>();
            List<Table.HTest03> lstobj3 = new List<Table.HTest03>();
            Random random = new Random();

            //插入的参数值都随机产生 以免数据库执行相同SQL时会有缓存影响测试结果
            for (int i = 0; i < _count; i++)
            {
                //hisql可以用实体类也可以用匿名类
                lstobj.Add(new Table.HTest01 { SID = (i + 1), UName = $"hisql{i}", Age = 20 + (i % 50), Salary = 5000 + (i % 2000) + random.Next(10), Descript = $"hisql初始创建" });

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
            //freeClient.Insert<Table.HTest03>(lstobj3).ExecuteAffrows();
            freeClient.Insert<Table.HTest03>(lstobj3).ExecutePgCopy();
            sw.Stop();
            Console.WriteLine($"FreeSql 数据插入{_count}条 耗时{sw.Elapsed}秒");
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
            //sqlClient.Insert("HTest01", lstobj).ExecCommand();

            sqlClient.BulkCopyExecCommand("HTest01", lstobj);
            sw.Stop();
            Console.WriteLine($"hisql 数据插入{_count}条 耗时{sw.Elapsed}秒");
            sw.Reset();
            #endregion






            #region sqlsugar
            //sw.Reset();
            //Console.WriteLine("------------------------------");
            //Console.WriteLine("----------SqlSugar 测试----------");
            //Console.WriteLine($"SqlSugar 预热...{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //var temp2 = sugarClient.Queryable<Table.HTest02>("HTest02").Where(w => w.Age < 1).ToList();
            //Console.WriteLine($"sqlsugar  正在插入数据\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //sw.Start();
            ////sugarClient.Insertable(lstobj2).AS("HTest02").ExecuteCommand();
            //sugarClient.Fastest<Table.HTest02>().BulkCopy(lstobj2);
            //sw.Stop();
            //Console.WriteLine($"sqlsugar 数据插入{_count}条 耗时{sw.Elapsed}秒");
            //sw.Reset();
            #endregion
        }
        #endregion
    }
}
