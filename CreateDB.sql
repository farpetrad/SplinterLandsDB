USE [master]
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Splinterlands')
	BEGIN
		PRINT 'Creating Database Splinterlands'
		CREATE DATABASE [Splinterlands]
	END
ELSE
	PRINT 'Database already exists skipping'
GO

USE [Splinterlands]
GO

IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = 'SplinterlandsReader') BEGIN
    /* Syntax for SQL server login.  See BOL for domain logins, etc. */
    CREATE LOGIN SplinterlandsReader 
    WITH PASSWORD = '420splinterlands#HIVE#Play2Earn$#bot69'
END
ELSE
	PRINT 'Login ''SplinterlandsReader'' already exists'
GO

/* Create the user for the specified login. */
IF NOT EXISTS(SELECT principal_id FROM sys.database_principals WHERE name = 'SplinterlandsReader') BEGIN
    CREATE USER SplinterlandsReader FOR LOGIN SplinterlandsReader
END
ELSE
	PRINT 'User ''SplinterlandsReader'' already exists'
GO

IF OBJECT_ID(N'dbo.Cards', N'U') IS NULL 
BEGIN   
	PRINT 'Creating table Cards'
	CREATE TABLE Cards (
		CardKey UNIQUEIDENTIFIER  PRIMARY KEY DEFAULT newsequentialid(),
		Id INT NOT NULL,
		[Name] NVARCHAR(128) NOT NULL,
		Color NVARCHAR(36) NOT NULL,
		[Type] NVARCHAR(64) NOT NULL,
		Total_Printed INT NOT NULL, 
		Rarity INT NOT NULL,
		Is_Promo BIT NOT NULL,
		Is_Starter BIT NOT NULL
	); 
END
GO

IF OBJECT_ID(N'dbo.SummonerStats', N'U') IS NULL 
BEGIN
	CREATE TABLE SummonerStats (
		StatsKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newsequentialid(),
		CardKey UNIQUEIDENTIFIER  NOT NULL,
		Armor INT NOT NULL,
		Attack INT NOT NULL,
		Health INT NOT NULL,
		Magic INT NOT NULL,
		Mana INT NOT NULL,
		Ranged INT NOT NULL,
		Speed INT NOT NULL,
		Abilities NVARCHAR(1024)
	);
END
GO

IF OBJECT_ID('dbo.[FK_SSCardId]') IS NOT NULL
	BEGIN
		PRINT 'Constraint FK_SSCardId exists, removing to add'
		ALTER TABLE dbo.SummonerStats DROP CONSTRAINT FK_SSCardId
	END
GO

ALTER TABLE dbo.SummonerStats
	ADD CONSTRAINT FK_SSCardId FOREIGN KEY (CardKey) REFERENCES Cards(CardKey);
GO

IF OBJECT_ID(N'dbo.MonsterStats', N'U') IS NULL 
BEGIN
	CREATE TABLE MonsterStats (
		StatsKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newsequentialid(),
		CardKey UNIQUEIDENTIFIER  NOT NULL,
		Armor NVARCHAR(256) NOT NULL,
		Attack NVARCHAR(256) NOT NULL,
		Health NVARCHAR(256) NOT NULL,
		Magic NVARCHAR(256) NOT NULL,
		Mana NVARCHAR(256) NOT NULL,
		Ranged NVARCHAR(256) NOT NULL,
		Speed NVARCHAR(256) NOT NULL,
		Abilities NVARCHAR(1024)
	);
END
GO

IF OBJECT_ID('dbo.[FK_MSCardId]') IS NOT NULL
	BEGIN
		PRINT 'Constraint FK_MSCardId exists, removing to add'
		ALTER TABLE dbo.MonsterStats DROP CONSTRAINT FK_MSCardId
	END
GO

ALTER TABLE MonsterStats
	ADD CONSTRAINT FK_MSCardId FOREIGN KEY (CardKey) REFERENCES Cards(CardKey);
GO


IF OBJECT_ID(N'dbo.Battle', N'U') IS NULL 
BEGIN
	CREATE TABLE Battle (
		PlayerBattleKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newsequentialid(),
		Battle_queue_id_1 NVARCHAR(1024) NOT NULL,
		Battle_queue_id_2 NVARCHAR(1024) NOT NULL,
		Player_1 NVARCHAR(512) NOT NULL,
		Player_2 NVARCHAR(512) NOT NULL,
		Winner NVARCHAR(512) NOT NULL,
		Player_1_rating_initial INT NOT NULL,
		Player_2_rating_initial INT NOT NULL,
		Player_1_rating_final INT NOT NULL,
		Player_2_rating_final INT NOT NULL,
		Created_Date DATETIME,
		Match_Type NVARCHAR(1024) NOT NULL,
		Mana_Cap INT NOT NULL,
		Inactive NVARCHAR(1024) NOT NULL,
		Block_Num BIGINT NOT NULL,
		Rshares BIGINT NOT NULL,
		Leaderboard BIGINT NOT NULL,
		Reward_Dec NVARCHAR(32),
		Reward_Sps NVARCHAR(32)
		
	);
END
GO

IF OBJECT_ID(N'dbo.BattleDetails', N'U') IS NULL 
BEGIN
	CREATE TABLE BattleDetails (
		BattleDetailsKey UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
		PlayerBattleKey UNIQUEIDENTIFIER NOT NULL ,
		Seed NVARCHAR(1024) NOT NULL,
		Winner NVARCHAR(1024) NOT NULL,
		Loser NVARCHAR(1024) NOT NULL
	);
END
GO

IF OBJECT_ID('dbo.[FK_BattleDetails_Battle]') IS NOT NULL
	BEGIN
		PRINT 'Constraint FK_BattleDetails_Battle exists, removing to add'
		ALTER TABLE dbo.BattleDetails DROP CONSTRAINT FK_BattleDetails_Battle
	END
GO

ALTER TABLE BattleDetails
	ADD CONSTRAINT FK_BattleDetails_Battle FOREIGN KEY (PlayerBattleKey) REFERENCES Battle(PlayerBattleKey);
GO

IF OBJECT_ID(N'dbo.Team', N'U') IS NULL 
BEGIN
	CREATE TABLE Team (
		TeamKey UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
		Player NVARCHAR(1024) NOT NULL,
		Rating BIGINT NOT NULL,
		Color NVARCHAR(36)
	);
END
GO

IF OBJECT_ID(N'dbo.BattleCard', N'U') IS NULL 
BEGIN
	CREATE TABLE BattleCard (
		BattleCardKey UNIQUEIDENTIFIER PRIMARY KEY DEFAULT newsequentialid(),
		TeamKey UNIQUEIDENTIFIER NOT NULL,
		Uid NVARCHAR(1024) NOT NULL,
		Xp INT NOT NULL,
		Gold BIT NOT NULL,
		Card_Detail_Id BIGINT NOT NULL,
		Level INT NOT NULL,
		Edition INT NOT NULL,
		Skin NVARCHAR(1024)
	);
END
GO

IF OBJECT_ID('dbo.[FK_BattleCard_Team]') IS NOT NULL
	BEGIN
		PRINT 'Constraint FK_BattleCard_Team exists, removing to add'
		ALTER TABLE dbo.BattleCard DROP CONSTRAINT FK_BattleCard_Team
	END
GO

ALTER TABLE BattleCard
	ADD CONSTRAINT FK_BattleCard_Team FOREIGN KEY (TeamKey) REFERENCES Team(TeamKey);
GO

GRANT SELECT, INSERT ON DATABASE::[Splinterlands] to  [SplinterlandsReader]
GO