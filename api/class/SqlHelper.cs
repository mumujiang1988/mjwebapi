using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;//第一步：引用与sql相关的命名空间
using System.Data;//引用表的命名空间
using System.Configuration;




/// <summary>
///第二步： 把命名空间删掉，使得我们在以后的开发中不用去创建一个sqlhelper类的对象和引用它的命名空间。实现直接调用
/// </summary>
//第三步：在class前面加上一个public   实现我们可以在dal的任何地方中去调用它。
//public 叫公共类   它的访问权限是最大的。 如果在class前面加上了它的话，那么这个类我们可以在当前的类库中任意调用。
public class SqlHelper
{
    //第四步：我们想要直接调用sqlHelper类，删除命名空间是不够的，我们还要把里面的方法和字段全部设置为静态。
    public static string connstr;
        //= ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
    
   
    //公共的静态的  连接字符串
    public static int ExecuteScalar(string cmdText, params SqlParameter[] pms)  
    {                                                      //params:不限长度的数组
        SqlConnection conn = new SqlConnection(connstr);
        
        //第一步：创建数据库对象连接
        conn.Open();    //第二步：打开数据库
        SqlCommand cmd = new SqlCommand(cmdText, conn);  //第三步：创建数据库命令对象和数据库查询语句
         cmd.CommandTimeout = 10000;//超时设置

        //第四步：创建参数对象
        //cmd.Parameters.Add(pms);//如果我们在这里直接放入pms数组，那么cmd添加进去的都是SqlParameter，而不是具体的参数。

        //  int[] ii = new int[5] { 1,2,3,4,5 };
        if (pms != null)
        {

            //for (int i = 0; i <pms.Length; i++)
            //{
            //    if (pms[i]!=null)
            //    {
            //        cmd.Parameters.Add(pms[i]); 
            //    }

            //}
            foreach (SqlParameter item in pms)//第一个参数：你的数据类型  //第二个参数是值  //第三个参数：in   在什么什么里面   //第四个参数   数组的名称
            {
                if (item != null)
                {
                    cmd.Parameters.Add(item);
                }
            }
        }
        //第五步：返回结果
        int i = Convert.ToInt32(cmd.ExecuteScalar());
        
        //第六步：关闭数据库
        conn.Close();
        //第六步:把最终的结果返回到外面去
        return i;
    }   ///执行查询，并返回object类型，查询所返回的结果集中第一行的第一列。
    public static int ExecuteNonQuery(string cmdText, params SqlParameter[] pms)
    {
        //params:不限长度的数组
        //第一步：创建数据库对象连接
        SqlConnection conn = new SqlConnection(connstr);
        //第二步：打开数据库
        conn.Open();
        //第三步：创建数据库命令对象和数据库查询语句
        SqlCommand cmd = new SqlCommand(cmdText, conn);
        //第四步：创建参数对象
        //cmd.Parameters.Add(pms);//如果我们在这里直接放入pms数组，那么cmd添加进去的都是SqlParameter，而不是具体的参数。
        

        //  int[] ii = new int[5] { 1,2,3,4,5 };
        if (pms != null)
        {

            //for (int i = 0; i <pms.Length; i++)
            //{
            //    if (pms[i]!=null)
            //    {
            //        cmd.Parameters.Add(pms[i]); 
            //    }

            //}
           
            foreach (SqlParameter item in pms)//第一个参数：你的数据类型  //第二个参数是值  //第三个参数：in   在什么什么里面   //第四个参数   数组的名称
            {
               
                if (item != null)
                {
                    cmd.Parameters.Add(item);
                }
            }
        }
        //第五步：返回结果
         int i = Convert.ToInt32(cmd.ExecuteNonQuery());
        cmd.Parameters.Clear();
        //第六步：关闭数据库
        conn.Close();
        //第六步:把最终的结果返回到外面去
        return i;
       
    }  /// 适用于 insert delete 和update操做 返回影响行。
    //返回一张表
    public static DataTable GetTable(string cmdText, params SqlParameter[] pms)
    {
        //params:不限长度的数组
        //第一步：创建数据库对象连接
        SqlConnection conn = new SqlConnection(connstr);
        //第二步：打开数据库
        conn.Open();
        //第三步：创建数据库命令对象和数据库查询语句
        SqlCommand cmd = new SqlCommand(cmdText, conn);
        //第四步：创建参数对象
        //cmd.Parameters.Add(pms);//如果我们在这里直接放入pms数组，那么cmd添加进去的都是SqlParameter，而不是具体的参数。


        //  int[] ii = new int[5] { 1,2,3,4,5 };
        if (pms != null)
        {

            //for (int i = 0; i <pms.Length; i++)
            //{
            //    if (pms[i]!=null)
            //    {
            //        cmd.Parameters.Add(pms[i]); 
            //    }

            //}
            foreach (SqlParameter item in pms)//第一个参数：你的数据类型  //第二个参数是值  //第三个参数：in   在什么什么里面   //第四个参数   数组的名称
            {
                if (item != null)
                {
                    cmd.Parameters.Add(item);
                }
            }
        }
        //5. SqlDataAdapter是.net中用于存放数组库里面取出来的数   相当于我们现实生活中的容器
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //6.去创建一个适配器 用来接受容器的
        DataSet ds = new DataSet();
        sda.Fill(ds,"aa");
        conn.Close();
        DataTable dt = ds.Tables["aa"];
        return dt;
    }      ///DataTable dt = conn.GetTable("查询语句");这样根据查询得到一张表  例： dataGridView1.DataSource = SqlHelper.GetTable("select * from express");// 表格显示数据库内容

    public static SqlDataReader GetReader(string cmdText, params SqlParameter[] pms)
    {
        SqlConnection conn = new SqlConnection(connstr);
        //第二步：打开数据库
        conn.Open();
        //第三步：创建数据库命令对象和数据库查询语句
        SqlCommand cmd = new SqlCommand(cmdText, conn);
        //第四步：创建参数对象
        //cmd.Parameters.Add(pms);//如果我们在这里直接放入pms数组，那么cmd添加进去的都是SqlParameter，而不是具体的参数。


        //  int[] ii = new int[5] { 1,2,3,4,5 };
        if (pms != null)
        {

            //for (int i = 0; i <pms.Length; i++)
            //{
            //    if (pms[i]!=null)
            //    {
            //        cmd.Parameters.Add(pms[i]); 
            //    }

            //}
            foreach (SqlParameter item in pms)//第一个参数：你的数据类型  //第二个参数是值  //第三个参数：in   在什么什么里面   //第四个参数   数组的名称
            {
                if (item != null)
                {
                    cmd.Parameters.Add(item);
                }
            }
        }
        SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);//把数据取出来以游标的形式放入sdr
        return sdr;


    }
    /*

   /// <summary>
   /// 
   /// </summary>
   /// <param name="connectionString">目标连接字符</param>
   /// <param name="TableName">目标表</param>
   /// <param name="dt">源数据</param>
   public static void DataTableToSQLServer(  DataTable dt, string TableName)
   { 
       string connectionString = connstr;
       using (SqlConnection conn = new SqlConnection(connectionString))
       {
           using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
           {
               try
               {
                   sqlbulkcopy.DestinationTableName = TableName;
                   for (int i = 0; i < dt.Columns.Count; i++)
                   {
                       sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                   }
                   sqlbulkcopy.WriteToServer(dt);
               }
               catch (System.Exception ex)
               {
                   throw ex;
               }
           }
       }
   }
     */







    public static void DataTableToSQLServer(DataTable dt,string tableName) //datable插入到数据库
   {
       string connectionString = connstr;

       using (SqlConnection destinationConnection = new SqlConnection(connectionString))
       {
           destinationConnection.Open();

           using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
           {


               try
               {

                   bulkCopy.DestinationTableName = tableName;//要插入的表的表名
                   bulkCopy.WriteToServer(dt);
                // System.Windows.Forms.MessageBox.Show("导入成功");
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
               }
               finally
               {


               }
           }


       }

   }

 










}

