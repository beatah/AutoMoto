
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_mytblid]  DEFAULT (NEXT VALUE FOR [mainseq]) FOR [Thread]



USE [AutoMotoEF]
GO
  CREATE TRIGGER tr_notifications ON Notifications
    After INSERT  
	AS 
  BEGIN
      UPDATE notifications
      SET datetime = GETDATE()
      FROM inserted
      WHERE notifications.id = inserted.id;
  END
GO

USE [AutoMotoEF]
GO
  CREATE TRIGGER tr_usernot ON UserNotifications
INSTEAD OF INSERT
	AS 
  BEGIN
      INSERT INTO UserNotifications(NotificationId,UserId,IsRead)
	 select NotificationId,UserId,0 from inserted
  END
GO



USE [AutoMotoEF]
GO
  CREATE TRIGGER tr_feature ON Features
INSTEAD OF INSERT
	AS 
  BEGIN
    INSERT INTO Features(Id,Name)
	SELECT NEXT VALUE FOR featureseq,Name from inserted
  END
GO

USE [AutoMotoEF]
GO
  CREATE TRIGGER tr_Manufacturers ON Manufacturers
INSTEAD OF INSERT
	AS 
  BEGIN
    INSERT INTO Manufacturers(Id,Name,IsActive)
	SELECT NEXT VALUE FOR manufacturersseq,Name,1 from inserted
  END
GO



USE [AutoMotoEF]
GO
  CREATE TRIGGER tr_Models ON Models
INSTEAD OF INSERT
	AS 
  BEGIN
    INSERT INTO Models(Id,Name,manufacturerId)
	SELECT NEXT VALUE FOR modelseq,Name,manufacturerId from inserted
  END
GO