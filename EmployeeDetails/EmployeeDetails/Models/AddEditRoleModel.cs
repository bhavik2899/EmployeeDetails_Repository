using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeDetails.Models
{
    public class AddEditRoleModel
    {
        public RoleClass Role { get; set; }

        public List<PrivilegeClass> PrivilegeList { get; set; }
    }

    public class RoleClass
    {
        public long RoleId { get; set; }
        [DisplayName("Role Name :")]
        [Required(ErrorMessage = "Please Enter Role Name.")]
        public string RoleName { get; set; }
        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PrivilegeClass
    {
        public long PrivilegeId { get; set; }

        public string PrivilegeName { get; set; }

        public bool IsView { get; set; }

        public bool IsCreate { get; set; }

        public bool IsUpdate { get; set; }

        public bool IsDelete { get; set; }
    }
}