USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 30/10/2022 21:22:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Price] [decimal](9, 2) NULL,
	[Time] [int] NULL,
	[Type] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[CreateDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[DeleteDate] [datetime2](7) NULL,
	[DeleterCode] [int] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
