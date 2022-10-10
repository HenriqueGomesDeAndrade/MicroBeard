USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Scheduling]    Script Date: 10/10/2022 09:03:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Scheduling](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCode] [int] NULL,
	[ContactCode] [int] NULL,
	[Date] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[CancellationDate] [datetime2](7) NULL,
	[CancellerCode] [int] NULL,
	[Cancelled] [bit] NULL,
	[DeleteDate] [datetime2](7) NULL,
	[DeleterCode] [int] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_Scheduling] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Scheduling]  WITH CHECK ADD FOREIGN KEY([ContactCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
ALTER TABLE [dbo].[Scheduling]  WITH CHECK ADD FOREIGN KEY([ServiceCode])
REFERENCES [dbo].[Service] ([Code])
GO
