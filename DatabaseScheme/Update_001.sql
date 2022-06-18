ALTER PROCEDURE [dbo].[ReturnBook] 
	@idBook int,
	@Date datetime
AS
BEGIN
declare @id int
select @id = IdRead from ReaderBooks where BookId = @idBook and DateOfReturning is null

update ReaderBooks set DateOfReturning = @Date where IdRead = @id
update Books set IsReserved = 0 where id = @idBook
END
GO