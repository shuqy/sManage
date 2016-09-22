use [SQY_Manage]
go
create table sys_user_menu(
	Id int identity(1,1) primary key,
	Title nvarchar(200) not null,
	STitle nvarchar(200),
	[Description] nvarchar(500),
	ParentId int,
	ControllName varchar(100),
	ActionName varchar(100),
	SortValue int ,
	[State] bit not null,
	IsPath bit,
	ShowInMenu bit default 0,
	Code varchar(200),
	Deleted bit default 0,
)
go
create table sys_user_group(
	Id int identity(1,1) primary key,
	Title nvarchar(200),
	[Description] nvarchar(500),
	Code varchar(200),
	[State] bit not null,
	SortValue int default 0,
	IsFree int,
	Deleted bit default 0,
)
go
CREATE TABLE sys_user(
	[Id] [int] IDENTITY(1,1) primary key NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[UserCode] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Mobile] [varchar](50) NULL,
	[PassCode] [varchar](100) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int],
	[State] int,
	[Deleted] [bit] NOT NULL,
)
go
create table user_group_mapping(
	Id int identity(1,1) primary key,
	UserId int not null,
	GroupId int not null,
)
go
create table group_menu_mapping(
	Id int identity(1,1) primary key,
	GroupId int not null,
	MenuID int not null,
)
go
--建立外键约束
alter table user_group_mapping add constraint usergroup_groupid_cons foreign key (GroupId) references sys_user_group
alter table user_group_mapping add constraint usergroup_userid_cons foreign key (UserId) references sys_user
alter table group_menu_mapping add constraint groupmenu_menuid_cons foreign key (MenuID) references sys_user_menu
alter table group_menu_mapping add constraint groupmenu_groupid_cons foreign key (GroupId) references sys_user_group
go