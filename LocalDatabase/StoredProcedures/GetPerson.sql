CREATE PROCEDURE [dbo].[GetPerson]
	@personId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[PersonId], 
		[FirstName], 
		[LastName], 
		[Age], 
		[Height], 
		[Weight] 
	FROM [dbo].[Person] 
	WHERE [PersonId] = @personId

	RETURN 0
END
