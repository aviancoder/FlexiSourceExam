using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opg_201910_interview.Models
{
    public class CustomerFile
    {
        public string CustomerFileName { get; set; }
        public string CategoryName { get; set; }
        public int CategoryOrderNumber { get; set; }
        public DateTime CustomerFileDate { get; set; }
    }
}
