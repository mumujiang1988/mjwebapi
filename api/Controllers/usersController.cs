using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ini;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace api.Controllers
{
   
    //实体类
    #region
    /// <summary>
    /// post 获取的数据
    /// </summary>
    public class user 
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
    public class userdata
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string roles { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string avatar { get; set; }

    }
    //===============================================================================================

    /// <summary>
    /// post 用户信息
    /// </summary>
    public class userinfo
    {/// <summary>
    /// code
    /// </summary>
        public int code { get; set; }
        public userdata data { get; set; }
        
    }
    //===============================================================================================

    /// <summary>
    /// 令牌实体类
    /// </summary>
    public class Tokenstr
    {
        /// <summary>
        ///  状态码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        ///返回令牌数据
        /// </summary>
        public tokendata data { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }

    }
    /// <summary>
    /// data 类
    /// </summary>
    public  class tokendata
    {
        /// <summary>
        /// data 类
        /// </summary>
        public   string token { get; set; }
   

    }
    //===============================================================================================
    /// <summary>
    /// 注销实体类
    /// </summary>
    public class Logout
    {
        public int code { get; set; }
        public string data { get; set; }

    }

    #endregion
    //===============================================================================================

    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {   
        //=============================================================================================== get获取用户信息
         
        /// <summary>
        ///   GET: api/users/id  获取用户信息
        /// </summary>
        /// <remarks> 
        /// 获取用户信息
        /// </remarks>
        /// <param name="id">用户id</param>
        /// <returns>用户信息</returns>
        /// <response code="200">返回用户信息</response> 
        /// <response code="201">如果id为空</response>  
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
           SqlHelper.connstr= sqlcon.consql();
            string sql = "select * from web_hr_users where id ='"+id+"'";  
            DataTable userdt = SqlHelper.GetTable(sql);
            user userjson = new user();
            userjson.username = userdt.Rows[0]["worknu"].ToString();
            userjson.password = userdt.Rows[0]["password"].ToString(); 
            return JsonConvert.SerializeObject(userjson);
            //return "cs";
        }
        //=============================================================================================== post 登录

        /// <summary>
        ///  POST:api/users/login 登录处理 返回令牌 
        /// </summary>
        /// <remarks> 
        /// 登录处理 返回令牌 
        /// </remarks>
        /// <response code="200">返回令牌 </response> 
        /// <response code="201">返回失败</response>  
        /// <returns>返回令牌 </returns>

        [HttpPost("login", Name = "loginPost")]
        public string loginPost()//登录
        {
            string body = new StreamReader(Request.Body).ReadToEnd();
            //Console.WriteLine(userjson);
              user userjson = JsonConvert.DeserializeObject<user>(body);//反序列
            string username = userjson.username;
            string password = userjson.password;  
            string Token = ""; //令牌
            Tokenstr Tokenstr = new Tokenstr();
            tokendata tokendatastr = new tokendata();
            SqlHelper.connstr = sqlcon.consql();
            string sql = "select * from web_hr_users where worknu='" + username + "'and password='" +  password + "'"; 
            DataTable user = SqlHelper.GetTable(sql);
            if (user.Rows.Count > 0)
            {
                var signature = user.Rows[0]["worknu"].ToString() + "-"+ user.Rows[0]["name"].ToString() + "-" + user.Rows[0]["roles"].ToString() + "-" + user.Rows[0]["avatar"].ToString();
                Token = TokenHelper.Generate(signature);
                HttpContext.Session.SetString( Token, signature); 
                Tokenstr.code = 20000;

                tokendatastr.token= Token; 
                Tokenstr.data = tokendatastr;
                Tokenstr.msg = "返回令牌成功";
             
            }
            else
            {

                Tokenstr.code = 20001; 
                tokendatastr.token = "";
                Tokenstr.data = tokendatastr;
                Tokenstr.msg= "返回令牌失败";
            }

            return JsonConvert.SerializeObject(Tokenstr); //返回 令牌字典json
        }
        //=============================================================================================== post user info

        /// <summary>
        ///  POST:aapi/users/getinfo 获取用户信息 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetInfo", Name = "GetInfo")]
        public string getInfopost()//获取用户信息
        {
            userinfo info = new userinfo();
            userdata infodata = new userdata();
            string Token =  Request.Headers["X-Token"];
            var signature = TokenHelper.TokenToSignature(Token);
            string Sessionsignature = HttpContext.Session.GetString(Token);
            if (Sessionsignature == signature)
            {
                SqlHelper.connstr = sqlcon.consql(); 
                string[] userinfo = signature.Split('-');
                string sql = "select * from web_hr_users where worknu='" + userinfo[0] + "'";
                DataTable user = SqlHelper.GetTable(sql); 
                 info.code = 20000;
                infodata.name = user.Rows[0]["name"].ToString();
                infodata.roles = user.Rows[0]["roles"].ToString();
                infodata.avatar = user.Rows[0]["avatar"].ToString();
                info.data = infodata;
            }
            else
            {
            
                info.code = 50008;//非法令牌
                infodata.name = "";
                infodata.roles = "";
                infodata.avatar = "";
                info.data = infodata;
            }

             
                return JsonConvert.SerializeObject(info); //返回 令牌字典json
        }

        //===============================================================================================
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout", Name = "Logout")]
        public string Logoutpost()//获取用户信息
        {
            string Token = Request.Headers["X-Token"];
            HttpContext.Session.Remove(Token);
            Logout userout = new Logout();
            userout.code = 20000;
            userout.data = "success";
            return JsonConvert.SerializeObject(userout);

        }

       /// <summary>
       /// put
       /// </summary>
       /// <param name="id">id</param>
       /// <param name="value">传参</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        ///    DELETE: api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
