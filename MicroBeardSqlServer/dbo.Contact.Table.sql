USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 30/09/2022 10:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Address] [varchar](100) NULL,
	[Email] [varchar](50) NULL,
	[CPF] [decimal](11, 0) NULL,
	[Phone] [decimal](11, 0) NULL,
	[Gender] [char](1) NULL,
	[BirthDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[CreateDate] [date] NULL,
	[UpdaterCode] [int] NULL,
	[UpdateDate] [date] NULL,
	[DeleterCode] [int] NULL,
	[DeleteDate] [date] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
