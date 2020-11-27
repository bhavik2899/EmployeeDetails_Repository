using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeDetails.Models
{
    public class EmployeesByDate
    {
        public List<sp_EmployeeListByDate_Result> empList { get; set; }


        [DisplayName("Joining Date From :")]
        public string FromDate { get; set; }

        [DisplayName("Joining Date To :")]
        public string ToDate { get; set; }

    }
}