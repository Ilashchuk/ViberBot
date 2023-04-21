CREATE FUNCTION [dbo].[distance_between_points](
  @latitude1 FLOAT,
  @longitude1 FLOAT,
  @latitude2 FLOAT,
  @longitude2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
  DECLARE @R FLOAT = 6371; -- Радіус Землі в кілометрах
  
  DECLARE @lat1 FLOAT = RADIANS(@latitude1);
  DECLARE @lon1 FLOAT = RADIANS(@longitude1);
  DECLARE @lat2 FLOAT = RADIANS(@latitude2);
  DECLARE @lon2 FLOAT = RADIANS(@longitude2);
  
  DECLARE @d_lat FLOAT = @lat2 - @lat1;
  DECLARE @d_lon FLOAT = @lon2 - @lon1;
  
  DECLARE @a FLOAT = POWER(SIN(@d_lat / 2), 2) + 
                      COS(@lat1) * COS(@lat2) * POWER(SIN(@d_lon / 2), 2);
                      
  DECLARE @c FLOAT = 2 * ASIN(SQRT(@a));
  
  DECLARE @distance FLOAT = @R * @c;
  
  RETURN @distance;
END;