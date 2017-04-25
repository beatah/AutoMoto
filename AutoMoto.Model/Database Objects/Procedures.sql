

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