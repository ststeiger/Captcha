
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.___passwords') AND type in (N'U'))
DROP TABLE dbo.___passwords
GO



CREATE TABLE dbo.___passwords 
( 
	 id int NULL 
	,password varchar(200) NULL 
	,des3 varchar(50) NULL 
); 


GO 



INSERT INTO ___passwords(id, password, des3) VALUES (1, N'asdfadsfasdfasdf', N'CRYPT==');
