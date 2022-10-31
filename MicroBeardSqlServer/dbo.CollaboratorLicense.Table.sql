USE [MicroBeard]
GO
/****** Object:  Table [dbo].[CollaboratorLicense]    Script Date: 30/10/2022 21:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollaboratorLicense](
	[LicensesCode] [int] NOT NULL,
	[CollaboratorsCode] [int] NOT NULL,
 CONSTRAINT [PK_CollaboratorLicense] PRIMARY KEY CLUSTERED 
(
	[LicensesCode] ASC,
	[CollaboratorsCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CollaboratorLicense]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorLicense_CollaboratorsCode] FOREIGN KEY([CollaboratorsCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
ALTER TABLE [dbo].[CollaboratorLicense] CHECK CONSTRAINT [FK_CollaboratorLicense_CollaboratorsCode]
GO
ALTER TABLE [dbo].[CollaboratorLicense]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorLicense_LicensesCode] FOREIGN KEY([LicensesCode])
REFERENCES [dbo].[License] ([Code])
GO
ALTER TABLE [dbo].[CollaboratorLicense] CHECK CONSTRAINT [FK_CollaboratorLicense_LicensesCode]
GO
