using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmployeeDetails.Filter
{
	public class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly string _Menuname;
		private readonly string _TypeOf;

		public CustomAuthorizeAttribute(string Menuname, string TypeOF)
		{
			_Menuname = Menuname;
			_TypeOf = TypeOF;
		}
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool authorize = false;
			var userId = Convert.ToInt64(httpContext.Session["UserId"]);
			if (userId != 0)
			{
				using (var _Context = new EmployeeDetailsEntities())
				{
					var roleId = _Context.UserRoles.Where(x => x.FK_UserID == userId).Select(x => x.FK_RoleID).FirstOrDefault();
					var privilegeId = _Context.Privileges.Where(x => x.MenuName == _Menuname).Select(x => x.PK_PrivilegeID).FirstOrDefault();
					var userRolePrivilege = _Context.UserRolePrivileges.Where(x => x.FK_RoleID == roleId && x.FK_PrivilegeID == privilegeId).Select(x => x).FirstOrDefault();

					var view = userRolePrivilege.IsView;
					var create = userRolePrivilege.IsCreate;
					var update = userRolePrivilege.IsUpdate;
					var delete = userRolePrivilege.IsDelete;

					httpContext.Items["IsCreate"] = create;
					httpContext.Items["IsUpdate"] = update;
					httpContext.Items["IsDelete"] = delete;

					if (_TypeOf == "View")
					{
						return view;
					}
					if (_TypeOf == "Create/Update")
					{
						if (httpContext.Request.QueryString.Count > 0)
						{
							return create;
						}
						else
						{
							return update;
						}
					}
					if (_TypeOf == "Delete")
					{
						return delete;
					}
				}
			}
			return authorize;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new RedirectToRouteResult(
			   new RouteValueDictionary
			   {
					{ "controller", "Employee" },
					{ "action", "UnAuthorized" }
			   });
		}
	}
}