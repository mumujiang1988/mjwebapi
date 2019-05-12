using api;
using ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

 /// <summary>
 /// 连接字符串
 /// </summary>
    public static class sqlcon
{ /// <summary>
  /// 连接字符串
  /// </summary>
    public static string consql()
    {
        string apppath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
           INIFile INI = new INIFile(apppath + @"\app.ini");
      string sqlstr = INI.IniReadValue("Section1", "sql"); ; //读取 ini 赋值类的 连接符
        return sqlstr;
    }
}
 
