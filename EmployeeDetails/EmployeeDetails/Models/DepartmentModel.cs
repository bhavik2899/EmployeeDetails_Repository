using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeDetails.Models
{
    public class DepartmentModel
    {
        public long Id { get; set; }

        [DisplayName("Department Name :")]
        [Required(ErrorMessage = "Please Enter Department Name.")]
        public string DepName { get; set; }
        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
    }
}