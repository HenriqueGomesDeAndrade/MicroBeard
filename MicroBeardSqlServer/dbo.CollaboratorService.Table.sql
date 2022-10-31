USE [MicroBeard]
GO
/****** Object:  Table [dbo].[CollaboratorService]    Script Date: 30/10/2022 21:22:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollaboratorService](
	[ServicesCode] [int] NOT NULL,
	[CollaboratorsCode] [int] NOT NULL,
 CONSTRAINT [PK_CollaboratorService] PRIMARY KEY CLUSTERED 
(
	[ServicesCode] ASC,
	[CollaboratorsCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CollaboratorService]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorService_CollaboratorsCode] FOREIGN KEY([CollaboratorsCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
ALTER TABLE [dbo].[CollaboratorService] CHECK CONSTRAINT [FK_CollaboratorService_CollaboratorsCode]
GO
ALTER TABLE [dbo].[CollaboratorService]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorService_ServicesCode] FOREIGN KEY([ServicesCode])
REFERENCES [dbo].[Service] ([Code])
GO
ALTER TABLE [dbo].[CollaboratorService] CHECK CONSTRAINT [FK_CollaboratorService_ServicesCode]
GO
