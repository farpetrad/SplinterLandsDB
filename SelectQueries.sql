USE [Splinterlands]
GO

SELECT * FROM Cards WITH(NOLOCK) 
 INNER JOIN SummonerStats ST WITH(NOLOCK)
   ON ST.CardKey = Cards.CardKey
GO

SELECT * FROM Cards WITH(NOLOCK)
  INNER JOIN MonsterStats MT WITH(NOLOCK)
    ON MT.CardKey = Cards.CardKey
GO