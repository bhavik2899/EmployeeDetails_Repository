----------------------------------------------
CREATE TABLE Employees 
  ( 
     Id   BIGINT IDENTITY(1, 1) NOT NULL, 
     NAME NVARCHAR(50) NOT NULL,
	 Salary MONEY NOT NULL,
	 DepId BIGINT NOT NULL,
     PRIMARY KEY (Id),
	 FOREIGN KEY (DepId) REFERENCES Departments(ID)
  ) 
  ALTER TABLE Employees ADD JoiningDate DATE NOT NULL

  CREATE TABLE Departments
  ( 
     Id   BIGINT IDENTITY(1, 1) NOT NULL, 
     DepName VARCHAR(100) NOT NULL, 
     PRIMARY KEY (Id) 
  ) 

INSERT INTO Employees
VALUES('Emp7',20000,1,'2019-06-04')
		
INSERT INTO Departments
VALUES
('Dep5')

---------------------------------------------

CREATE TABLE Users 
(
	PK_UserID   BIGINT IDENTITY(1, 1) NOT NULL, 
    UserName NVARCHAR(50) NOT NULL,
	Password NVARCHAR(50) NOT NULL,
	FNmae NVARCHAR(30) NOT NULL,
	LName NVARCHAR(30) NOT NULL,
	MobileNo NVARCHAR(15),
    PRIMARY KEY (PK_UserID)
)

CREATE TABLE Privileges 
(
	PK_PrivilegeID   BIGINT IDENTITY(1, 1) NOT NULL, 
    PrivilegeName NVARCHAR(50) NOT NULL,
	MenuName NVARCHAR(50),
	IsPage bit,
	IsFunction bit,
    PRIMARY KEY (PK_PrivilegeID)
)

CREATE TABLE Roles 
(
	PK_RoleID   BIGINT IDENTITY(1, 1) NOT NULL, 
    RoleName NVARCHAR(50) NOT NULL,
    PRIMARY KEY (PK_RoleID)
)

CREATE TABLE UserRole 
(
	PK_UserRoleID   BIGINT IDENTITY(1, 1) NOT NULL, 
    FK_UserID BIGINT NOT NULL,
	FK_RoleID BIGINT NOT NULL,
    PRIMARY KEY (PK_UserRoleID),
	FOREIGN KEY (FK_UserID) REFERENCES Users(pk_UserID),
	FOREIGN KEY (FK_RoleID) REFERENCES Roles(PK_RoleID)
)

CREATE TABLE UserRolePrivilege
(
	PK_UserRolePrivilegeID   BIGINT IDENTITY(1, 1) NOT NULL, 
    FK_RoleID BIGINT NOT NULL,
	FK_PrivilegeID BIGINT NOT NULL,
	IsView bit NOT NULL,
	IsCreate bit NOT NULL,
	IsUpdate bit NOT NULL,
	IsDelete bit NOT NULL,
    PRIMARY KEY (PK_UserRolePrivilegeID),
	FOREIGN KEY (FK_RoleID) REFERENCES Roles(pk_RoleID),
	FOREIGN KEY (FK_PrivilegeID) REFERENCES Privileges(PK_PrivilegeID)
)

INSERT INTO Users 
VALUES('bhavik@gmail.com','pass1','Bhavik','Thummar','9987654321')

INSERT INTO Roles
VALUES
('Admin'),
('user')

INSERT INTO UserRole
VALUES(1,1)

INSERT INTO Privileges
VALUES
('Role','Role',1,0),
('User','USer',1,0)

INSERT INTO UserRolePrivilege
VALUES
(2,3,1,1,1,1)
