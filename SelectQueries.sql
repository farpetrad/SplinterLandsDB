USE [Splinterlands]
GO

-- Select all sumoners
SELECT * FROM Cards WITH(NOLOCK) 
 INNER JOIN SummonerStats ST WITH(NOLOCK)
   ON ST.CardKey = Cards.CardKey
GO

-- Select 1 summoner
SELECT * FROM Cards WITH(NOLOCK)
  INNER JOIN SummonerStats ST WITH(NOLOCK)
   ON ST.CardKey = Cards.CardKey
WHERE Id=5
GO

-- Select all monsters
SELECT * FROM Cards WITH(NOLOCK)
  INNER JOIN MonsterStats MT WITH(NOLOCK)
    ON MT.CardKey = Cards.CardKey
GO

-- Select 1 monster
SELECT * FROM Cards WITH(NOLOCK)
  INNER JOIN MonsterStats ST WITH(NOLOCK)
   ON ST.CardKey = Cards.CardKey
WHERE Id=1
GO