This program tests access to a SQL DB from a Windows Service using Active Directory credentials. Success/Failure is written to an event log. 
Although it can work on any given Windows platform, it was written specifically to run on a Windows Container running in Azure.

This program accesses a SQL database. The name of the DB can be anything but the DB must contain one table named 'Table1' defined as specified here:

---------------------

USE [tempdb]
GO

/****** Object:  Table [dbo].[Table1]    Script Date: 1/12/2017 9:46:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Table1](
	[Id] [int] NOT NULL,
	[Data] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Table1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--------------------