
create table ZhihuAnswer(
	Id int identity(1,1) primary key,
	QuestionId varchar(100),
	AnswerId varchar(100),
	Question nvarchar(max),
	Author nvarchar(200),
	Bio nvarchar(1000),
	Summary nvarchar(Max),
	Content nvarchar(Max),
	ZanCount int,
	Recommended bit not null,
	ViewCount int default 0,
	CreatedOn datetime default getdate(),
	deleted bit not null default 0,
)