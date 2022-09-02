-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE GetBySearch
	-- Add the parameters for the stored procedure here
	@search nvarchar(max)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	SELECT * from [dbo].[Tbl_Product] P
	left join [dbo].[Tbl_Category] C on p.CategoryId=c.CategoryId
	where
    p.ProductName like case when @search is not null then  '%'+@search+'%' else p.ProductName end
	or
	c.CategoryName like case when @search is not null then  '%'+@search+'%' else c.CategoryName end
END
GO
