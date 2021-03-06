CREATE DATABASE cookbook
GO
USE [cookbook]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 7/20/2016 8:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ingredients]    Script Date: 7/20/2016 8:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ingredients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[recipe_category]    Script Date: 7/20/2016 8:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recipe_category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NULL,
	[recipe_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[recipe_ingredients]    Script Date: 7/20/2016 8:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recipe_ingredients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ingredient_id] [int] NULL,
	[recipe_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[recipes]    Script Date: 7/20/2016 8:24:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[recipes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[description] [text] NULL,
	[rating] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
