using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using opg_201910_interview.CustomLogic;
using opg_201910_interview.Models;

namespace opg_201910_interview
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerFilesController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CustomerFilesController(IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/customerfiles
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/customerfiles
        [HttpGet("{id}")]
        public ResponseObject Get(int id)
        {
            ResponseObject retval = new ResponseObject();

            try
            {
                CustomerFilesLogic cfl = new CustomerFilesLogic(_config, _hostingEnvironment);
                List<CustomerFile> files = cfl.GetCustomerFiles(id);
                var orderedFiles = files.OrderBy(x => x.CategoryOrderNumber).ThenBy(x => x.CustomerFileDate);

                List<string> strFiles = new List<string>();
                foreach (var file in orderedFiles)
                    strFiles.Add(file.CustomerFileName);

                retval.ReturnedValue = strFiles;
            }
            catch (Exception ex)
            {
                retval.ErrorMessage = ex.Message;
            }

            
            return retval;

        }
    }
}
