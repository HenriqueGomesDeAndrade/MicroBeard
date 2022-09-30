USE [MicroBeard]
GO
/****** Object:  Table [dbo].[Licenced_Collaborator]    Script Date: 30/09/2022 10:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Licenced_Collaborator](
	[LicenceCode] [int] NULL,
	[CollaboratorCode] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Licenced_Collaborator]  WITH CHECK ADD FOREIGN KEY([CollaboratorCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
ALTER TABLE [dbo].[Licenced_Collaborator]  WITH CHECK ADD FOREIGN KEY([LicenceCode])
REFERENCES [dbo].[License] ([Code])
GO
