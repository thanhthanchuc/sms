using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Owin;
using Owin;
using SMS.Models.EF;


[assembly: OwinStartup(typeof(SMS.Web.Startup))]

namespace SMS.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var config = new MapperConfiguration(cfg => {
            });
            //IMapper mapper = config.CreateMapper();
        }
    }
}
