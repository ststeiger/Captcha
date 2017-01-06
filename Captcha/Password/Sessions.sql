


SELECT 
	 BE_ID
	,BE_Name
	,BE_Vorname
	,BE_User
	,BE_Passwort
	,BE_Language
	,BE_Hash 
FROM T_Benutzer 
WHERE BE_Status = 1 
AND BE_Vorname LIKE '%Ignaz%' 



SELECT 
	 id
	,user_id
	,action
	,value
	,created_on
	,updated_on
FROM tokens



CREATE TABLE dbo.T_Tokens 
( 
	id int IDENTITY(1,1) NOT NULL 
	,user_id int NOT NULL 
	,action national character varying(30) NOT NULL 
	,value national character varying(40) NOT NULL 
	,created_on datetime NOT NULL 
	,updated_on datetime NULL 
	,PRIMARY KEY(id) 
); 

GO

ALTER TABLE dbo.tokens ADD  DEFAULT ((0)) FOR user_id 
GO

ALTER TABLE dbo.tokens ADD  DEFAULT (N'') FOR action 
GO

ALTER TABLE dbo.tokens ADD  DEFAULT (N'') FOR value 
GO


