using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardLanguageController : ControllerBase
    {
        EduManDBContext context = null;
        public StandardLanguageController(EduManDBContext ctx)
        {
            context = ctx;
        }

        

    }
}
