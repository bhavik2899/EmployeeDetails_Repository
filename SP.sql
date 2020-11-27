--------------------------------------------

ALTER PROCEDURE sp_EmployeeListByDate
@fromdate date,
@todate date
AS
	SELECT Employees.Id,NAME,DepName,Salary,JoiningDate FROM Employees
	LEFT JOIN Departments
		ON Employees.DepId = Departments.Id
	WHERE Employees.JoiningDate BETWEEN @fromdate AND  @todate
GO

EXEC sp_EmployeeListByDate @fromdate = '2019-04-03', @todate ='2020-08-08'

-------------------------------------------

CREATE PROCEDURE sp_HighestSalarybyDep
AS
;WITH cteRowNum AS (
    SELECT NAME, Departments.DepName, Salary,
           DENSE_RANK() OVER(PARTITION BY Departments.Id ORDER BY Salary DESC) AS RowNum
        FROM Employees 
		LEFT JOIN Departments
		 ON Employees.DepId = Departments.Id
)
SELECT NAME,DepName ,Salary
    FROM cteRowNum
    WHERE RowNum = 1;
GO

EXEC sp_HighestSalarybyDep

----------------------------------------------------------

ALTER PROCEDURE Sp_RolePrivilegeWXml(
@XmlDoc XML
)
AS
DECLARE @role_ID BIGINT = 0;

SELECT
  xc.value('RoleID[1]', 'int') as RoleID,
  xc.value('RoleName[1]', 'nvarchar(50)') as RoleName,
  xc.value('PrivilegeID[1]', 'int') as PrivilegeID,
  xc.value('IsView[1]', 'bit') as IsView,
  xc.value('ISCreate[1]', 'bit') as IsCreate,
  xc.value('IsUpdate[1]', 'bit') as IsUpdate,
  xc.value('IsDelete[1]', 'bit') as IsDelete
  into #RolePriviegeTable
  FROM 
  @XmlDoc.nodes('/UserRolePrivilegeList/userRolePrililege') AS xt(xc)

SET @role_ID = (SELECT TOP 1 RoleID FROM #RolePriviegeTable) 

if @role_ID != 0
	BEGIN
		UPDATE Roles
			SET RoleName = t.RoleName
			FROM Roles as r
				INNER JOIN #RolePriviegeTable as t
				ON r.PK_RoleID = t.RoleID

		UPDATE UserRolePrivilege
			SET IsView = t.IsView, IsCreate = t.IsCreate, IsUpdate = t.IsUpdate , IsDelete = t.IsDelete
			FROM UserRolePrivilege as urp
				INNER JOIN #RolePriviegeTable as t
				ON urp.FK_RoleID = t.RoleID AND urp.FK_PrivilegeID = t.PrivilegeID
	END

	else
	BEGIN
		insert into Roles(RoleName) 
			select top 1 t.RoleName 
			From #RolePriviegeTable t 

		SET @role_ID = SCOPE_IDENTITY()
		
		INSERT INTO UserRolePrivilege(FK_RoleID,FK_PrivilegeID,IsView,IsCreate,IsUpdate,IsDelete)
			SELECT @role_ID,t.PrivilegeID,t.IsView,t.IsCreate,t.IsUpdate,t.IsDelete
			FROM #RolePriviegeTable t
	END

	Drop TABLE #RolePriviegeTable;
GO

------------------------------------------------------

ALTER PROCEDURE Sp_AddEditUser(
@user_Id BIGINT, 
@userName NVARCHAR(50),
@password NVARCHAR(50),
@fName NVARCHAR(30),
@lName NVARCHAR(30),
@moileNo NVARCHAR(15),
@roleId BIGINT
)
AS
	if @user_Id != 0
	BEGIN
		declare @userRoleId BIGINT = 0;

		UPDATE Users
		SET UserName = @userName , Password = @password, FNmae = @fName , LName = @lName , MobileNo = @moileNo
		WHERE PK_UserID = @user_Id

		UPDATE UserRole
		SET FK_RoleID = @roleId,  @userRoleId = PK_UserRoleID
		WHERE FK_UserID = @user_Id 

		if @userRoleId = 0
		BEGIN
			INSERT INTO UserRole VALUES(@user_Id,@roleId)
		END

	END

	else
	BEGIN
		DECLARE @userId BIGINT = null

		INSERT INTO Users VALUES(@userName,@password,@fName,@lName,@moileNo)

		SET @userId = SCOPE_IDENTITY()

		INSERT INTO UserRole 
			VALUES(@userId,@roleId)
	END

GO

EXEC Sp_AddEditUser @user_ID = 0, @userName = 'abc@gmail.com', @password = 'pass1', @fName = 'abc', @lName = 'xyz' , @moileNo= '1234567899', @roleId = 1;

------------------------------------------------------------

SELECT * FROM Users
SELECT * FROM Roles
SELECT * FROM UserRole

---------------------------------------------------------
