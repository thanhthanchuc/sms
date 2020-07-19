USE [master]
CREATE DATABASE [DB_SMS]


USE [DB_SMS]

GO
ALTER DATABASE [DB_SMS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_SMS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_SMS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_SMS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_SMS] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_SMS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_SMS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_SMS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_SMS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_SMS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_SMS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_SMS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_SMS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_SMS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_SMS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB_SMS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_SMS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_SMS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_SMS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_SMS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_SMS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_SMS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_SMS] SET RECOVERY FULL 
GO
ALTER DATABASE [DB_SMS] SET  MULTI_USER 
GO
ALTER DATABASE [DB_SMS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_SMS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_SMS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_SMS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_SMS] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DB_SMS', N'ON'
GO
ALTER DATABASE [DB_SMS] SET QUERY_STORE = OFF
GO
USE [DB_SMS]
GO
/****** Object:  Table [dbo].[Bring_In]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bring_In](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmpCode] [varchar](50) NULL,
	[FullName] [nvarchar](100) NULL,
	[Position] [varchar](50) NULL,
	[Team] [varchar](10) NULL,
	[Reason] [nvarchar](max) NULL,
	[EstimatedDate] [varchar](50) NULL,
	[EstimatedTime] [varchar](50) NULL,
	[Status] [bit] NULL,
	[Cancel] [bit] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Bring_In] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bring_In_Items]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bring_In_Items](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NULL,
	[Item] [nvarchar](250) NULL,
	[Serial] [nvarchar](250) NULL,
	[Quantity] [decimal](18, 1) NULL,
	[Unit] [nvarchar](50) NULL,
	[AssetType] [int] NULL,
	[IsReturn] [bit] NULL,
	[ReturnDate] [varchar](50) NULL,
	[ReturnTime] [varchar](50) NULL,
	[ApprovedBy] [varchar](50) NULL,
	[ApprovedStatus] [int] NULL,
	[ApproverRemark] [nvarchar](max) NULL,
	[ApprovedDate] [datetime] NULL,
	[GuardIn] [varchar](50) NULL,
	[GuardStatusIn] [int] NULL,
	[GuardRemarkIn] [nvarchar](max) NULL,
	[GuardDateIn] [datetime] NULL,
	[GuardOut] [varchar](50) NULL,
	[GuardStatusOut] [int] NULL,
	[GuardRemarkOut] [nvarchar](max) NULL,
	[GuardDateOut] [datetime] NULL,
	[SMT] [varchar](50) NULL,
	[SMT_Status] [int] NULL,
	[SMT_Remark] [nvarchar](max) NULL,
	[SMT_Date] [datetime] NULL,
	[ITT] [varchar](50) NULL,
	[ITT_Status] [int] NULL,
	[ITT_Remark] [nvarchar](max) NULL,
	[ITT_Date] [datetime] NULL,
	[FST] [varchar](50) NULL,
	[FST_Status] [int] NULL,
	[FST_Remark] [nvarchar](max) NULL,
	[FST_Date] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Bring_In_Items] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bring_Out]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bring_Out](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmpCode] [varchar](50) NULL,
	[FullName] [nvarchar](100) NULL,
	[Position] [varchar](50) NULL,
	[Team] [varchar](10) NULL,
	[Reason] [nvarchar](max) NULL,
	[EstimatedDate] [varchar](50) NULL,
	[EstimatedTime] [varchar](50) NULL,
	[Status] [bit] NULL,
	[Cancel] [bit] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Bring_Out] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bring_Out_Items]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bring_Out_Items](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NULL,
	[Item] [nvarchar](250) NULL,
	[Serial] [nvarchar](250) NULL,
	[Quantity] [decimal](18, 1) NULL,
	[Unit] [nvarchar](50) NULL,
	[AssetType] [int] NULL,
	[IsReturn] [bit] NULL,
	[ReturnDate] [varchar](50) NULL,
	[ReturnTime] [varchar](50) NULL,
	[ApprovedBy] [varchar](50) NULL,
	[ApprovedStatus] [int] NULL,
	[ApproverRemark] [nvarchar](max) NULL,
	[ApprovedDate] [datetime] NULL,
	[GuardOut] [varchar](50) NULL,
	[GuardStatusOut] [int] NULL,
	[GuardRemarkOut] [nvarchar](max) NULL,
	[GuardDateOut] [datetime] NULL,
	[GuardReturn] [varchar](50) NULL,
	[GuardStatusReturn] [int] NULL,
	[GuardRemarkReturn] [nvarchar](max) NULL,
	[GuardDateReturn] [datetime] NULL,
	[SMT] [varchar](50) NULL,
	[SMT_Status] [int] NULL,
	[SMT_Remark] [nvarchar](max) NULL,
	[SMT_Date] [datetime] NULL,
	[ITT] [varchar](50) NULL,
	[ITT_Status] [int] NULL,
	[ITT_Remark] [nvarchar](max) NULL,
	[ITT_Date] [datetime] NULL,
	[FST] [varchar](50) NULL,
	[FST_Status] [int] NULL,
	[FST_Remark] [nvarchar](max) NULL,
	[FST_Date] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Bring_Out_Items] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Go_Out]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Go_Out](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmpCode] [varchar](50) NULL,
	[FullName] [nvarchar](100) NULL,
	[Position] [varchar](50) NULL,
	[Team] [varchar](10) NULL,
	[Shift] [int] NULL,
	[Reason] [nvarchar](max) NULL,
	[Purpose] [bit] NULL,
	[EstimatedDateOut] [varchar](50) NULL,
	[EstimatedTimeOut] [varchar](50) NULL,
	[EstimatedDateReturn] [varchar](50) NULL,
	[EstimatedTimeReturn] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ApprovedBy] [varchar](50) NULL,
	[ApprovedStatus] [int] NULL,
	[ApproverRemark] [nvarchar](max) NULL,
	[ApprovedDate] [datetime] NULL,
	[GuardOut] [varchar](50) NULL,
	[GuardStatusOut] [int] NULL,
	[GuardRemarkOut] [nvarchar](max) NULL,
	[GuardDateOut] [datetime] NULL,
	[GuardReturn] [varchar](50) NULL,
	[GuardStatusReturn] [int] NULL,
	[GuardRemarkReturn] [nvarchar](max) NULL,
	[GuardDateReturn] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Go_Out] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupUsers]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](50) NULL,
	[RolesID] [int] NULL,
 CONSTRAINT [PK_GroupUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guest]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guest](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NULL,
	[IO] [bit] NULL,
	[FullName] [nvarchar](150) NULL,
	[IDCard] [nvarchar](250) NULL,
	[Company] [nvarchar](max) NULL,
	[NumbersOfPerson] [int] NULL,
	[Visa] [nvarchar](200) NULL,
	[Hotel] [nvarchar](max) NULL,
	[Purpose] [nvarchar](max) NULL,
	[TransportNo] [nvarchar](100) NULL,
	[EmployeeID] [varchar](10) NULL,
	[EmployeeName] [nvarchar](250) NULL,
	[Team] [varchar](4) NULL,
	[Position] [varchar](3) NULL,
	[EstimatedDateIn] [varchar](50) NULL,
	[EstimatedTimeIn] [varchar](50) NULL,
	[EstimatedDateOut] [varchar](50) NULL,
	[EstimatedTimeOut] [varchar](50) NULL,
	[KVPNo] [varchar](50) NULL,
	[GuardIn] [varchar](50) NULL,
	[GuardStatusIn] [int] NULL,
	[GuardRemarkIn] [ntext] NULL,
	[GuardDateIn] [datetime] NULL,
	[GuardOut] [varchar](50) NULL,
	[GuardStatusOut] [int] NULL,
	[GuardRemarkOut] [ntext] NULL,
	[GuardDateOut] [datetime] NULL,
	[Status] [bit] NULL,
	[FileRedirectURL] [nvarchar](max) NULL,
	[Cancel] [bit] NULL,
	[Check_Guest] [bit] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Guest_In] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guest_Item]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guest_Item](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NULL,
	[Item] [nvarchar](250) NULL,
	[Serial] [nvarchar](250) NULL,
	[Quantity] [decimal](18, 1) NULL,
	[Unit] [nvarchar](50) NULL,
	[AssetType] [int] NULL,
	[IsReturn] [bit] NULL,
	[ReturnDate] [varchar](50) NULL,
	[ReturnTime] [varchar](50) NULL,
	[SMT] [varchar](50) NULL,
	[SMT_Status] [int] NULL,
	[SMT_Remark] [nvarchar](max) NULL,
	[SMT_Date] [datetime] NULL,
	[ITT] [varchar](50) NULL,
	[ITT_Status] [int] NULL,
	[ITT_Remark] [nvarchar](max) NULL,
	[ITT_Date] [datetime] NULL,
	[FST] [varchar](50) NULL,
	[FST_Status] [int] NULL,
	[FST_Remark] [nvarchar](max) NULL,
	[FST_Date] [datetime] NULL,
	[GuardIn] [varchar](50) NULL,
	[GuardStatusIn] [int] NULL,
	[GuardRemarkIn] [nvarchar](max) NULL,
	[GuardDateIn] [datetime] NULL,
	[GuardOut] [varchar](50) NULL,
	[GuardStatusOut] [int] NULL,
	[GuardRemarkOut] [nvarchar](max) NULL,
	[GuardDateOut] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Guest_Item] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave_Early]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leave_Early](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmpCode] [varchar](50) NULL,
	[FullName] [nvarchar](100) NULL,
	[Position] [varchar](50) NULL,
	[Team] [varchar](10) NULL,
	[Shift] [int] NULL,
	[Reason] [nvarchar](max) NULL,
	[Purpose] [bit] NULL,
	[EstimatedDate] [varchar](50) NULL,
	[EstimatedTime] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ApprovedBy] [varchar](50) NULL,
	[ApprovedStatus] [int] NULL,
	[ApproverRemark] [nvarchar](max) NULL,
	[ApprovedDate] [datetime] NULL,
	[Guard] [varchar](50) NULL,
	[GuardStatus] [int] NULL,
	[GuardRemark] [nvarchar](max) NULL,
	[GuardDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Leave_Early] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL,
	[IDParent] [int] NULL,
	[Link] [nvarchar](50) NULL,
	[DropDown] [nvarchar](50) NULL,
	[Hiden] [int] NULL,
	[Active] [nvarchar](50) NULL,
	[SubActive] [nvarchar](50) NULL,
	[IndexA] [int] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/12/2020 9:33:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmpCode] [varchar](10) NOT NULL,
	[Password] [varchar](50) NULL,
	[FullName] [nvarchar](100) NULL,
	[GroupID] [int] NULL,
	[RoleID] [int] NULL,
	[Status] [int] NULL,
	[Position] [nvarchar](50) NULL,
	[Team] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Cellphone] [nvarchar](20) NULL,
 CONSTRAINT [PK_USR_Users] PRIMARY KEY CLUSTERED 
(
	[EmpCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bring_In_Items]  WITH CHECK ADD  CONSTRAINT [FK_Bring_In_Items_CatID] FOREIGN KEY([CatID])
REFERENCES [dbo].[Bring_In] ([ID])
GO
ALTER TABLE [dbo].[Bring_In_Items] CHECK CONSTRAINT [FK_Bring_In_Items_CatID]
GO
ALTER TABLE [dbo].[Bring_Out_Items]  WITH CHECK ADD  CONSTRAINT [FK_Bring_Out_Items_CatID] FOREIGN KEY([CatID])
REFERENCES [dbo].[Bring_Out] ([ID])
GO
ALTER TABLE [dbo].[Bring_Out_Items] CHECK CONSTRAINT [FK_Bring_Out_Items_CatID]
GO
ALTER TABLE [dbo].[GroupUsers]  WITH CHECK ADD  CONSTRAINT [FK_Roles_ID] FOREIGN KEY([RolesID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[GroupUsers] CHECK CONSTRAINT [FK_Roles_ID]
GO
ALTER TABLE [dbo].[Guest_Item]  WITH CHECK ADD  CONSTRAINT [FK_Guest_Item_CatID] FOREIGN KEY([CatID])
REFERENCES [dbo].[Guest] ([ID])
GO
ALTER TABLE [dbo].[Guest_Item] CHECK CONSTRAINT [FK_Guest_Item_CatID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Group_ID] FOREIGN KEY([GroupID])
REFERENCES [dbo].[GroupUsers] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Group_ID]
GO
USE [master]
GO
ALTER DATABASE [DB_SMS] SET  READ_WRITE 
GO
