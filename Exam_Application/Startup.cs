using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Exam_Application.Startup))]
namespace Exam_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
