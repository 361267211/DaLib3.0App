/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 'ByKpD6IAtgEEXaXd' AS ColumnID
,'ByKpD6IAtgEEXaXd' AS ColumnIDs
,[Title]
      ,'' AS TitleStyle	  
      ,[SubTitle]
	  ,'BV9bK6IAbRrELwXb' AS ParentCatalogue
      ,[Content]
      ,[Pic] AS Cover
      ,[Author]
      ,[Operator] AS Publisher
      ,'' AS PublisherName
      ,[Createtime] AS PublishDate
      ,'1' AS [Status]
	  ,'1;2;3;4' AS Terminals
	  ,'8' AS [AuditStatus]
      ,[Keywords]
	  ,CONVERT(DATETIME,'2022-12-31 00:00:00',20) AS ExpirationDate
      ,[OutUrl] AS JumpLink
      ,[ClickNum] AS HitCount
      ,[OrderIndex] AS OrderNum
      ,'' AS AuditProcessJson
	  ,'' AS ExpendFiled1
	  ,'' AS ExpendFiled2
      ,'' AS ExpendFiled3
      ,'' AS ExpendFiled4
	  ,'' AS ExpendFiled5
      ,0 AS [DeleteFlag]
      ,NULL AS TenantId
	  ,Createtime AS CreatedTime
	  ,Updatetime AS UpdatedTime
  FROM [Smart_Portal_Demo].[dbo].[ContentInfo] WHERE DeleteFlag=0 AND ItemType=0 AND PlateID=60 AND (Status=1 OR Status=6 OR Status=9)  ORDER BY OrderNum 