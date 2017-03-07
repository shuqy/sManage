

create table ZhihuAnswer(
	Id int identity(1,1) primary key,
	QuestionId varchar(50) not null,
	AnswerId varchar(50) not null,
	Question nvarchar(500) null,
	Author nvarchar(200) null,
	Bio nvarchar(500) null,
	Summary nvarchar(2000) null,
	Content nvarchar(max) null,
	ZanCount int null,
	Recommended bit default 0,
	ViewCount int null,
	CreatedOn datetime default getdate(),
	Deleted bit default 0,
)