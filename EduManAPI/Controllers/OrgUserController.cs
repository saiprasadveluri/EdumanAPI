using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgUserController : ControllerBase
    {
        EduManDBContext context = null;
        private IConfiguration _config;
        public OrgUserController(EduManDBContext ctx, IConfiguration config)
        {
            _config = config;
            context = ctx;
        }

        
    }
}
