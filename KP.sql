USE [master]
GO
/****** Object:  Database [KP]    Script Date: 21.10.2020 0:44:59 ******/
CREATE DATABASE [KP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KP', FILENAME = N'C:\Users\Public\KP.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'KP_log', FILENAME = N'C:\Users\Public\KP_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [KP] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KP] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KP] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KP] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KP] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KP] SET ARITHABORT OFF 
GO
ALTER DATABASE [KP] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [KP] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KP] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KP] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KP] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KP] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KP] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KP] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KP] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KP] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KP] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KP] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KP] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [KP] SET  MULTI_USER 
GO
ALTER DATABASE [KP] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KP] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KP] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [KP] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [KP] SET QUERY_STORE = OFF
GO
USE [KP]
GO
/****** Object:  Table [dbo].[Albums]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Albums](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Date] [date] NULL,
	[Image] [int] NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Executors]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Executors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Country] [int] NULL,
	[Date] [date] NULL,
	[Image] [int] NULL,
 CONSTRAINT [PK_Executors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genres]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Images]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](260) NOT NULL,
	[Binary] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Likes]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Likes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](20) NOT NULL,
	[Track] [int] NOT NULL,
 CONSTRAINT [PK_Likes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Records]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Records](
	[Track] [int] NOT NULL,
	[Album] [int] NOT NULL,
	[Executor] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Styles]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Styles](
	[Executor] [int] NOT NULL,
	[Genre] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tracks]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tracks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Binary] [varbinary](max) NULL,
	[Image] [int] NULL,
 CONSTRAINT [PK_Tracks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Login] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Surname] [nvarchar](20) NOT NULL,
	[Gender] [nvarchar](7) NOT NULL,
	[Country] [int] NULL,
	[Image] [int] NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Images] FOREIGN KEY([Image])
REFERENCES [dbo].[Images] ([Id])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Images]
GO
ALTER TABLE [dbo].[Executors]  WITH CHECK ADD  CONSTRAINT [FK_Executors_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Executors] CHECK CONSTRAINT [FK_Executors_Countries]
GO
ALTER TABLE [dbo].[Executors]  WITH CHECK ADD  CONSTRAINT [FK_Executors_Images] FOREIGN KEY([Image])
REFERENCES [dbo].[Images] ([Id])
GO
ALTER TABLE [dbo].[Executors] CHECK CONSTRAINT [FK_Executors_Images]
GO
ALTER TABLE [dbo].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Tracks] FOREIGN KEY([Track])
REFERENCES [dbo].[Tracks] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Likes] CHECK CONSTRAINT [FK_Likes_Tracks]
GO
ALTER TABLE [dbo].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Users] FOREIGN KEY([Login])
REFERENCES [dbo].[Users] ([Login])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Likes] CHECK CONSTRAINT [FK_Likes_Users]
GO
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_Albums] FOREIGN KEY([Album])
REFERENCES [dbo].[Albums] ([Id])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_Albums]
GO
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_Executors] FOREIGN KEY([Executor])
REFERENCES [dbo].[Executors] ([Id])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_Executors]
GO
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_Tracks] FOREIGN KEY([Track])
REFERENCES [dbo].[Tracks] ([Id])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_Tracks]
GO
ALTER TABLE [dbo].[Styles]  WITH CHECK ADD  CONSTRAINT [FK_Styles_Executors] FOREIGN KEY([Executor])
REFERENCES [dbo].[Executors] ([Id])
GO
ALTER TABLE [dbo].[Styles] CHECK CONSTRAINT [FK_Styles_Executors]
GO
ALTER TABLE [dbo].[Styles]  WITH CHECK ADD  CONSTRAINT [FK_Styles_Genres] FOREIGN KEY([Genre])
REFERENCES [dbo].[Genres] ([Id])
GO
ALTER TABLE [dbo].[Styles] CHECK CONSTRAINT [FK_Styles_Genres]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Countries]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Images] FOREIGN KEY([Image])
REFERENCES [dbo].[Images] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Images]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([Role])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [MorF] CHECK  (([Gender]='Мужчина' OR [Gender]='Женщина'))
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [MorF]
GO
/****** Object:  StoredProcedure [dbo].[AddNewUser]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewUser] 
	@login nvarchar(20),
	@password nvarchar(20),
	@name nvarchar(20),
	@surname nvarchar(20),
	@gender nvarchar(7),
	@country nvarchar(100),
	@image int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Users VALUES (@login, @password, @name, @surname, @gender, (SELECT Id FROM Countries WHERE Name = @country), @image, (SELECT Id FROM Roles WHERE Name = 'Обычный пользователь'))
END
GO
/****** Object:  StoredProcedure [dbo].[AddNewUsersUser]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddNewUsersUser]
	-- Add the parameters for the stored procedure here
	@login nvarchar(20),
	@password nvarchar(20),
	@name nvarchar(20),
	@surname nvarchar(20),
	@gender nvarchar(7),
	@country nvarchar(100),
	@image int,
	@role nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Users VALUES (@login, @password, @name, @surname, @gender, (SELECT Id FROM Countries WHERE Name = @country), @image, (SELECT Id FROM Roles WHERE Name = @role))
END
GO
/****** Object:  StoredProcedure [dbo].[FindAlbumPiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindAlbumPiece] 
	-- Add the parameters for the stored procedure here
	@album nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Albums.Name as Album, YEar(Albums.Date) as Date
	  from Albums inner join Records on Albums.Id = Album
	where Albums.Name LIKE('%' + @album + '%')
END
GO
/****** Object:  StoredProcedure [dbo].[FindExecutors]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindExecutors]
	-- Add the parameters for the stored procedure here
	@login nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Executors.Name as Executor, YEar(Executors.Date) as Date
	  from Executors inner join Records on Executors.Id = Executor
			
END
GO
/****** Object:  StoredProcedure [dbo].[FindExecutorsInGenre]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindExecutorsInGenre] 
	-- Add the parameters for the stored procedure here
	@name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Executors.Name as Executor, Year(Executors.Date) as Date
	  from Executors inner join Styles on Executors.Id = Executor
							inner join Genres on Genres.Id = Genre
	where Genres.Name = @name
END

GO
/****** Object:  StoredProcedure [dbo].[FindExsInGenrePiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindExsInGenrePiece]
	-- Add the parameters for the stored procedure here
	@genre nvarchar(50),
	@executor nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Executors.Name as Executor, Year(Executors.Date) as Date
	from Executors inner join Styles on Executors.Id = Executor
				   inner join Genres on Genres.Id = Genre
				   where (Genres.Name = @genre and Executors.Name LIKE ('%' + @executor + '%'))
END
GO
/****** Object:  StoredProcedure [dbo].[FindExsPiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindExsPiece] 
	-- Add the parameters for the stored procedure here
	@executor nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Executors.Name as Executor, YEar(Executors.Date) as Date
	  from Executors inner join Records on Executors.Id = Executor
	where Executors.Name LIKE('%' + @executor + '%')
END
GO
/****** Object:  StoredProcedure [dbo].[FindImage]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindImage] 
	@binary varbinary(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id FROM Images WHERE Binary = @binary
END
GO
/****** Object:  StoredProcedure [dbo].[FindLikedExecutors]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindLikedExecutors] 
	@login nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	select Executors.Name as Executor, YEAR(Executors.Date) as Date 
	  from Executors inner join Records on Executor = Executors.Id 
						inner join Tracks on Track = Tracks.Id
						inner join Likes on Likes.Track = Tracks.Id
						inner join Users on Likes.Login = Users.Login
						WHERE Users.Login = @login
END
GO
/****** Object:  StoredProcedure [dbo].[FindLikedExsPiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindLikedExsPiece] 
	-- Add the parameters for the stored procedure here
	@login nvarchar(50),
	@executor nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Executors.Name as Executor, YEAR(Executors.Date) as Date 
	  from Executors inner join Records on Executor = Executors.Id 
						inner join Tracks on Track = Tracks.Id
						inner join Likes on Likes.Track = Tracks.Id
						inner join Users on Likes.Login = Users.Login
						WHERE (Users.Login = @login and Executors.Name LIKE('%' + @executor + '%'))
END
GO
/****** Object:  StoredProcedure [dbo].[FindLikedTracks]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindLikedTracks] 
	-- Add the parameters for the stored procedure here
	@login nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Tracks.Name as Track, Albums.Name as Album, Executors.Name as Executor, Binary
		FROM Tracks inner join records on Tracks.Id = Track
					inner join albums on Albums.Id = Album
					inner join Executors on executors.Id = Executor
					INNER JOIN Likes ON Tracks.Id = Likes.Track
					inner join Users on Likes.Login = Users.Login
		WHERE users.Login = @login
END
GO
/****** Object:  StoredProcedure [dbo].[FindLikedTracksPiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindLikedTracksPiece]
	-- Add the parameters for the stored procedure here
	@login nvarchar(20),
	@track nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Tracks.Name as Track, Albums.Name as Album, Executors.Name as Executor, Binary
		FROM Tracks inner join records on Tracks.Id = Track
					inner join albums on Albums.Id = Album
					inner join Executors on executors.Id = Executor
					INNER JOIN Likes ON Tracks.Id = Likes.Track
					inner join Users on Likes.Login = Users.Login
		WHERE (users.Login = @login and tracks.Name LIKE('%' + @track + '%'))
END
GO
/****** Object:  StoredProcedure [dbo].[FindRole]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindRole] 
	@login nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Roles.Name FROM Roles INNER JOIN Users ON Id = Role WHERE Login = @login
END
GO
/****** Object:  StoredProcedure [dbo].[FindTracks]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindTracks] 
	-- Add the parameters for the stored procedure here
	@login nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Tracks.Name as Track, case
									when tracks.Id not in (select Track
															 from Likes inner join users 
																on users.Login = likes.Login 
															where users.Login = @login)
									then 'False'
									else 'True'
								end as [Like], Albums.Name as Album, Executors.Name as Executor, Binary
	  from Likes inner join Users on users.Login = likes.Login
			full outer join tracks on tracks.Id = Track
			inner join Records on Tracks.Id = Records.Track
			inner join Albums on Albums.Id = Album
			inner join Executors on Executors.Id = Executor
END
GO
/****** Object:  StoredProcedure [dbo].[FindTracksPiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindTracksPiece] 
	-- Add the parameters for the stored procedure here
	@login nvarchar(20),
	@track nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Tracks.Name as Track, case
									when tracks.Id not in (select Track
															 from Likes inner join users 
																on users.Login = likes.Login 
															where users.Login = @login)
									then 'False'
									else 'True'
								end as [Like], Albums.Name as Album, Executors.Name as Executor, Binary
	  from Likes inner join Users on users.Login = likes.Login
			full outer join tracks on tracks.Id = Track
			inner join Records on Tracks.Id = Records.Track
			inner join Albums on Albums.Id = Album
			inner join Executors on Executors.Id = Executor
	where Tracks.Name LIKE('%' + @track + '%')
END
GO
/****** Object:  StoredProcedure [dbo].[FindUsers]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindUsers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Gender, Login, Password, 
		Users.Name as Name, Surname, Roles.Name as Role, Countries.Name as Country
	FROM Users inner join Roles on Roles.Id = Role
				left join Countries on Countries.Id = Country
END
GO
/****** Object:  StoredProcedure [dbo].[FindUsersPiece]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FindUsersPiece]
	-- Add the parameters for the stored procedure here
	@search nvarchar(20)
AS
BEGIN
	SELECT Gender, Login, Password, 
		Users.Name as Name, Surname, Roles.Name as Role, Countries.Name as Country
	FROM Users inner join Roles on Roles.Id = Role
				left join Countries on Countries.Id = Country
	WHERE Users.Name Like('%' + @search + '%') or Login Like('%' + @search + '%')
			or Surname Like('%' + @search + '%') or Gender Like('%' + @search + '%')
			or Countries.Name Like('%' + @search + '%') or Roles.Name Like('%' + @search + '%')
END
GO
/****** Object:  StoredProcedure [dbo].[GetInfos]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetInfos] 
	-- Add the parameters for the stored procedure here
	@login nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Login, Password, Users.Name, Surname, Gender, Countries.Name, Images.Binary, Roles.Name
	  FROM Users INNER JOIN Countries
						 ON Countries.Id = Country
				 LEFT JOIN Images
						 ON Images.Id = Image
				 LEFT JOIN Roles
						 ON Roles.Id = Role
	 WHERE Login = @login
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 21.10.2020 0:44:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUser]
	-- Add the parameters for the stored procedure here
	@login nvarchar(20),
	@password nvarchar(20),
	@name nvarchar(20),
	@surname nvarchar(20),
	@gender nvarchar(7),
	@country nvarchar(100),
	@image int,
	@role nvarchar(30),
	@search nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Users SET Login = @login, Password = @password, Users.Name = @name, 
	Users.Surname = @surname, Users.Gender = @gender, Users.Country = (select Id from Countries where Countries.Name = @country),
	Users.Image = @image, Users.Role = (select id from Roles where Roles.Name = @role)
	WHERE Login = @search
END
GO
USE [master]
GO
ALTER DATABASE [KP] SET  READ_WRITE 
GO
