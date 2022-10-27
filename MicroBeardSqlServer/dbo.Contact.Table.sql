USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 26/10/2022 23:21:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Address] [varchar](200) NULL,
	[Email] [varchar](80) NULL,
	[Password] [varchar](128) NULL,
	[PasswordSaltGUID] [varchar](36) NULL,
	[CPF] [varchar](11) NULL,
	[Phone] [varchar](15) NULL,
	[Gender] [char](1) NULL,
	[BirthDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[DeleterCode] [int] NULL,
	[DeleteDate] [datetime2](7) NULL,
	[Deleted] [bit] NULL,
	[Token] [varchar](50) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IDX_CPF_Contact] UNIQUE NONCLUSTERED 
(
	[CPF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IDX_Email_Contact] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
