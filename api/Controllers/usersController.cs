using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    /// <summary>
    /// post 用户信息
    /// </summary>
    public class userinfo  
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
    /// <summary>
    /// 令牌实体类
    /// </summary>
    public class Tokenstr
    {
        /// <summary>
        ///  状态码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        ///返回令牌数据
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }

    }


    #endregion

    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        /// <summary>
        /// GET: api/users
        /// </summary>
        /// <returns></returns>
         
        [HttpGet]
        public  string  Get()
        {
            user userjson = new user();
             userjson.username="stt";
            userjson.password= "stt";
            return JsonConvert.SerializeObject(userjson); ;
        }

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
            string sql = "select * from web_hr_users where id ='"+id+"'";
            //SqlHelper.connstr = "Data Source=.SQLEXPRESS;Initial Catalog=MjWeb;User ID=sa;Password =junsu";
            SqlHelper.connstr = "Data Source =.\\SQLEXPRESS; Initial Catalog = MjWeb; User ID = sa; Password = junsu";
            DataTable user = SqlHelper.GetTable(sql);
            return JsonConvert.SerializeObject(user);
            //return "cs";
        }

        /// <summary>
        ///  POST:api/users/logon 登录处理 返回令牌 
        /// </summary>
        /// <remarks> 
        /// 登录处理 返回令牌 
        /// </remarks>
        /// <response code="200">返回令牌 </response> 
        /// <response code="201">返回失败</response>  
        /// <returns>返回令牌 </returns>

        [HttpPost("logon", Name = "logonPost")]
        public string logonPost()//登录
        {
            string body = new StreamReader(Request.Body).ReadToEnd();
            //Console.WriteLine(userjson);
              user userjson = JsonConvert.DeserializeObject<user>(body);//反序列
            string username = userjson.username;
            string password = userjson.password;  
            string Token = ""; //令牌
            Tokenstr Tokenstr = new Tokenstr();
            string sql = "select * from web_hr_users where 工号='" + username + "'and 密码='" +  password + "'";
            SqlHelper.connstr = "Data Source=localhost;Initial Catalog=MjWeb;User ID=sa;Password =junsu";
            DataTable user = SqlHelper.GetTable(sql);
            if (user.Rows.Count > 0)
            {
                var signature = user.Rows[0]["工号"].ToString() + "-" + user.Rows[0]["roles"].ToString();
                Token = TokenHelper.Generate(signature);
                HttpContext.Session.SetString(signature, Token);

                Tokenstr.code = "20000";
                Tokenstr.data = Token;
                Tokenstr.msg = "返回令牌成功";

            }
            else
            {

                Tokenstr.code = "20001";
                Tokenstr.data = Token;
                Tokenstr.msg= "返回令牌失败";
            }

            return JsonConvert.SerializeObject(Tokenstr); //返回 令牌字典json
        }

        /// <summary>
        ///  POST:aapi/users/getinfo 获取用户信息 
        /// </summary>
        /// <returns></returns>
        [HttpPost("getinfo", Name = "GetInfo")]
        public string getInfopost()//获取用户信息
        {
            //string body = new StreamReader(Request.Headers).ReadToEnd();
            string h=  Request.Headers["X-Token"];
            //Console.WriteLine(userjson);
           // user userjson = JsonConvert.DeserializeObject<user>(body);//反序列



         //   userinfo info = new userinfo();
         //   info.name =;
         //   info.roles =;
         //   info.avatar =;

            return JsonConvert.SerializeObject(h); //返回 令牌字典json
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
