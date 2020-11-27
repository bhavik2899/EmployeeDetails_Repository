using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeDetails.Models
{
    public class UserModel
    {
        public long UserID { get; set; }

        [DisplayName("User Name :")]
        [Required(ErrorMessage = "Please Enter User Name.")]
        public string UserName { get; set; }

        [DisplayName("Password :")]
        [Required(ErrorMessage = "Please Enter Password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must have at least 8 Characters Including 1 Uppercase and 1 Lowercase alphabet, 1 Number and 1 Special character.")]
        public string Password { get; set; }

        [DisplayName("First Name :")]
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string FName { get; set; }

        [DisplayName("Last Name :")]
        [Required(ErrorMessage = "Please Enter Last Name.")]
        public string LName { get; set; }

        [DisplayName("Mobile Number :")]
        public string MobileNo { get; set; }


        [DisplayName("Role Name :")]
        [Required(ErrorMessage = "Please Select Role Name.")]
        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public bool IsCreate {get;set;}
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
    }
}