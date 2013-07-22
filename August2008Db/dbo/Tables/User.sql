CREATE TABLE [dbo].[User] (
    [UserId]		INT				IDENTITY(1, 1) NOT NULL,
    [Email]			NVARCHAR(50)	NULL,
    [DisplayName]	NVARCHAR(50)	NULL,
    [MemberSince]	DATETIME		NOT NULL DEFAULT(GETDATE()), 
	[SuperAdmin]	BIT				NULL DEFAULT(0)
    CONSTRAINT [PK_User] PRIMARY KEY ([UserId])
);



