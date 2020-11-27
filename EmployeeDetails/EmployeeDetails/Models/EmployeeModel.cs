using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeDetails.Models
{
    public class EmployeeModel
    {
        public long Id { get; set; }

        [DisplayName("Employee Name :")]
        [Required(ErrorMessage = "Please Enter Employee Name.")]
        public string Name { get; set; }

        [DisplayName("Salary :")]
        [Required(ErrorMessage = "Please Enter Salary.")]
        public decimal Salary { get; set; }

        [DisplayName("Department Name :")]
        [Required(ErrorMessage = "Please Enter Department Name.")]
        public long DepId { get; set; }

        [DisplayName("Joining Date :")]
        [Required(ErrorMessage = "Please Enter Joining Date.")]
        public string JoiningDate { get; set; }

        public IEnumerable<SelectListItem> DepList { get; set; }

        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }

    }
}