drop table turn_in_out_record
create table turn_in_out_record(
	Id int identity(1,1) primary key,
	UserId int,
	[Money] decimal(18,2) not null,--操作金额
	OperationDate datetime not null,--操作日期
	CreatedOn datetime not null default getdate(),
	Deleted bit not null default 0,
)
drop table stock_exchange_record
create table stock_exchange_record(
	Id int identity(1,1) primary key,
	UserId int,
	StockCode varchar(20) not null,
	StockName nvarchar(20) null,
	BuyPrice decimal(18,5) not null,
	SellPrice decimal(18,5) not null,
	Quantity int not null,
	BuyDate datetime not null,--
	SellDate datetime not null,--
	Profit decimal(18,5) null,--盈亏金额
	CreatedOn datetime not null default getdate(),
	Deleted bit not null default 0,
)
drop table water_bill
create table water_bill(
	Id int identity(1,1) primary key,
	UserId int not null,
	BMonthMoney decimal(18,5) not null,
	TurnInMoney decimal(18,5) not null,
	TurnOutMoney decimal(18,5) not null,
	ProfitMoney decimal(18,5) not null,	
	EMonthMoney decimal(18,5) not null,
	[Date] datetime not null,
	UpdatedOn datetime not null,
)


CREATE TABLE [stock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StockCode] [varchar](100) NULL,
	[StockName] [nvarchar](100) NULL,
	[PyFullName] [varchar](100) NULL,
	[PyAbbre] [varchar](100) NULL,
)
