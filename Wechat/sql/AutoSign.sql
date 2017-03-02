--mans用户
Create table MansUser(
	Id int identity(1,1) primary key,
	LeShareUserName varchar(50) null,
	RealName nvarchar(200) null,
	Mobile varchar(50) null,
	Email varchar(50) null,
	HeadImg varchar(200) null,
	LeSharePwd varchar(200) null,
	IsAutoSign bit null,
	Deleted bit null default 0,
)
--自动签到用户
Create table AutoSignUser(
	Id int identity (1,1) primary key,
	MansUserId int not null,
	NextSignDateTime datetime not null,
	NextSignType int not null,
	SuccessTimes int default 0,

)
--自动签到历史纪录
create table AutoSignHistory(
	Id int identity (1,1) primary key,
	MansUserId int not null,
	SignDateTime datetime not null,
	SignType int not null,
	CreatedDate datetime default getdate(),
	Deleted bit default 0,
)