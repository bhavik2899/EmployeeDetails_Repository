using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EmployeeDetails.Models
{
    public class LoginModel
    {
        public long LoginID { get; set; }

        [DisplayName("UserName :")]
        public string UserName { get; set; }

        [DisplayName("Password :")]
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid First Name")]
        public string Password { get; set; }
        public string FNmae { get; set; }
        public string LName { get; set; }
        public string MobileNo { get; set; }
    }
}