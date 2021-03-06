USE [TasksDB]
GO
/****** Object:  Table [dbo].[Worker]    Script Date: 08/01/2016 20:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Worker](
	[workerID] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [nvarchar](50) NOT NULL,
	[lastName] [nvarchar](50) NOT NULL,
	[job] [nvarchar](20) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NULL,
	[isManager] [tinyint] NULL,
	[phone] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Worker] PRIMARY KEY CLUSTERED 
(
	[workerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 08/01/2016 20:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[taskID] [int] IDENTITY(1,1) NOT NULL,
	[workerID] [int] NOT NULL,
	[taskDescription] [nvarchar](50) NOT NULL,
	[dateCreated] [datetime] NULL,
	[accept] [tinyint] NULL,
	[acceptDate] [datetime] NULL,
	[status] [nvarchar](50) NOT NULL,
	[endDate] [datetime] NULL,
	[numOfHowers] [int] NULL,
	[priority] [nvarchar](20) NOT NULL,
	[managerID] [int] NULL,
	[taskRevision] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 08/01/2016 20:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[messageID] [int] IDENTITY(1,1) NOT NULL,
	[workerID] [int] NOT NULL,
	[nonRead] [bit] NULL,
	[TaskId] [int] NULL,
	[message] [nchar](200) NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[messageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Message_Worker]    Script Date: 08/01/2016 20:53:27 ******/
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Worker] FOREIGN KEY([workerID])
REFERENCES [dbo].[Worker] ([workerID])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Worker]
GO
/****** Object:  ForeignKey [FK_Task_Worker]    Script Date: 08/01/2016 20:53:27 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Worker] FOREIGN KEY([workerID])
REFERENCES [dbo].[Worker] ([workerID])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Worker]
GO
