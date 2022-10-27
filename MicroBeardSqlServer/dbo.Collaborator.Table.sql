USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Collaborator]    Script Date: 26/10/2022 23:21:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collaborator](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CPF] [varchar](11) NULL,
	[Email] [varchar](80) NULL,
	[Password] [varchar](128) NULL,
	[PasswordSaltGUID] [varchar](36) NULL,
	[BirthDate] [datetime2](7) NULL,
	[Phone] [varchar](15) NULL,
	[Function] [varchar](100) NULL,
	[Salary] [decimal](9, 2) NULL,
	[Commision] [decimal](9, 2) NULL,
	[CreateDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[DesactivationDate] [datetime2](7) NULL,
	[DesactivatorCode] [int] NULL,
	[Desactivated] [bit] NULL,
	[Token] [varchar](50) NULL,
 CONSTRAINT [PK_Collaborator] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
