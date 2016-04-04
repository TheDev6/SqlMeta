CREATE PROCEDURE [dbo].[GetPerson]
	@personGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[PersonGuid], 
		[FirstName], 
		[LastName], 
		[Age], 
		[Height], 
		[Weight] 
	FROM [dbo].[Person] 
	WHERE [PersonGuid] = @personGuid

	RETURN 0
END
