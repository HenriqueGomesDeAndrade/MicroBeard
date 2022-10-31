/******** DMA Schema Migration Deployment Script      Script Date: 31/10/2022 11:11:45 ********/

/****** Object:  Table [dbo].[Contact]    Script Date: 31/10/2022 11:11:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contact]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Contact](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[Address] [varchar](200) COLLATE Latin1_General_CI_AS NULL,
	[Email] [varchar](80) COLLATE Latin1_General_CI_AS NULL,
	[Password] [varchar](128) COLLATE Latin1_General_CI_AS NULL,
	[PasswordSaltGUID] [varchar](36) COLLATE Latin1_General_CI_AS NULL,
	[CPF] [varchar](11) COLLATE Latin1_General_CI_AS NULL,
	[Phone] [varchar](15) COLLATE Latin1_General_CI_AS NULL,
	[Gender] [char](1) COLLATE Latin1_General_CI_AS NULL,
	[BirthDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[DeleterCode] [int] NULL,
	[DeleteDate] [datetime2](7) NULL,
	[Deleted] [bit] NULL,
	[Token] [varchar](500) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contact]') AND name = N'IDX_CONTACT_CPF')
CREATE UNIQUE NONCLUSTERED INDEX [IDX_CONTACT_CPF] ON [dbo].[Contact]
(
	[CPF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contact]') AND name = N'IDX_CONTACT_EMAIL')
CREATE UNIQUE NONCLUSTERED INDEX [IDX_CONTACT_EMAIL] ON [dbo].[Contact]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contact]') AND name = N'IDX_CONTACT_TOKEN')
CREATE NONCLUSTERED INDEX [IDX_CONTACT_TOKEN] ON [dbo].[Contact]
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[Collaborator]    Script Date: 31/10/2022 11:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Collaborator]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Collaborator](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[CPF] [varchar](11) COLLATE Latin1_General_CI_AS NULL,
	[Email] [varchar](80) COLLATE Latin1_General_CI_AS NULL,
	[Password] [varchar](128) COLLATE Latin1_General_CI_AS NULL,
	[PasswordSaltGUID] [varchar](36) COLLATE Latin1_General_CI_AS NULL,
	[BirthDate] [datetime2](7) NULL,
	[Phone] [varchar](15) COLLATE Latin1_General_CI_AS NULL,
	[Function] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	[Salary] [decimal](9, 2) NULL,
	[Commision] [decimal](9, 2) NULL,
	[CreateDate] [datetime2](7) NULL,
	[CreatorCode] [int] NULL,
	[UpdateDate] [datetime2](7) NULL,
	[UpdaterCode] [int] NULL,
	[DesactivationDate] [datetime2](7) NULL,
	[DesactivatorCode] [int] NULL,
	[Desactivated] [bit] NULL,
	[Token] [varchar](500) COLLATE Latin1_General_CI_AS NULL,
	[IsAdmin] [bit] NULL,
 CONSTRAINT [PK_Collaborator] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Collaborator]') AND name = N'IDX_COLLABORATOR_CPF')
CREATE UNIQUE NONCLUSTERED INDEX [IDX_COLLABORATOR_CPF] ON [dbo].[Collaborator]
(
	[CPF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Collaborator]') AND name = N'IDX_COLLABORATOR_EMAIL')
CREATE UNIQUE NONCLUSTERED INDEX [IDX_COLLABORATOR_EMAIL] ON [dbo].[Collaborator]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Collaborator]') AND name = N'IDX_COLLABORATOR_TOKEN')
CREATE NONCLUSTERED INDEX [IDX_COLLABORATOR_TOKEN] ON [dbo].[Collaborator]
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[License]    Script Date: 31/10/2022 11:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[License]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[License](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](250) COLLATE Latin1_General_CI_AS NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
/****** Object:  Table [dbo].[CollaboratorLicense]    Script Date: 31/10/2022 11:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CollaboratorLicense]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CollaboratorLicense](
	[LicensesCode] [int] NOT NULL,
	[CollaboratorsCode] [int] NOT NULL,
 CONSTRAINT [PK_CollaboratorLicense] PRIMARY KEY CLUSTERED 
(
	[LicensesCode] ASC,
	[CollaboratorsCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
/****** Object:  Table [dbo].[Service]    Script Date: 31/10/2022 11:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Service](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	[Price] [decimal](9, 2) NULL,
	[Time] [int] NULL,
	[Type] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Description] [varchar](250) COLLATE Latin1_General_CI_AS NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
/****** Object:  Table [dbo].[CollaboratorService]    Script Date: 31/10/2022 11:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CollaboratorService]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CollaboratorService](
	[ServicesCode] [int] NOT NULL,
	[CollaboratorsCode] [int] NOT NULL,
 CONSTRAINT [PK_CollaboratorService] PRIMARY KEY CLUSTERED 
(
	[ServicesCode] ASC,
	[CollaboratorsCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
/****** Object:  Table [dbo].[Scheduling]    Script Date: 31/10/2022 11:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Scheduling]') AND type in (N'U'))
BEGIN
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 10)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorLicense_CollaboratorsCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorLicense]'))
ALTER TABLE [dbo].[CollaboratorLicense]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorLicense_CollaboratorsCode] FOREIGN KEY([CollaboratorsCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorLicense_CollaboratorsCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorLicense]'))
ALTER TABLE [dbo].[CollaboratorLicense] CHECK CONSTRAINT [FK_CollaboratorLicense_CollaboratorsCode]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorLicense_LicensesCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorLicense]'))
ALTER TABLE [dbo].[CollaboratorLicense]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorLicense_LicensesCode] FOREIGN KEY([LicensesCode])
REFERENCES [dbo].[License] ([Code])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorLicense_LicensesCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorLicense]'))
ALTER TABLE [dbo].[CollaboratorLicense] CHECK CONSTRAINT [FK_CollaboratorLicense_LicensesCode]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorService_CollaboratorsCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorService]'))
ALTER TABLE [dbo].[CollaboratorService]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorService_CollaboratorsCode] FOREIGN KEY([CollaboratorsCode])
REFERENCES [dbo].[Collaborator] ([Code])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorService_CollaboratorsCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorService]'))
ALTER TABLE [dbo].[CollaboratorService] CHECK CONSTRAINT [FK_CollaboratorService_CollaboratorsCode]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorService_ServicesCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorService]'))
ALTER TABLE [dbo].[CollaboratorService]  WITH CHECK ADD  CONSTRAINT [FK_CollaboratorService_ServicesCode] FOREIGN KEY([ServicesCode])
REFERENCES [dbo].[Service] ([Code])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollaboratorService_ServicesCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollaboratorService]'))
ALTER TABLE [dbo].[CollaboratorService] CHECK CONSTRAINT [FK_CollaboratorService_ServicesCode]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Scheduling_Contact_Code]') AND parent_object_id = OBJECT_ID(N'[dbo].[Scheduling]'))
ALTER TABLE [dbo].[Scheduling]  WITH CHECK ADD  CONSTRAINT [FK_Scheduling_Contact_Code] FOREIGN KEY([ContactCode])
REFERENCES [dbo].[Contact] ([Code])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Scheduling_Contact_Code]') AND parent_object_id = OBJECT_ID(N'[dbo].[Scheduling]'))
ALTER TABLE [dbo].[Scheduling] CHECK CONSTRAINT [FK_Scheduling_Contact_Code]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Scheduling_Service_Code]') AND parent_object_id = OBJECT_ID(N'[dbo].[Scheduling]'))
ALTER TABLE [dbo].[Scheduling]  WITH CHECK ADD  CONSTRAINT [FK_Scheduling_Service_Code] FOREIGN KEY([ServiceCode])
REFERENCES [dbo].[Service] ([Code])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Scheduling_Service_Code]') AND parent_object_id = OBJECT_ID(N'[dbo].[Scheduling]'))
ALTER TABLE [dbo].[Scheduling] CHECK CONSTRAINT [FK_Scheduling_Service_Code]
GO

