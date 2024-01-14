using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class EmployeeController : ControllerBase
    {
        EduManDBContext context = null;
        public EmployeeController(EduManDBContext ctx)
        {
            context = ctx;
        }
    }
}
