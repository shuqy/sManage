
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](200) NULL,
	[Name] [nvarchar](500) NOT NULL,
	[RealName] [nvarchar](50) NULL,
	[Gender] [nvarchar](1) NULL,
	[Age] [int] NULL,
	[WeixinId] [nvarchar](200) NULL,
	[Phone] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[HeadImg] [nvarchar](max) NULL,
	[CustomerId] [int] NULL,
	[Status] [int] NOT NULL,
	[SupervisorCustomerId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
