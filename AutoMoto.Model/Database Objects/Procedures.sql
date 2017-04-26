

CREATE PROCEDURE [sp_GetAdvertisementById]
(
@id INT 
)
AS
BEGIN
SELECT Id,Title,AddedDate,Description,UserId,IsActive
FROM Advertisements
WHERE id = @id
END


GO

CREATE PROCEDURE [sp_NewMessage]
(
@fromUserId NVARCHAR(128),
@toUserId NVARCHAR(128),
@content  NVARCHAR(max),
@advertisementId INT
)
AS
BEGIN

DECLARE @NextNotID int ;  
DECLARE @NextMessID int ;  
SET @NextNotID = NEXT VALUE FOR [dbo].[notseq] ;  
SET @NextMessID = NEXT VALUE FOR [dbo].[messseq] ;

SET IDENTITY_INSERT Messages ON
INSERT INTO Messages (id,fromUserId,toUserId,content,advertisementId)
VALUES (@NextMessID,@fromUserId,@toUserId,@content,@advertisementId);
SET IDENTITY_INSERT Messages OFF

SET IDENTITY_INSERT Notifications ON
INSERT INTO Notifications (Id) Values (@NextNotID);
SET IDENTITY_INSERT Notifications OFF

INSERT INTO MessageNotifications (Id,MessageId) Values (@NextNotID,@NextMessID);
INSERT INTO UserNotifications (NotificationId,UserId) Values (@NextNotID,@toUserId);
END


go



CREATE PROCEDURE [sp_GetAllFeatures]
AS
BEGIN
SELECT Id,Name
FROM Features
END
go


CREATE PROCEDURE [sp_GetAllManufactures]
AS
BEGIN
SELECT Id,Name, IsActive
FROM Manufacturers
END



go 
CREATE PROCEDURE [sp_GetModelsByManufacturer]
(
@manufacturerId INT 
)
AS
BEGIN
SELECT Id,Name,ManufacturerId
FROM Models
WHERE ManufacturerId = @manufacturerId
END

go

CREATE PROCEDURE [sp_DeleteAdvertisement]
(
@id INT 
)
AS
BEGIN
UPDATE ADVERTISEMENTS
set IsActive = 0
WHERE id = @id
END

go


CREATE PROCEDURE [sp_InsertFeature]
(
@name  NVARCHAR(max)
)
AS
BEGIN

SET IDENTITY_INSERT Features ON
INSERT INTO Features (name)
VALUES (@name);
SET IDENTITY_INSERT Features OFF
END


go

CREATE PROCEDURE [sp_InsertManufacturer]
(
@name  NVARCHAR(max)
)
AS
BEGIN
  
SET IDENTITY_INSERT Manufacturers ON
	INSERT INTO Manufacturers (name)
	VALUES (@name);
SET IDENTITY_INSERT Manufacturers OFF
END


go


CREATE PROCEDURE [sp_GetManufacturerById]
(
@id INT 
)
AS
BEGIN
SELECT Id,Name,IsActive
FROM Manufacturers
WHERE id = @id
END


GO



CREATE PROCEDURE [sp_ChangeManufacturerStatus]
(
@id INT ,
@status INT
)
AS
BEGIN
UPDATE Manufacturers
set IsActive = @status
WHERE id = @id
END

go


CREATE PROCEDURE [sp_GetAllModels]
AS
BEGIN
SELECT m.Id,m.Name,m.ManufacturerId,ma.Name as ManufacturerName
FROM Models m
INNER JOIN Manufacturers ma on ma.Id = m.ManufacturerId
END

go


CREATE PROCEDURE [sp_InsertModel]
(
@name  NVARCHAR(max),
@manufacturerName  NVARCHAR(max)
)
AS
BEGIN
  

DECLARE 
@ManId int ;  
SELECT
	@ManId = id
	FROM Manufacturers
	WHERE NAME = @manufacturerName;

	SET IDENTITY_INSERT Models ON
		INSERT INTO Models (name,manufacturerId)
		VALUES (@name,@ManId);
	SET IDENTITY_INSERT Models OFF
END


go


CREATE PROCEDURE [sp_GetAllAdvertDetails]
AS
BEGIN
SELECT a.Id,a.Title,a.Description,a.IsActive,a.AddedDate,c.EngineCap,a.UserId,
c.FuelType,c.Mileage,c.ModelId,c.Price,c.Year,mo.Name as ModelName,
mo.ManufacturerId, ma.Name as ManufacturerName,
u.FirstName,u.LastName,u.AddressId, ad.City, p.Content,p.Extension
FROM Advertisements a
INNER JOIN Cars c on c.Id = a.Id
INNER JOIN Models mo on mo.Id = c.ModelId
INNER JOIN Manufacturers ma on ma.Id = mo.ManufacturerId
INNER JOIN AspNetUsers u on a.UserId = u.Id
INNER JOIN Addresses ad on u.AddressId= ad.Id
INNER JOIN Photos p on p.Advertisement_Id = a.Id
END

go

CREATE PROCEDURE [sp_GetAllAdvertDetailsByUser]
(
@userId NVARCHAR(128)
)
AS
BEGIN
SELECT a.Id,a.Title,a.Description,a.IsActive,a.AddedDate,c.EngineCap,a.UserId,
c.FuelType,c.Mileage,c.ModelId,c.Price,c.Year,mo.Name as ModelName,
mo.ManufacturerId, ma.Name as ManufacturerName,
u.FirstName,u.LastName,u.AddressId, ad.City, p.Content,p.Extension
FROM Advertisements a
INNER JOIN Cars c on c.Id = a.Id
INNER JOIN Models mo on mo.Id = c.ModelId
INNER JOIN Manufacturers ma on ma.Id = mo.ManufacturerId
INNER JOIN AspNetUsers u on a.UserId = u.Id
INNER JOIN Addresses ad on u.AddressId= ad.Id
INNER JOIN Photos p on p.Advertisement_Id = a.Id
WHERE A.UserId = @userId
END


go

CREATE PROCEDURE [sp_GetAllAdvertDetailsById]
(
@id INT
)
AS
BEGIN
SELECT a.Id,a.Title,a.Description,a.IsActive,a.AddedDate,c.EngineCap,a.UserId,
c.FuelType,c.Mileage,c.ModelId,c.Price,c.Year,mo.Name as ModelName,
mo.ManufacturerId, ma.Name as ManufacturerName,
u.FirstName,u.LastName,u.AddressId, ad.City, p.Content,p.Extension
FROM Advertisements a
INNER JOIN Cars c on c.Id = a.Id
INNER JOIN Models mo on mo.Id = c.ModelId
INNER JOIN Manufacturers ma on ma.Id = mo.ManufacturerId
INNER JOIN AspNetUsers u on a.UserId = u.Id
INNER JOIN Addresses ad on u.AddressId= ad.Id
INNER JOIN Photos p on p.Advertisement_Id = a.Id
WHERE A.Id = @id
END

go

CREATE PROCEDURE [sp_FollowersCountByFollowee]
(
@followerId NVARCHAR(128),
@followeeId NVARCHAR(128)
)
AS
BEGIN
SELECT COUNT(*)
FROM Followings
WHERE FollowerId=@followerId AND FolloweeId=@followeeId
END

go

CREATE PROCEDURE [sp_InsertFollowing]
(
@followerId NVARCHAR(128),
@followeeId NVARCHAR(128)
)
AS
BEGIN
INSERT INTO FOLLOWINGS(FollowerId,FolloweeId)
VALUES (@followerId,@followeeId);
END




go

CREATE PROCEDURE [sp_DeleteFollowing]
(
@followerId NVARCHAR(128),
@followeeId NVARCHAR(128)
)
AS
BEGIN
delete from FOLLOWINGS(FollowerId,FolloweeId)
where FollowerId=@followerId AND FolloweeId=@followeeId
END


GO

CREATE PROCEDURE [sp_MarkAsRead]
(
@userId NVARCHAR(128)
)
AS
BEGIN
	UPDATE UserNotifications
	SET IsRead = 1
	WHERE UserId=@userId
END


GO 

CREATE PROCEDURE [sp_GetUnreadForUser]
(
@userId NVARCHAR(128)
)
AS
BEGIN

SELECT n.id, n.datetime,fn.advertisementId,u.FirstName,u.LastName,a.Title
FROM UserNotifications un
INNER JOIN Notifications n on n.id=un.NotificationId
INNER JOIN FollowingNotifications fn on fn.Id=n.Id
INNER JOIN Advertisements a on fn.AdvertisementId = a.Id
INNER JOIN AspNetUsers u on u.Id = a.UserId
WHERE un.UserId = @userId AND un.IsRead = 0;

END


go

CREATE PROCEDURE [sp_InsertAdvertisement]
(
@followerId NVARCHAR(128),
@followeeId NVARCHAR(128)
)
AS
BEGIN
INSERT INTO FOLLOWINGS(FollowerId,FolloweeId)
VALUES (@followerId,@followeeId);
END

