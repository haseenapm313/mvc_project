Create database MVC_Project

create table Registration(
RegId int primary key identity(1,1) not null,
Firstname varchar(50) null,
Lastname varchar(50) null,
Username varchar(50) null,
Password varchar(20) null,
ConfirmPassword varchar(20) null,
Mobile varchar(50) null,
EmailID varchar(50) null,
Address varchar(50) null,
City varchar(50) null,
District varchar(50) null,
State varchar(50) null,
Role varchar(10) null,
Status varchar(10) null)
----------------------------------------------------------------------------
CREATE TABLE TblAdmin(
	[Aid] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Status] [nchar](10) NULL)

---=============================================================================


CREATE TABLE HallService(
Categoryid int primary key identity(1,1) NOT NULL,
Categoryname varchar(50) NOT NULL,
Cost int NOT NULL,
Status varchar(50) NULL)


--==========================================================================
create TABLE Eventbooking(
BookingId int primary key identity(1,1) NOT NULL,
BookingDate varchar(50) NOT NULL,
RegId int  NOT NULL,
EventDate varchar(50) NOT NULL,
EventName varchar(50) not NULL,
Lobby varchar(100) NULL,
Hall varchar(100) NULL,
Room varchar(100) NULL,
Dining varchar(100) NULL,
Parking varchar(100) NULL,
Status varchar(50) NULL
foreign key(RegId)references Registration(RegId)
)


--===========================================================
use MVC_Project
alter procedure manager_accepts
( 
@Role as varchar(10)
)
as
begin

update Registration set Status='Accepted' where Role='Manager'

end
GO

--=================================================================
select * from Registration
select * from Eventbooking
select * from HallService
select * from TblAdmin




