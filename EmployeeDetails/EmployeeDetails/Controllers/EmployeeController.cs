using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using EmployeeDetails.Filter;
using EmployeeDetails.Models;

namespace EmployeeDetails.Controllers
{
	public class EmployeeController : Controller
	{
		EmployeeDetailsEntities _Context = new EmployeeDetailsEntities();

		public ActionResult Login()
		{
			Session.Clear();
			return View();
		}

		[HttpPost]
		public ActionResult Login(LoginModel lm)
		{
			var userDetails = _Context.Users.Where(x => x.UserName == lm.UserName && x.Password == lm.Password).Select(x => x).FirstOrDefault();

			if (userDetails != null)
			{
				Session["UserName"] = userDetails.UserName;
				Session["UserID"] = userDetails.PK_UserID;
				Session["RoleId"] = _Context.UserRoles.Where(x => x.FK_UserID == userDetails.PK_UserID).Select(x => x.FK_RoleID).FirstOrDefault();
				return RedirectToAction("ListEmployee");
			}
			else
			{
				ModelState.AddModelError("", "Invalid User Name or Password.");
				return View(lm);
			}
		}
		
		[CustomAuthenticationFilter]
		[CustomAuthorize("Role", "View")]
		public ActionResult RoleList()
		{
			var roleList = new List<RoleClass>();
			var roles = _Context.Roles.Select(x => x).ToList();
			foreach (var item in roles)
			{
				var rm = new RoleClass();
				rm.RoleId = item.PK_RoleID;
				rm.RoleName = item.RoleName;
				roleList.Add(rm);
			}

			TempData["IsCreate"] = HttpContext.Items["IsCreate"];
			TempData["IsUpdate"] = HttpContext.Items["IsUpdate"];
			TempData["IsDelete"] = HttpContext.Items["IsDelete"];

			return View(roleList);
		}

		[CustomAuthorize("Role", "Create/Update")]
		public ActionResult AddEditRole(int id = 0)
		{
			var aerm = new AddEditRoleModel();
			RoleClass Role = new RoleClass();
			var PrivilegeList = new List<PrivilegeClass>();

			if (id != 0)
			{
				var roleDetail = _Context.Roles.Where(x => x.PK_RoleID == id).Select(x => x).FirstOrDefault();
				Role.RoleName = roleDetail.RoleName;
				Role.RoleId = roleDetail.PK_RoleID;
				aerm.Role = Role;
				var userRolePrivileges = _Context.UserRolePrivileges.Where(x => x.FK_RoleID == id).Select(x => x).ToList();
				var privileges = _Context.Privileges.Select(x => new { x.PK_PrivilegeID, x.PrivilegeName }).ToList();
				foreach (var item in userRolePrivileges)
				{
					var previlege = new PrivilegeClass();
					previlege.PrivilegeId = item.FK_PrivilegeID;
					previlege.PrivilegeName = privileges.Where(x => x.PK_PrivilegeID == item.FK_PrivilegeID).Select(x => x.PrivilegeName).FirstOrDefault();
					previlege.IsView = item.IsView;
					previlege.IsCreate = item.IsCreate;
					previlege.IsUpdate = item.IsUpdate;
					previlege.IsDelete = item.IsDelete;

					PrivilegeList.Add(previlege);
				}
				aerm.PrivilegeList = PrivilegeList;
			}
			else
			{
				var privileges = _Context.Privileges.Select(x => x).ToList();
				foreach (var item in privileges)
				{
					var previlege = new PrivilegeClass();
					previlege.PrivilegeId = item.PK_PrivilegeID;
					previlege.PrivilegeName = item.PrivilegeName;
					PrivilegeList.Add(previlege);
				}
				aerm.PrivilegeList = PrivilegeList;
			}
			return View(aerm);
		}

		[HttpPost]
		public ActionResult AddEditRole(AddEditRoleModel aerm)
		{
			XDocument AddEditRoles = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
			 new XElement("UserRolePrivilegeList",
				 from PList in aerm.PrivilegeList
				 select new XElement("userRolePrililege",
				 new XElement("RoleID", aerm.Role.RoleId),
				 new XElement("RoleName", aerm.Role.RoleName),
				 new XElement("PrivilegeID", PList.PrivilegeId),
				 new XElement("IsView", PList.IsView),
				 new XElement("ISCreate", PList.IsCreate),
				 new XElement("IsUpdate", PList.IsUpdate),
				 new XElement("IsDelete", PList.IsDelete)))
			 );
			var abc = AddEditRoles.ToString();
			_Context.Sp_RolePrivilegeWXml(abc);

			return RedirectToAction("RoleList");
		}

		[CustomAuthorize("Role", "Delete")]
		public ActionResult DeleteRole(int id = 0)
		{
			try
			{
				var deleteUserRole = _Context.UserRoles.Where(x => x.FK_RoleID == id).Select(x => x).ToList();
				foreach(var item in deleteUserRole)
				{
					_Context.UserRoles.Remove(item);
				}
				_Context.SaveChanges();
			}
			catch { }

			try
			{
				var deleteUserRolePrivilege = _Context.UserRolePrivileges.Where(x => x.FK_RoleID == id).Select(x => x).ToList();
				foreach(var item in deleteUserRolePrivilege)
				{
					_Context.UserRolePrivileges.Remove(item);
				}
				_Context.SaveChanges();
			}
			catch { }

			var deleteRole = _Context.Roles.Where(x => x.PK_RoleID == id).Select(x => x).SingleOrDefault();
			_Context.Roles.Remove(deleteRole);
			_Context.SaveChanges();

			return RedirectToAction("RoleList");
		}

		[CustomAuthenticationFilter]
		[CustomAuthorize("User", "View")]
		public ActionResult UserList()
		{
			var userList = new List<UserModel>();
			var users = _Context.Users.Select(x => x).ToList();
			foreach (var item in users)
			{
				var um = new UserModel();
				um.UserID = item.PK_UserID;
				um.UserName = item.UserName;
				um.FName = item.FNmae;
				um.LName = item.LName;
				um.MobileNo = item.MobileNo;
				var roleID = _Context.UserRoles.Where(x => x.FK_UserID == item.PK_UserID).Select(x => x.FK_RoleID).FirstOrDefault();
				um.RoleId = roleID;
				um.RoleName = _Context.Roles.Where(x => x.PK_RoleID == roleID).Select(x => x.RoleName).SingleOrDefault();
				userList.Add(um);
			}

			TempData["IsCreate"] = HttpContext.Items["IsCreate"];
			TempData["IsUpdate"] = HttpContext.Items["IsUpdate"];
			TempData["IsDelete"] = HttpContext.Items["IsDelete"];

			return View(userList);
		}

		[CustomAuthorize("User", "Create/Update")]
		public ActionResult AddEditUser(int id = 0)
		{
			var user = new UserModel();

			user.RoleList = _Context.Roles.Select(c => new SelectListItem
			{
				Value = c.PK_RoleID.ToString(),
				Text = c.RoleName
			});

			if (id != 0)
			{
				var EditUser = _Context.Users.Where(x => x.PK_UserID == id).Select(x => x).SingleOrDefault();

				user.UserID = EditUser.PK_UserID;
				user.UserName = EditUser.UserName;
				user.Password = EditUser.Password;
				user.FName = EditUser.FNmae;
				user.LName = EditUser.LName;
				user.MobileNo = EditUser.MobileNo;
				user.RoleId = _Context.UserRoles.Where(x => x.FK_UserID == id).Select(x => x.FK_RoleID).FirstOrDefault();
			}
			return View(user);
		}

		[HttpPost]
		public ActionResult AddEditUser(UserModel um)
		{
			if (um.UserID != 0 && ModelState.IsValid == false) { return View(um); }
			else
			{
				_Context.Sp_AddEditUser(um.UserID, um.UserName, um.Password, um.FName, um.LName, um.MobileNo, um.RoleId);
			}
			return RedirectToAction("UserList");
		}

		[CustomAuthorize("User", "Delete")]
		public ActionResult DeleteUser(int id = 0)
		{
			try 
			{
				var DeleteUserRole = _Context.UserRoles.Where(x => x.FK_UserID == id).Select(x => x).SingleOrDefault();
				_Context.UserRoles.Remove(DeleteUserRole);
				_Context.SaveChanges();
			}
			catch { }

			var DeleteUser = _Context.Users.Where(x => x.PK_UserID == id).Select(x => x).SingleOrDefault();
			_Context.Users.Remove(DeleteUser);
			_Context.SaveChanges();

			return RedirectToAction("UserList");
		}

		[CustomAuthorize("Employee", "View")]
		public ActionResult ListEmployee()
		{
			var employeesByDate = new EmployeesByDate();

			DateTime joiningFrom = DateTime.MinValue;
			DateTime joiningTo = DateTime.Now;
			employeesByDate.empList = _Context.sp_EmployeeListByDate(joiningFrom, joiningTo).ToList();

			TempData["IsCreate"] = HttpContext.Items["IsCreate"];
			TempData["IsUpdate"] = HttpContext.Items["IsUpdate"];
			TempData["IsDelete"] = HttpContext.Items["IsDelete"];

			return View(employeesByDate);
		}

		[HttpPost]
		[CustomAuthorize("Employee", "View")]
		public ActionResult ListEmployee(EmployeesByDate employeesByDate)
		{
			DateTime joiningFrom = DateTime.MinValue;
			DateTime joiningTo = DateTime.Now;

			if (employeesByDate.FromDate != null) { joiningFrom = Convert.ToDateTime(employeesByDate.FromDate); }
			if (employeesByDate.ToDate != null) { joiningTo = Convert.ToDateTime(employeesByDate.ToDate); }
			employeesByDate.empList = _Context.sp_EmployeeListByDate(joiningFrom, joiningTo).ToList();

			return View(employeesByDate);
		}

		[CustomAuthorize("Employee", "Create/Update")]
		public ActionResult EditEmployee(int id = 0)
		{
			var employee = new EmployeeModel();

			employee.DepList = _Context.Departments.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.DepName
			});

			if (id != 0)
			{
				var EditEmployee = _Context.Employees.Where(x => x.Id == id).Select(x => x).SingleOrDefault();

				employee.Id = EditEmployee.Id;
				employee.Name = EditEmployee.NAME;
				employee.Salary = EditEmployee.Salary;
				employee.DepId = EditEmployee.DepId;
				employee.JoiningDate = EditEmployee.JoiningDate.ToShortDateString().ToString();
			}
			return View(employee);
		}

		[HttpPost]
		public ActionResult EditEmployee(EmployeeModel employeeModel)
		{
			if (employeeModel.Id != 0)
			{
				var EditEmployee = _Context.Employees.Where(x => x.Id == employeeModel.Id).Select(x => x).SingleOrDefault();

				EditEmployee.NAME = employeeModel.Name;
				EditEmployee.Salary = employeeModel.Salary;
				EditEmployee.DepId = employeeModel.DepId;
				EditEmployee.JoiningDate = Convert.ToDateTime(employeeModel.JoiningDate);
				_Context.SaveChanges();
			}
			else
			{
				if (!ModelState.IsValid) { return View(employeeModel); }

				var employee = new Employee();

				employee.NAME = employeeModel.Name;
				employee.Salary = employeeModel.Salary;
				employee.DepId = employeeModel.DepId;
				employee.JoiningDate = Convert.ToDateTime(employeeModel.JoiningDate);
				_Context.Employees.Add(employee);
				_Context.SaveChanges();
			}
			return RedirectToAction("ListEmployee");
		}

		[CustomAuthorize("Employee", "Delete")]
		public ActionResult DeleteEmployee(int id = 0)
		{
			var DeleteEmployee = _Context.Employees.Where(x => x.Id == id).Select(x => x).SingleOrDefault();
			_Context.Employees.Remove(DeleteEmployee);
			_Context.SaveChanges();

			return RedirectToAction("ListEmployee");
		}

		[CustomAuthorize("Department", "View")]
		public ActionResult Department()
		{
			var emplist = _Context.Departments.Include("Employees").ToList();

			TempData["IsCreate"] = HttpContext.Items["IsCreate"];
			TempData["IsUpdate"] = HttpContext.Items["IsUpdate"];
			TempData["IsDelete"] = HttpContext.Items["IsDelete"];

			return View(emplist);
		}

		[CustomAuthorize("Department", "Create/Update")]
		public ActionResult EditDepartment(int id = 0)
		{
			var department = new DepartmentModel();

			if (id != 0)
			{
				var EditDepartment = _Context.Departments.Where(x => x.Id == id).Select(x => x).SingleOrDefault();

				department.Id = EditDepartment.Id;
				department.DepName = EditDepartment.DepName;
			}
			return View(department);
		}

		[HttpPost]
		public ActionResult EditDepartment(DepartmentModel departmentModel)
		{
			if (departmentModel.Id != 0)
			{
				var EditDepartment = _Context.Departments.Where(x => x.Id == departmentModel.Id).Select(x => x).SingleOrDefault();

				EditDepartment.DepName = departmentModel.DepName;
				_Context.SaveChanges();
			}
			else
			{
				if (!ModelState.IsValid) { return View(departmentModel); }

				var department = new Department();

				department.DepName = departmentModel.DepName;
				_Context.Departments.Add(department);
				_Context.SaveChanges();
			}
			return RedirectToAction("Department");
		}

		[CustomAuthorize("Department", "Delete")]
		public ActionResult DeleteDepartment(int id = 0)
		{
			var Employees = _Context.Employees.Where(x => x.DepId == id).Select(x => x).ToList();

			foreach (var item in Employees)
			{
				_Context.Employees.Remove(item);
				_Context.SaveChanges();
			}

			var DeleteDepartment = _Context.Departments.Where(x => x.Id == id).Select(x => x).SingleOrDefault();
			_Context.Departments.Remove(DeleteDepartment);
			_Context.SaveChanges();

			return RedirectToAction("Department");
		}

		[CustomAuthorize("Employee", "View")]
		public ActionResult H_Salary_Department()
		{
			var salaryByDep = new H_SalaryByDepModel();

			salaryByDep.empList = _Context.sp_HighestSalarybyDep().ToList();

			return View(salaryByDep);
		}

		public ActionResult UnAuthorized()
		{
			return View();
		}

		[HttpPost]
		public JsonResult ValidateUserName(string User)
		{
			bool isValid = _Context.Users.ToList().Exists(x => x.UserName.Equals(User, StringComparison.CurrentCultureIgnoreCase));
			return Json(isValid);
		}

		[HttpPost]
		public JsonResult ValidateRoleName(string Role)
		{
			bool isValid = _Context.Roles.ToList().Exists(x => x.RoleName.Equals(Role, StringComparison.CurrentCultureIgnoreCase));
			return Json(isValid);
		}
	}
}