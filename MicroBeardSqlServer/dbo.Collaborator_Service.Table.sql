USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Collaborator_Service]    Script Date: 30/09/2022 10:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collaborator_Service](
	[ServiceCode] [int] NULL,
	[CollaboratorCode] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Collaborator_Service]  WITH CHECK ADD FOREIGN KEY([CollaboratorCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
ALTER TABLE [dbo].[Collaborator_Service]  WITH CHECK ADD FOREIGN KEY([ServiceCode])
REFERENCES [dbo].[Service] ([Code])
GO
