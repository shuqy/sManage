
create table ZhihuReadHistory(
	Id int identity(1,1) primary key,
	CustomerId int,
	ZhihuId int,
	CreatedOn datetime not null,
	Deleted bit not null,
)