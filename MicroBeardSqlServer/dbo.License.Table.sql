USE [MicroBeard]
GO
/****** Object:  Table [dbo].[License]    Script Date: 30/10/2022 21:22:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[License](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](250) NULL,
	[CreateDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[DesactivationDate] [datetime2](7) NULL,
	[DesactivatorCode] [int] NULL,
	[Desactivated] [bit] NULL,
 CONSTRAINT [PK_License] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
