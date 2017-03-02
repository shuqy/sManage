

create table Customer_MansUser_Mapping(
	Id int identity(1,1) primary key,
	CustomerId int not null,
	MansUserId int not null,
)