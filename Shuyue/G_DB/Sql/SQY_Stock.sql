--��ҵ
create table T_Industry(
	Id int identity(1,1) primary key,
	Name nvarchar(100) not null,
	Deleted bit not null default 0,
)

--drop table T_Stock
create table T_Stock(
	Id int identity(1,1) primary key,
	StockCode varchar(50) not null,
	FullCode varchar(50) not null,
	StockName nvarchar(100) not null,
	TotalAmount decimal(18,4) default 0,
	TotalMarketValue decimal(18,4) default 0,
	CirculationMarketValue decimal(18,4) default 0,
	MainCount decimal(18,4) default 0,--��������
	Earning decimal(18,4) default 0,--��ӯ������
	PERatio decimal(18,4) default 0,--��ӯ��
	Industry int not null,
	IndustryName nvarchar(100),
	PyAbbre varchar(50) not null,
	PyFullName varchar(100) not null,
	CreatedOn datetime default getdate(),
	UpdatedOn datetime,
	Deleted bit default 0,
)
--�Ƿ��ѽ���
alter table T_Stock add IsCreatedTable bit default 0