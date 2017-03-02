

CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](500) NULL,
	[Email] [varchar](200) NULL,
	[Mobile] [varchar](200) NULL,
	[Password] [varchar](500) NULL,
	[Realname] [varchar](200) NULL,
	[LastIpAddress] [varchar](200) NULL,
	[LastLoginOn] [datetime] NULL,
	[CreateOn] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [Deleted]
GO

