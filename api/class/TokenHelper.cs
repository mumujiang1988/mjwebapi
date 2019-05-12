using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

 

    /*
     * 安装包
      Install-Package Microsoft.AspNetCore.Session
      Install-Package Microsoft.AspNetCore.Http.Extensions


      *Startup类中添加启用session服务
      public void ConfigureServices(IServiceCollection services)
{
    services.AddSession();// 添加session服务
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }

    app.UseSession();// 启用session
    app.UseHttpsRedirection();
    app.UseMvc();
}


     * 生成Token
     var signature = "admin";
     var token = TokenHelper.Generate(signature);
     HttpContext.Session.SetString(signature, token);
     Token验证
      var signature = TokenHelper.TokenToSignature(token);
 HttpContext.Session.GetString(signature) == token;

          */

    public static class TokenHelper
        {
            public static string Generate(string signature)
            {
                if (string.IsNullOrWhiteSpace(signature))
                    return "";

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
                var nonce = new Random().Next(10, 100).ToString();

                var arr = new string[] { signature, nonce, timestamp };
                Array.Sort(arr);

                var str = string.Join("", arr);
                return Base64Helper.Base64Encode(str);
            }

            public static string TokenToSignature(string token)
            {
                if (string.IsNullOrWhiteSpace(token))
                    return "";

                var means = 14;// (timestamp + nonce) lenght = 14
                var clearText = Base64Helper.Base64Decode(token);
                return clearText.Substring(means);
            }
         
}
