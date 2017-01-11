--ÐÐÒµ
create table T_Industry(
	Id int identity(1,1) primary key,
	Name nvarchar(100) not null,
	Deleted bit not null default 0,
)


create table T_Stock(
	Id int identity(1,1) primary key,
	StockCode varchar(50) not null,
	FullCode varchar(50) not null,
	StockName nvarchar(100) not null,
	TotalAmount decimal(18,2) default 0.00,
	TotalMarketValue decimal(18,2) default 0.00,
	CirculationMarketValue decimal(18,2) default 0.00,
	Industry int not null,
	PyAbbre varchar(50) not null,
	PyFullName varchar(100) not null,
	CreatedOn datetime default getdate(),
	UpdatedOn datetime,
	Deleted bit default 0,
)