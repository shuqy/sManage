
CREATE TABLE [dbo].[WeixinCustomer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[OpenId] [varchar](200) NOT NULL,
	[Nickname] [nvarchar](200) NULL,
	[Gender] [bit] NULL,
	[Country] [nvarchar](200) NULL,
	[Province] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[HeadImg] [varchar](500) NULL,
	[Language] [varchar](100) NULL,
	[SubscribeDate] [datetime] NULL,
	[UnSubscribed] [bit] NULL,
	[UnSubscribeDate] [datetime] NULL,
	[CreateOn] [datetime] NOT NULL,
	[ModifyOn] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
	[LastActivityTime] [datetime] NULL,
 CONSTRAINT [PK__WeixinCu__3214EC07B5C3F44A] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[WeixinCustomer] ADD  CONSTRAINT [DF__WeixinCus__Delet__1920BF5C]  DEFAULT ((0)) FOR [Deleted]
GO


