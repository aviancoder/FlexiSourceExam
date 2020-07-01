using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using opg_201910_interview.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace opg_201910_interview.CustomLogic
{
    public class CustomerFilesLogic
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CustomerFilesLogic(IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<CustomerFile> GetCustomerFiles(int _customerID)
        {
            List<CustomerFile> retval = new List<CustomerFile>();

            if (_customerID > 0)
            {
                var clientSettings = _config.GetSection("ClientSettings").GetChildren();
                var client = clientSettings.Where(x => int.Parse(x.GetSection("ClientID").Value) == _customerID);

                if (client != null)
                {
                    string fileDirectoryPath = client.First().GetSection("FileDirectoryPath").Value;
                    var pcategories = client.First().GetSection("Categories").AsEnumerable();
                   
                    List<string> categories = (from kvp in pcategories select kvp.Value).Where(x => String.IsNullOrEmpty(x) == false).Distinct().ToList();
                    List<string> ordercategories = new List<string>();
                    for (int i = categories.Count - 1; i >= 0; i--)
                        ordercategories.Add(categories[i]);

                    var path = Path.Combine(_hostingEnvironment.WebRootPath, fileDirectoryPath.Replace("/", @"\"));
                    
                    string[] files = Directory.GetFiles(path);
                    foreach(var obj in files)
                    {
                        // get specific file
                        FileInfo fileinfo = new FileInfo(obj);
                        CustomerFile file = new CustomerFile();

                        // check if the segments count is correct
                        // aternatively the fomat checking can be done using regex
                        string fileName = Path.GetFileNameWithoutExtension(fileinfo.Name);
                        file.CustomerFileName = fileName;
                        bool validFileFormat = false;

                        string[] splittedFileName = fileName.Split(@"-");

                        if (splittedFileName.Length == 4)
                        {
                            DateTime tempDate;
                            if (DateTime.TryParseExact(splittedFileName[1] + splittedFileName[2] + splittedFileName[3],  "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out tempDate))
                            {
                                file.CustomerFileDate = tempDate;
                                string tempcategory = splittedFileName[0];
                                int index = ordercategories.FindIndex(a => String.Equals(a.ToUpper(), tempcategory.ToUpper()) == true);
                                if (index >= 0)
                                {
                                    file.CategoryName = tempcategory;
                                    file.CategoryOrderNumber = index;

                                    validFileFormat = true;
                                }
                            }
                        }

                        if (validFileFormat == false)
                        {
                            splittedFileName = fileName.Split(@"_");
                            if (splittedFileName.Length == 2)
                            {
                                DateTime tempDate;
                                if (DateTime.TryParseExact(splittedFileName[1], "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out tempDate))
                                {
                                    file.CustomerFileDate = tempDate;
                                    string tempcategory = splittedFileName[0];
                                    int index = ordercategories.FindIndex(a => String.Equals(a.ToUpper(), tempcategory.ToUpper()) == true);
                                    if (index >= 0)
                                    {
                                        file.CategoryName = tempcategory;
                                        file.CategoryOrderNumber = index;

                                        validFileFormat = true;
                                    }
                                }
                            }
                        }

                        if (validFileFormat)
                            retval.Add(file);
                    }
                }
            }


            return retval;
        }
    }
}
